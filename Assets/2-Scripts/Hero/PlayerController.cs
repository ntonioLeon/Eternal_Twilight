using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("Componentes")]
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    [Header("Movimiento")]
    public float moveSpeed;

    [Header("Ground")]
    public Transform groundCheck;
    public LayerMask whatIsGround;
    private bool isGrounded;

    [Header("Salto")]
    public float jumpForce;
    private bool canDoubleJump;
    private bool hasDoubleJump;
    public float bounceForce;

    [Header("KnockBack")]
    public float knockBackLength, knockBackForce;
    private float knockBackCounter;
    public bool stopInput;

    [Header("Dash")]
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 25f;
    private float dashingTime = 0.4f;
    private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer tr;

    [Header("Parry")]
    private bool canParry;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.instance.isPaused && !stopInput)
        {
            if (knockBackCounter <= 0)
            {
                if (isDashing)
                {
                    return;
                }                 

                if (Input.GetAxisRaw("Horizontal") != 0)
                {
                    AudioManager.instance.PlaySFX(6);
                    rb.velocity = new Vector2(moveSpeed * Input.GetAxisRaw("Horizontal"), rb.velocity.y);
                    anim.SetBool("Run", true);
                } else
                {
                    rb.velocity = new Vector2(moveSpeed * Input.GetAxisRaw("Horizontal"), rb.velocity.y);
                    anim.SetBool("Run", false);
                }
                

                isGrounded = Physics2D.OverlapCircle(groundCheck.position, .2f, whatIsGround);

                if (isGrounded)
                {
                    canDoubleJump = true;
                }                

                Saltar();

                if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
                {
                    StartCoroutine(Dash());
                }

                Attack();

                //Parry();

                Invertir();
            }
        }
        else
        {
            GetKnockBacked();
        }

        anim.SetBool("Jump", !isGrounded);
    }

    /*private void Parry()
    {
        if (Input.GetButtonDown("Fire2") && canParry)
        {
            Debug.Log("Parry");
            StartCoroutine(Parrying());
        }
    }
    */
    public void KnockBack()
    {
        knockBackCounter = knockBackLength;
        rb.velocity = new Vector2(0f, knockBackForce);
    }

    private void Invertir()
    {
        if (rb.velocity.x < 0)
        {
            sprite.flipX = true;
        }
        else if (rb.velocity.x > 0)
        {
            sprite.flipX = false;
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

    private void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetBool("Attack", true);
        }
        else
        {
            anim.SetBool("Attack", false);
        }
    }
    IEnumerator Parrying()
    {
        stopInput = true;   
        yield return new WaitForSeconds(3);
        stopInput = false;
    }

    IEnumerator Dash()
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
}
