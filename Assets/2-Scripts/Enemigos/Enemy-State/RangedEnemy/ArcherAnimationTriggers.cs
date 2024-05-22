using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAnimationTriggers : MonoBehaviour
{
    private Enemy_Archer enemy => GetComponentInParent<Enemy_Archer>();

    protected virtual void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    protected virtual void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats player = hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(player);
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

