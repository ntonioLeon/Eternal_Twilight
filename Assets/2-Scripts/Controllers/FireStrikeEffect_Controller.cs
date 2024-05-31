using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStrikeEffect_Controller : ThunderStike_Controller
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemigo>() != null)
        {
            PlayerStats playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
            EnemyStats enemyStats = collision.GetComponent<EnemyStats>();
            playerStats.DoMagicalDamage(enemyStats);
        }
    }
}
