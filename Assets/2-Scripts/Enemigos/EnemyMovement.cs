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
    [HideInInspector] public bool attackMode;
    [HideInInspector] public bool isRanged;
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange; //Check if Player is in range    
    public GameObject triggerArea;
    public GameObject hotBox;
    public float meleeZone;
    public bool hasGuard;
    #endregion

    #region Private Variables
    private Animator anim;
    private float distance; //Store the distance b/w enemy and player
    private bool cooling; //Check if Enemy is cooling after attack
    private float intTimer;
    private float moveSpeed;
    [HideInInspector] private Rigidbody2D rb;
    #endregion

    #region proyectil
    public GameObject proyectil;
    public Transform proyectilPos;
    public float shootSpeed;
    public float fixHigh;
    private bool shooted;


    #endregion

    void Awake()
    {
        SelectTarget();
        intTimer = timer; //Store the inital value of timer
        anim = GetComponent<Animator>();
        moveSpeed = GetComponent<Enemy>().speed;
        isRanged = GetComponent<Enemy>().isRanged;
        rb = GetComponent<Rigidbody2D>();
        shooted = false;
    }

    void Update()
    {
        if (!attackMode)
        {
            Move();                
        }

        if (!InsideOfLimits() && !inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            SelectTarget();
        }

        if (inRange)
        {
            if (isRanged)
            {
                RangedLogic();
            }
            else
            {
                EnemyLogic();
            }   
        }
    }

    void RangedLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);

        if (distance > attackDistance && !attackMode)
        {
            StopAttack();
        }
        else if (attackDistance >= distance && !cooling)
        {           
            if (!shooted)
            {
                Shoot();
            }            
        }
        else if (attackDistance >= distance && cooling)
        {
            RunFromTarget();
        }
        else if (attackDistance < distance && attackMode && !cooling)
        {
            Move();
        }

        if (cooling)
        {
            Cooldown();
            anim.SetBool("Atacar", false);
            if (isRanged && timer < 0)
            {
                cooling = false;
            }

            if (distance >= attackDistance)
            {
                anim.SetBool("Moverse", false);
            }
        }
    }

    void Shoot()
    {        
        Disparo();

        timer = intTimer; //Reset Timer when Player enter Attack Range
        attackMode = true; //To check if Enemy can still attack or not

        anim.SetBool("Moverse", false);
        anim.SetBool("Atacar", true);
    }

    void Disparo()
    {        
        GameObject disparo = Instantiate(proyectil, proyectilPos.position, Quaternion.identity);
        shooted = true;
    }

    void RunFromTarget()
    {
        if (Vector2.Distance(transform.position, target.transform.position) <= attackDistance)
        {

            if ((transform.position.x - target.transform.position.x) <= 0) //player a la izquierda
            {
                // + 
                anim.SetBool("Atacar", false);
                anim.SetBool("Moverse", true);
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            }
            else
            {
                // -
                anim.SetBool("Atacar", false);
                anim.SetBool("Moverse", true);
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            }
        }
        else
        {
            anim.SetBool("Atacar", false);
            anim.SetBool("Moverse", false);
        }
    }

    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);

        if (distance > attackDistance && !attackMode)
        {
            StopAttack();
        }
        else if (attackDistance >= distance && !cooling)
        {
            Attack();
        }
        else if (distance > attackDistance && attackMode && cooling)
        {
            if (hasGuard)
            {
                anim.SetBool("Guardia", false);
            }
            Move();
        }
        else if (distance <= attackDistance && attackMode && cooling)
        {
            anim.SetBool("Moverse", false);
            if (hasGuard)
            {
                anim.SetBool("Guardia", true);
            }
        }
            if (cooling)
        {
            Cooldown();
            anim.SetBool("Atacar", false);
        }
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
        if (hasGuard)
        {
            anim.SetBool("Guardia", false);
        }
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
        if (!isRanged)
        {
            //cooling = false;
            //attackMode = false;
        }       
        
        anim.SetBool("Atacar", false);
    }

    public void TriggerCooling()
    {
        cooling = true;
        shooted = false;
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
