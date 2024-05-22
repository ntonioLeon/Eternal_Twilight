using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private string targetLayerName = "Player";

    [SerializeField] private float xVelocity;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private bool canMove;
    [SerializeField] private bool flipped;

    private CharacterStats myStats;

    private void Update()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(xVelocity, rb.velocity.y);
        }
        
    }

    public void SetupArrow(float speed, CharacterStats charactarStats)
    {
        xVelocity = speed;
        myStats = charactarStats;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            if (collision.GetComponent<CharacterStats>() != null)
            {
                //collision.GetComponent<CharacterStats>().TakeDamage(damage);
                myStats.DoDamage(collision.GetComponent<CharacterStats>());

                StuckInto(collision);
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                StuckInto(collision);
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {
        GetComponentInChildren<ParticleSystem>().Stop();
        GetComponent<CapsuleCollider2D>().enabled = false;
        
        canMove = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;

        Destroy(gameObject, Random.Range(5, 7));
    }

    public void FLipArrow()
    {
        if (flipped)
        {
            return;
        }

        xVelocity = xVelocity * -1;
        flipped = true;
        transform.Rotate(0,180,0);
        targetLayerName = "Enemy";
    }
}
