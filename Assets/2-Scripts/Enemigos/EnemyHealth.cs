using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    #region Public Variables
    [Header("Components")]
    public float originalHealth;
    public bool recibeDano;
    public GameObject deathEffect;
    #endregion

    #region Private Variables
    private SpriteRenderer render;
    private Blink material;
    private Rigidbody2D rb;
    private Enemy enemy;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();

        render= GetComponentInChildren<SpriteRenderer>();
        material = GetComponent<Blink>();

        originalHealth = enemy.healthPoints;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon") && !recibeDano)
        {
            /*
            enemy.healthPoints -= collision.GetComponentInParent<PlayerController>().weaponDamage;
            */
            enemy.healthPoints -= 5f; // temporal


            if (collision.transform.position.x < transform.position.x)
            {
                rb.AddForce(new Vector2(enemy.knockbackForceX, enemy.knockbackForceY), ForceMode2D.Force);
            }else 
            {
                rb.AddForce(new Vector2(-enemy.knockbackForceX, enemy.knockbackForceY), ForceMode2D.Force);
            }


            StartCoroutine(Damager());
            if (enemy.healthPoints<=0) 
            {
                Instantiate(deathEffect, transform.position, Quaternion.identity);
                ExperienceScript.instance.ExpModifier(GetComponent<Enemy>().expToGive);
                if (enemy.shouldRespawn)
                {
                    transform.GetComponentInParent<EnemyRespawn>().StartCoroutine(GetComponentInParent<EnemyRespawn>().RespawnEnemy());
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    
    public void TakeDamage(Transform collision, float damage)
    {
        enemy.healthPoints -= damage; // falta weapon

        if (collision.transform.position.x < transform.position.x)
        {
            rb.AddForce(new Vector2(enemy.knockbackForceX, enemy.knockbackForceY), ForceMode2D.Force);
        }
        else
        {
            rb.AddForce(new Vector2(-enemy.knockbackForceX, enemy.knockbackForceY), ForceMode2D.Force);
        }


        StartCoroutine(Damager());
        if (enemy.healthPoints <= 0)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            ExperienceScript.instance.ExpModifier(GetComponent<Enemy>().expToGive);
            if (enemy.shouldRespawn)
            {
                transform.GetComponentInParent<EnemyRespawn>().StartCoroutine(GetComponentInParent<EnemyRespawn>().RespawnEnemy());
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator Damager()
    {
        recibeDano = true;
        render.material = material.parpadeo;
        yield return new WaitForSeconds(.5f);
        render.material = material.original;
        recibeDano= false;
    }
}
