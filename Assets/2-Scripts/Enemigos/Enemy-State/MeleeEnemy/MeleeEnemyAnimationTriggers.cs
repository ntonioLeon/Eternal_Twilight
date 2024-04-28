using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAnimationTriggers : MonoBehaviour
{
    private MeleeEnemy enemy => GetComponentInParent<MeleeEnemy>();

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats player = hit.GetComponent<PlayerStats>();
                Debug.Log(player.name + "--"+player.nameChar); // Sabe a quien pega pero no pued epegar por que no estan bien puestos los niveles.
                enemy.stats.DoDamage(player);
               //EL LA SILENCIA// hit.GetComponent<Player>().Damage();
            }
        }
    }

    private void OpenCounterWindow()
    {
        enemy.OpenCounterAttackWindow();
    }

    private void CloseoCounterWindow()
    {
        enemy.CloseCounterAttackWindow();
    }
}
