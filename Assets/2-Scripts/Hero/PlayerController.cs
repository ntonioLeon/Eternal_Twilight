using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("Componentes")]
    public Rigidbody2D rb;
    private Animator anim;
    public SpriteRenderer sprite;

    [Header("Movimiento")]
    public float moveSpeed;
    private bool isRunning;

    [Header("Ground")]
    public Transform groundCheck;
    public LayerMask whatIsGround;
    private bool isGrounded;

    [Header("Stamina")]
    public float maxStamina;
    public float staminaRegen;
    public float staminaCost;
    public Image staminaImage;
    private float currentStamina;

    [Header("Salto")]
    public float jumpForce;
    private bool canDoubleJump;
    private bool hasDoubleJump;
    public float bounceForce;

    [Header("KnockBack")]
    public float knockBackLength;
    public float knockBackForce;
    private float knockBackCounter;
    public bool stopInput;

    [Header("Dash")]
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 25f;
    private float dashingTime = 0.4f;
    private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer tr;

    [Header("Bloqueo")]
    public float parryWindow;
    public bool isBlocking;
    public float blockTimer;
    private float blockDuration = 1f;

    [Header("Ataque")]
    public float weaponDamage;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponentInChildren<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();

        currentStamina = maxStamina;
        //UpdateStaminaUI();
        InvokeRepeating("RegenerateStamina", 1f, 1f);// esto no me gusta del todo
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.instance.isPaused && !stopInput)
        {
            if (knockBackCounter <= 0)
            {
                isGrounded = Physics2D.OverlapCircle(groundCheck.position, .2f, whatIsGround);

                if (isGrounded)
                {
                    canDoubleJump = true;
                }

                if (isDashing)
                {
                    return;
                }                 

                Walk();
                WalkingSound();// problemas en Pause;  

                Saltar();

                Dash();

                Attack();

                Block();
                UpdateBlock();

                Invertir();
                
            }
        }
        else
        {
            GetKnockBacked();
        }
        anim.SetBool("Jump", !isGrounded);
    }

    #region Block
    private void Block()
    {
        if (Input.GetKeyDown(KeyCode.P) && !isBlocking && isGrounded && !isDashing)
        {
            if (currentStamina >= staminaCost)
            {
                blockTimer = blockDuration;
                currentStamina -= staminaCost;
                StartCoroutine(BlockCoroutine());
            }
        }
    }

    private void UpdateBlock()
    {
        if (isBlocking)
        {
            blockTimer -= Time.deltaTime;
            if (blockTimer <= 0f)
            {
                // Finalizar el bloqueo
                isBlocking = false;
                anim.SetBool("Parry", false);
            }
        }
    }
    IEnumerator BlockCoroutine()
    {
        // Inicia el bloqueo
        isBlocking = true;
        anim.SetBool("Parry", true);
        //stopInput = true; esto lo manda a la puta

        // Espera durante 1 segundo
        yield return new WaitForSeconds(blockDuration);

        // Finaliza el bloqueo
        isBlocking = false;
        anim.SetBool("Parry", false);
        //stopInput = false; esto lo manda a la puta
    }
    #endregion

    private void WalkingSound()
    {
        if (isRunning && isGrounded)
        {
            // Debug.Log("Corriendo y grounded");
            // AudioManager.instance.PlaySFX(5);
        }
        else
        {
            // Debug.Log("XCorriendo y Xgrounded");
            // AudioManager.instance.StopSFX();
        }
    }

    #region Stamina
    void RegenerateStamina()
    {
        currentStamina += staminaRegen;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        UpdateStaminaUI();
    }

    void UpdateStaminaUI()
    {
        staminaImage.fillAmount = currentStamina / maxStamina;
    }
    #endregion

    #region KnockBack
    public void KnockBack()
    {
        knockBackCounter = knockBackLength;
        rb.velocity = new Vector2(0f, knockBackForce);
    }

    public void GetKnockBacked()
    {
        knockBackCounter -= Time.deltaTime;
        if (!sprite.flipX)
        {
            rb.velocity = new Vector2(-knockBackForce, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(knockBackForce, rb.velocity.y);
        }
    }
    #endregion

    #region Movimiento
    private void Walk()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            rb.velocity = new Vector2(moveSpeed * Input.GetAxisRaw("Horizontal"), rb.velocity.y);
            anim.SetBool("Run", true);
            isRunning = true; 
        }
        else
        {
            rb.velocity = new Vector2(moveSpeed * Input.GetAxisRaw("Horizontal"), rb.velocity.y);
            anim.SetBool("Run", false);
            isRunning = false;
        }
        
    }
    private void Invertir()
    {
        if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }        
    }

    private void Saltar()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                //AudioManager.instance.playSFX(10);
            }
            else
            {
                if (canDoubleJump)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    canDoubleJump = false;
                    //AudioManager.instance.playSFX(10);
                }
            }
        }
    }
    #endregion

    private void Attack()
    {
        if (Input.GetButtonDown("Fire1") && currentStamina>=(staminaCost*2) && isGrounded)
        {
            AudioManager.instance.PlaySFX(0);
            currentStamina -= staminaCost * 2;
            anim.SetBool("Attack", true);
        }
        else
        {
            anim.SetBool("Attack", false);
        }
    }

    #region Dash
    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && currentStamina >= (staminaCost * 10))
        {
            currentStamina -= staminaCost * 10;
            StartCoroutine(DashCourutine());
        }
    }
    IEnumerator DashCourutine()
    {
        Physics2D.IgnoreLayerCollision(10 ,9 , true);
        canDash = false;
        isDashing = true;
        tr.emitting = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;        
        
        if (sprite.flipX)
        {
            rb.velocity = new Vector2(-transform.localScale.x * dashingPower, 0f /*rb.position.y*/);
        } else
        {
            rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f /*rb.position.y*/);
        }      
        
        yield return new WaitForSeconds(dashingTime);

        tr.emitting = false;
        rb.gravityScale += originalGravity;
        isDashing = false;
        Physics2D.IgnoreLayerCollision(10, 9, false);

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    #endregion
}
