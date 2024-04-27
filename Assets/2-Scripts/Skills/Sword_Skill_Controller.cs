using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    [SerializeField] private float returningSpeed= 12;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    public void SetUpSword(Vector2 dir, float gravityScale, Player newPlayer)
    {
        player = newPlayer;

        rb.velocity = dir;
        rb.gravityScale = gravityScale;

        anim.SetBool("Rotation", true);
    }
    public void ReturnSword()
    {
        Physics2D.IgnoreLayerCollision(10, 3, false);

        rb.isKinematic = false;
        transform.parent = null;

        isReturning = true;

    }

    private void Update()
    {
        if (canRotate)
        {
            transform.right = rb.velocity;
        }

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returningSpeed * Time.deltaTime);

            if(Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.ClearSword();
            }            
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.CompareTag("Enemy") || collision.CompareTag("Ground"))
        {
            anim.SetBool("Rotation", false);

            canRotate = false;
            cd.enabled = false;

            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

            transform.parent = collision.transform;
        }
       
    }
}
