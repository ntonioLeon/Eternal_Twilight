using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAreaCheck : MonoBehaviour
{
    private EnemyMovement enemyParent;

    private void Awake()
    {
        enemyParent = GetComponentInParent<EnemyMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            enemyParent.target = collision.transform;
            enemyParent.inRange = true;
            enemyParent.hotBox.SetActive(true);
            if (enemyParent.isFlyer)
            {
                enemyParent.chase = true;
                enemyParent.target = collision.gameObject.transform;
            }
        }
    }
}
