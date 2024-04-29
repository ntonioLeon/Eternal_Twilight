using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;

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
    [SerializeField] protected LayerMask whatIsGround;
    #endregion

    public int facingDir { get; private set; } = 1;
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
    }

    protected virtual void Update()
    {
        
    }

    public virtual void DamageEffect()
    {
        fx.StartCoroutine("FlashFX");
        StartCoroutine("HitKnockBack");

        //Debug.Log(gameObject.name + " Was damaged!");
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

        rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
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
        return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    public virtual bool IsWallDetected()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
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

    public virtual void Die()
    {

    }
}
