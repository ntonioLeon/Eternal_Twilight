using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    [Header("Bounce Info")]
    [SerializeField] private float bounceSpeed;
    private bool isBouncing;
    private int amountOfBounds;
    private List<Transform> enemyTarget;
    private int targetIndex;

    [Header("Pierce Info")]
    private int pierceAmount;

    [Header("Spin Info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;

    private float hitTimer;
    private float hitCooldown;

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
        //Aqui falta algo pero no me gusta y no me sale
        anim.SetBool("Rotation", true);
        
    }

    public void SetUpBounce(bool isBoun, int amountOfBoun)
    {
        isBouncing = isBoun;
        amountOfBounds = amountOfBoun;

        enemyTarget = new List<Transform>();
    }

    public void SetUpPierce(int amountOfPiercing)
    {
        pierceAmount = amountOfPiercing;
    }

    public void SetUpSpin(bool itsSpinning, float maxTravel, float spintDur, float hitCD)
    {
        isSpinning = itsSpinning;
        maxTravelDistance = maxTravel;
        spinDuration = spintDur;
        hitCooldown = hitCD;
    }

    public void ReturnSword()
    {

        Physics2D.IgnoreLayerCollision(10, 3, false);

        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;
        transform.parent = null;

        isReturning = true;

    }

    private void Update()
    {
        Debug.Log(spinTimer);
        if (canRotate)
        {
            transform.right = rb.velocity;
        }

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returningSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.CatchTheSword();
            }
        }
        BounceLogic();

        SpinningLogic();
    }

    private void SpinningLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;

                if (hitTimer < 0)
                {
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemigo>() != null) //Add enemy
                        {
                            hit.GetComponent<Enemigo>().Damage();
                        }
                    }
                }
            }
            else 
            {
                hitTimer = hitCooldown;
            }
        }
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .5f)
            {
                enemyTarget[targetIndex].GetComponent<Enemigo>().Damage();
                targetIndex++;
                amountOfBounds--;

                if (amountOfBounds <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count)
                {
                    targetIndex = 0;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
        {
            return;
        }
        collision.GetComponent<Enemigo>()?.Damage();
        SetUpTargetForBouncing(collision);
    }

    private void SetUpTargetForBouncing(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Ground"))
        {
            if (collision.GetComponent<Enemigo>() != null) // Add Enemy
            {
                if (isBouncing && enemyTarget.Count <= 0)
                {
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemigo>() != null) //Add enemy
                        {
                            enemyTarget.Add(hit.transform);
                        }
                    }
                }
            }
            StuckInto(collision);
        }
    }

    private void StuckInto(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemigo>() != null) 
        {
            pierceAmount--;
            return;
        }

        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }

        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count >0)
        {
            return;
        }
        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
