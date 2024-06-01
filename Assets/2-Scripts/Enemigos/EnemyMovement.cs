using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    #region Public Variables
    [HideInInspector] public bool isRanged;
    [HideInInspector] public bool isFlyer;
    [HideInInspector] public bool hasGuard;

    [Header("Puntos y localizaciones")]
    public Transform leftLimit;
    public Transform rightLimit;
    public Transform startPoint;
    public Transform precipicio;
    public Transform paredes;
    public Transform suelo;
    public Transform backPrecipicio;
    public Transform backParedes;
    public LayerMask queEsSuelo;

    [Header("Medidas de accion y cadencia")]
    public GameObject triggerArea;
    public GameObject hotBox;
    public float attackDistance;
    public float meleeZone;
    public float radioDeteccion;
    public float timer;
    [HideInInspector] public bool attackMode;
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange;
    [HideInInspector] public bool chase = false;
    #endregion

    #region Private Variables
    private Animator anim;
    private float distance; //Store the distance b/w enemy and player
    private bool cooling; //Check if Enemy is cooling after attack
    private float intTimer;
    private float moveSpeed;
    [HideInInspector] private Rigidbody2D rb;
    private bool sueloDetectado, precipicioDetectado, paredDetectada, backPrecipicioDetectado, backParedDetectada;
    private bool contraLaPared;
    private bool inStart;
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
        isFlyer = GetComponent<Enemy>().isFlyer;
        hasGuard = GetComponent<Enemy>().hasGuard;
        rb = GetComponent<Rigidbody2D>();
        shooted = false;
        contraLaPared = false;
        leftLimit.parent = null;
        rightLimit.parent = null;
        startPoint.parent = null;
    }

    void Update()
    {
        if (isFlyer)
        {
            if (target == null)
                return;

            if (chase)
            {
                FlyerLogic();
            }
            else
            {
                ReturnEnemyToStartingPosition();
            }
            Flip();
        }
        else if (TerrenoTransitable())
        {
            ResetPatroll();
        }
        else if (ObstaculoALaEspalda() && !contraLaPared)
        {
            StopMoving();
        }
        else
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
    }

    #region Ranged
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
            if (!contraLaPared)
            {
                RunFromTarget();
            }
            else
            {
                anim.SetBool("Moverse", false);
                anim.SetBool("Atacar", false);
            }
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
        timer = intTimer; //Reset Timer when Player enter Attack Range
        attackMode = true; //To check if Enemy can still attack or not

        anim.SetBool("Moverse", false);
        anim.SetBool("Atacar", true);
    }

    public void Disparo()
    {
        Instantiate(proyectil, proyectilPos.position, Quaternion.identity);
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
    #endregion

    #region Melee
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
        else if (distance > attackDistance && attackMode)
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
    #endregion

    #region Comportamientos comunes
    public void FlyerLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);

        if (attackDistance >= distance && !cooling)
        {
            Attack();
        }
        else if (distance > attackDistance)
        {
            if (hasGuard)
            {
                anim.SetBool("Guardia", false);
            }
            Chase();
        }
        else if (distance <= attackDistance && cooling)
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

    private bool CheckStart()
    {
        return Vector2.Distance(transform.position, startPoint.position) < 0.5f;
    }

    private void ReturnEnemyToStartingPosition()
    {
        if (CheckStart())
        {
            anim.SetBool("Moverse", false);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, startPoint.position, moveSpeed * Time.deltaTime);
            anim.SetBool("Moverse", true);
        }
    }

    private void Chase()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        anim.SetBool("Moverse", true);
    }

    void Move()
    {
        anim.SetBool("Moverse", true);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }

        if (!ObstaculoALaEspalda())
        {
            contraLaPared = false;
        }
    }

    void Cooldown()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && cooling)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    void StopAttack()
    {
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

    private bool TerrenoTransitable()
    {
        precipicioDetectado = !Physics2D.OverlapCircle(precipicio.position, radioDeteccion, queEsSuelo);
        paredDetectada = Physics2D.OverlapCircle(paredes.position, radioDeteccion, queEsSuelo);
        sueloDetectado = Physics2D.OverlapCircle(suelo.position, radioDeteccion, queEsSuelo);

        return ((precipicioDetectado || paredDetectada) && sueloDetectado);
    }

    private void ResetPatroll()
    {
        StartCoroutine(ResetPatrollCoroutine());
    }

    IEnumerator ResetPatrollCoroutine()
    {
        GetComponentInChildren<HotZoneCheck>().OnTriggerExit2D(target.GetComponentInChildren<Collider2D>());
        hotBox.SetActive(false);

        yield return new WaitForSeconds(2f);
    }

    private bool ObstaculoALaEspalda()
    {
        if (isRanged)
        {
            backPrecipicioDetectado = !Physics2D.OverlapCircle(backPrecipicio.position, radioDeteccion, queEsSuelo);
            backParedDetectada = Physics2D.OverlapCircle(backParedes.position, radioDeteccion, queEsSuelo);

            return (backPrecipicioDetectado || backParedDetectada);
        }
        else
        {
            return false;
        }
    }

    private void StopMoving()
    {
        anim.SetBool("Moverse", false);
        contraLaPared = true;
    }
    #endregion
}
