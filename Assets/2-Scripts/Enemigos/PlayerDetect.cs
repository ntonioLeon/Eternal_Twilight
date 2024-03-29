using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetect : MonoBehaviour
{
    Enemy enemy;
    

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Patatas entrando");

            if (!enemy.isRanged)
            {
                GetComponentInParent<EnemyMovement>().esEstatico = false;
                GetComponentInParent<EnemyMovement>().esCaminante = false;
                GetComponentInParent<EnemyMovement>().patrulla = false;
                GetComponentInParent<EnemyMovement>().isChasing = true;

                GetComponentInParent<EnemyMovement>().objetivo = collision.transform;
            } else
            {
                /*if (transform.GetComponentInParent<EnemyProyectil>().watcher && transform.GetComponentInParent<EnemyProyectil>().shootCooldown < 0)
                {
                    transform.GetComponentInParent<EnemyProyectil>().Shoot();
                }*/
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Patatas saliendo");

            if (!enemy.isRanged)
            {
                GetComponentInParent<EnemyMovement>().esEstatico = false;
                GetComponentInParent<EnemyMovement>().esCaminante = true;
                GetComponentInParent<EnemyMovement>().patrulla = false;
                GetComponentInParent<EnemyMovement>().isChasing = false;
            }
        }
    }
}
