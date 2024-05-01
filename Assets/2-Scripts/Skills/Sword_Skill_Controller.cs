using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Sword_Skill_Controller : MonoBehaviour
{
    private float returningSpeed;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;
    private float freezeTimeDuration;

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
    private float spinDirection;

    private float hitTimer;
    private float hitCooldown;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void SetUpSword(Vector2 dir, float gravityScale, Player newPlayer, float freezeTimeDur, float retSpeed)
    {
        player = newPlayer;
        freezeTimeDuration = freezeTimeDur;
        returningSpeed = retSpeed;

        rb.velocity = dir;
        rb.gravityScale = gravityScale;
        //Aqui falta algo pero no me gusta y no me sale
        anim.SetBool("Rotation", true);

        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

        //Invoke("DestroyMe", 7); el usa esta, prefiero la otra
        Invoke("ReturnSword", 7); 
    }

    public void SetUpBounce(bool isBoun, int amountOfBoun, float bounSpeed)
    {
        isBouncing = isBoun;
        amountOfBounds = amountOfBoun;
        bounceSpeed = bounSpeed;

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

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);

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
                            DamageEnemy(hit.GetComponent<Enemigo>());
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
                DamageEnemy(enemyTarget[targetIndex].GetComponent<Enemigo>());
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
        if (collision.GetComponent<Enemigo>() != null)
        {
            Enemigo enemy = collision.GetComponent<Enemigo>();
            DamageEnemy(enemy);

        }
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

    private void DamageEnemy(Enemigo enemy)
    {
        player.stats.DoDamage(enemy.GetComponent<CharacterStats>());
        enemy.StartCoroutine("FreezeTimeFor", freezeTimeDuration);
    }
}
