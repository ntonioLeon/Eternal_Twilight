using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.U2D;

public class EnemyMovement : MonoBehaviour{

    [Header("Stats")]
    float speed;
    Rigidbody2D rb;
    Animator anim;
    public float radioDeteccion;
    public bool estaEsperando;
    public Transform precipicio, paredes, suelo;
    bool sueloDetectado, precipicioDetectado, paredDetectada;
    public LayerMask queEsSuelo;

    [Header("Tipos Enemigo")]
    public bool esEstatico;

    //Correr
    public bool esCaminante;
    bool andaIzquierda;

    //Patrullar
    public bool patrulla;
    public GameObject puntoA, puntoB;
    public Transform objetivo;
    bool debeEsperar;
    public float tiempoQueEspera;

    //Perseguir
    public bool isChasing;

    [Header("Variables de ataque")]
    public float attackDistance; //Minimum distance for attack
    public float timer; //Timer for cooldown between attacks
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange; //Check if Player is in range    
    public GameObject triggerArea;
    public GameObject hotZone;
    

    private float distance; //Store the distance b/w enemy and player
    private bool attackMode;    
    private bool cooling; //Check if Enemy is cooling after attack
    private float intTimer;
    


    // Start is called before the first frame update
    void Start()
    {
        if (tiempoQueEspera > 0)
        {
            debeEsperar = true;
        }

        objetivo = puntoA.transform;
        speed = GetComponent<Enemy>().speed;
        rb = GetComponent<Rigidbody2D>();
        
    }

    private void Awake()
    {
        SelectTarget();
        intTimer = timer;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        precipicioDetectado = !Physics2D.OverlapCircle(precipicio.position, radioDeteccion, queEsSuelo);
        paredDetectada = Physics2D.OverlapCircle(paredes.position, radioDeteccion, queEsSuelo);
        sueloDetectado = Physics2D.OverlapCircle(suelo.position, radioDeteccion, queEsSuelo);

        if ((precipicioDetectado || paredDetectada) && sueloDetectado)
        {
            Flip();
        }

    }

    private void FixedUpdate()
    {
        if (esEstatico)
        {
            anim.SetBool("Moverse", false); //Puesto que empezamos en idle y pasamos a movimientiento esto es al revés.
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        if (esCaminante)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            anim.SetBool("Moverse", true);

            if (andaIzquierda)
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        if (patrulla)
        {
            Vector2 punto = objetivo.position - transform.position;

            if (objetivo == puntoA.transform)
            {
                if (!estaEsperando)
                {
                    anim.SetBool("Moverse", true);
                    rb.velocity = new Vector2(speed, rb.velocity.y);
                    transform.localScale = new Vector3(-1, 1, 1);
                }
            }
            else
            {
                if (!estaEsperando)
                {
                    anim.SetBool("Moverse", true);
                    rb.velocity = new Vector2(-speed, rb.velocity.y);
                    transform.localScale = new Vector3(1, 1, 1);
                }

            }

            if ((transform.position.x - puntoA.transform.position.x) >= -radioDeteccion && objetivo == puntoA.transform)
            {
                if (debeEsperar)
                {
                    StartCoroutine(Esperar());
                }

                Flip();
                objetivo = puntoB.transform;
            }

            if ((transform.position.x - puntoB.transform.position.x) <= radioDeteccion && objetivo == puntoB.transform)
            {
                if (debeEsperar)
                {
                    StartCoroutine(Esperar());
                }

                Flip();
                objetivo = puntoA.transform;
            }
        }

        /*
        if (isChasing)
        {
            if (isChasing && ((precipicioDetectado || paredDetectada) && sueloDetectado))
            {
                StartCoroutine(DejarDeChasear());    
            } 
            else
            {
                GoToTarget();
            }

            if (((precipicioDetectado || paredDetectada) && sueloDetectado))
            {
                esEstatico = true;
                isChasing = false;
            }
        }*/

        if (isChasing)
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
                EnemyLogic();
            }
        }        
    }

    public bool InsideOfLimits()
    {
        return transform.position.x > puntoA.transform.position.x && transform.position.x < puntoB.transform.position.x;
    }

    public void Move()
    {
        anim.SetBool("Moverse", true);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }

    public void EnemyLogic()
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

    public void Attack()
    {
        timer = intTimer; //Reset Timer when Player enter Attack Range
        attackMode = true; //To check if Enemy can still attack or not

        anim.SetBool("Moverse", false);
        anim.SetBool("Atacar", true);
    }

    public void Cooldown()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    public void StopAttack()
    {
        cooling = false;
        attackMode = false;
        anim.SetBool("Atacar", false);
    }

    public void Flip()
    {
        if (sueloDetectado)
        {
            if (andaIzquierda)
            {
                andaIzquierda = false;
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                andaIzquierda = true;
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

    }
    public void SelectTarget()
    {
        float distanceToLeft = Vector3.Distance(transform.position, puntoA.transform.position);
        float distanceToRight = Vector3.Distance(transform.position, puntoB.transform.position);

        if (distanceToLeft > distanceToRight)
        {
            target = puntoA.transform;
        }
        else
        {
            target = puntoB.transform;
        }

        //Ternary Operator
        //target = distanceToLeft > distanceToRight ? leftLimit : rightLimit;

        Flip();
    }    

    IEnumerator Esperar()
    {
        anim.SetBool("Moverse", false);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        estaEsperando = true;
        for (int i = 0; i < tiempoQueEspera; i++)
        {
            yield return new WaitForSeconds(1);

            Flip();
        }
        if (tiempoQueEspera % 2 == 1)
        {
            Flip();
        }
        estaEsperando = false;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        anim.SetBool("Moverse", true);
    }

    IEnumerator DejarDeChasear()
    {
        GetComponentInChildren<CircleCollider2D>().enabled = false;

        objetivo = null;
        esCaminante = true;        
        yield return new WaitForSeconds(1.5f);

        GetComponentInChildren<CircleCollider2D>().enabled = true;
    }

    private void GoToTarget()
    {
        if (objetivo.position.x < transform.position.x)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (objetivo.position.x > transform.position.x)
        {

            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }    
}
