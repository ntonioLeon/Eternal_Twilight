using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStike_Controller : MonoBehaviour
{
    private PlayerStats playerStats;
    
    void Start()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemigo>() != null)
        { 
            EnemyStats enemyStats = collision.GetComponent<EnemyStats>();

            playerStats.DoMagicalDamage(enemyStats);
        }
    }
}
