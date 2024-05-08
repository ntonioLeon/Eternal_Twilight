using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.Rendering;

public class Entity : MonoBehaviour
{
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx { get; private set; }
    public SpriteRenderer sr { get; private set; }  
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D capsule { get; private set; }
    #endregion    

    #region Collision Variables
    [Header("KnockBack info")]
    [SerializeField] protected Vector2 knockBackDirection;
    [SerializeField] protected float knockBackDuration;
    protected bool isKnocked;
    #endregion

    #region Collision Variables
    [Header("Collision info")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] public float pendienteCheckDistance;
    [SerializeField] public LayerMask whatIsGround;
    [SerializeField] public bool isWatered = false;
    #endregion

    public int facingDir { get; private set; } = 1;

    public bool isPaused=false;
    private Vector2 realVel;
    protected bool facingRight = true;

    public System.Action onFlipped;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        fx = GetComponent<EntityFX>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();
        capsule = GetComponent<CapsuleCollider2D>();
        realVel = GetComponent<Rigidbody2D>().velocity;       
    }

    protected virtual void Update()
    {
        
    }

    public virtual void SlowEntityBy(float slowPercentage, float slowDuration)
    {

    }

    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }

    public virtual void DamageImpact()
    {
        StartCoroutine("HitKnockBack");
    }

    protected virtual IEnumerator HitKnockBack()
    {
        isKnocked = true;
        rb.velocity = new Vector2(knockBackDirection.x * -facingDir, knockBackDirection.y);
        yield return new WaitForSeconds(knockBackDuration);
        isKnocked = false;
    }

    #region Velocity
    public void SetZeroVelocity()
    {
        if (isKnocked)
        {
            return;
        }

        rb.velocity = new Vector2(0, 0);
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnocked)
        {
            return;
        }
        FlipController(xVelocity);
        rb.velocity = new Vector2(xVelocity, yVelocity);  
    }

    public void Retroceder(float xVelocity, float yVelocity)
    {
        if (isKnocked)
        {
            return;
        }

        rb.velocity = new Vector2(xVelocity, yVelocity);
    }
    #endregion

    #region Collision
    public virtual bool IsGroundDetected()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckDistance, whatIsGround);
    }

    public virtual bool IsWallDetected()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckDistance);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion

    #region Flip
    public virtual void Flip()
    {
        facingDir *=- 1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if (onFlipped != null)
        {
            onFlipped();
        }
    }

    public virtual void FlipController(float direccion)
    {
        if (direccion > 0 && !facingRight)
        {
            Flip();
        }
        else if (direccion < 0 && facingRight)
        {
            Flip();
        }
    }
    #endregion

    public virtual void Stop(bool paused)
    {
        Debug.Log("parado");
        if (paused)
        {
            rb.velocity = rb.velocity * 0 ;
        }
        else
        {
            rb.velocity = realVel;
        }
    }
    public virtual void Die()
    {

    }
}
