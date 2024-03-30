using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private EnemyMovement enemyMovement;
    private Animator anim;
    private bool inRange;

    private void Awake()
    {
        anim = GetComponentInParent<Animator>();
        enemyMovement = GetComponentInParent<EnemyMovement>();
    }

    private void Update()
    {
        if (inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            enemyMovement.Flip();
        }    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = false;
            gameObject.SetActive(false);
            enemyMovement.triggerArea.SetActive(true);
            enemyMovement.inRange = false;
            enemyMovement.SelectTarget();
        }
    }
}
