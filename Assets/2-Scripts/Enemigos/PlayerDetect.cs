using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetect : MonoBehaviour
{
    Enemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (enemy.isRanged)
            {
               // if (transform.GetComponentInParent<EnemyProyectil>().watcher && transform.GetComponentInParent<EnemyProyectil>().shootCooldown < 0)
                //{
                //    transform.GetComponentInParent<EnemyProyectil>().Shoot();
               // }
                    
            }
            else
            {
                if(GetComponent<EnemyMovement>().patrulla = true)
                GetComponent<EnemyMovement>().patrulla = true;
            }
        }
    }
}
