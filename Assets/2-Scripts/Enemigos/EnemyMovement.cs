using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.U2D;

public class EnemyMovement : MonoBehaviour{

    #region Public Variables
    public float attackDistance; //Minimum distance for attack
    
    public float timer; //Timer for cooldown between attacks
    public Transform leftLimit;
    public Transform rightLimit;

    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange; //Check if Player is in range    
    public GameObject triggerArea;
    public GameObject hotBox;
    #endregion

    #region Private Variables
    private Animator anim;
    private float distance; //Store the distance b/w enemy and player
    private bool attackMode;    
    private bool cooling; //Check if Enemy is cooling after attack
    private float intTimer;
    private float moveSpeed;
    #endregion

    void Awake()
    {
        SelectTarget();
        intTimer = timer; //Store the inital value of timer
        anim = GetComponent<Animator>();
        moveSpeed = GetComponent<Enemy>().speed;
    }

    void Update()
    {
        if (!attackMode)
        {
            Move();
        }

        if (!InsideOfLimits() && !inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (GetComponent<Enemy>().isRanged)
            {
                // RunFromTarget();
            }
            else
            {
                SelectTarget();
            }
        }

        if (inRange)
        {
            if (GetComponent<Enemy>().isRanged)
            {
                // RangedLogic();
            }
            else
            {
                EnemyLogic();
            }   
        }
        
    }

    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);

        if (distance > attackDistance)
        {
            StopAttack();
        }
        else if (attackDistance >= distance && cooling == false)
        {
            Attack();
        }

        if (cooling)
        {
            Cooldown();
            anim.SetBool("Atacar", false);
        }
    }

    void RangedLogic()
    {
        //Shoot();
        //Cooldown();
        //KeepRunning();
    }

    void Move()
    {
        anim.SetBool("Moverse", true);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void Attack()
    {
        timer = intTimer; //Reset Timer when Player enter Attack Range
        attackMode = true; //To check if Enemy can still attack or not

        anim.SetBool("Moverse", false);
        anim.SetBool("Atacar", true);
    }

    void Cooldown()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    void StopAttack()
    {
        cooling = false;
        attackMode = false;
        anim.SetBool("Atacar", false);
    }

    public void TriggerCooling()
    {
        cooling = true;
    }

    private bool InsideOfLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    public void SelectTarget()
    {
        float distanceToLeft = Vector3.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector3.Distance(transform.position, rightLimit.position);

        if (distanceToLeft > distanceToRight)
        {
            target = leftLimit;
        }
        else
        {
            target = rightLimit;
        }

        //Ternary Operator
        //target = distanceToLeft > distanceToRight ? leftLimit : rightLimit;

        Flip();
    }

    public void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        if (transform.position.x > target.position.x)
        {
            rotation.y = 0;
        }
        else
        {
            
            rotation.y = 180;
        }

        //Ternary Operator
        //rotation.y = (currentTarget.position.x < transform.position.x) ? rotation.y = 180f : rotation.y = 0f;

        transform.eulerAngles = rotation;
    }
}
