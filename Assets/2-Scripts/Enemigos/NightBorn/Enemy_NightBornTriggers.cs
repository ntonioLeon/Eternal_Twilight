using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_NightBornTriggers : MeleeEnemyAnimationTriggers
{
    private Enemy_NightBorn nightBorn => GetComponentInParent<Enemy_NightBorn>();

    protected override void AnimationTrigger()
    {
        nightBorn.AnimationFinishTrigger();
    }

    protected override void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(nightBorn.attackCheck.position, nightBorn.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats player = hit.GetComponent<PlayerStats>();
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                nightBorn.stats.DoDamage(player);
            }
        }
    }

    private void Relocate()
    {
        nightBorn.FindPosition();
    }

    private void MakeInvisible()
    {
        nightBorn.fx.MakeTransprent(true);
    }

    private void MakeVisible()
    {
        nightBorn.fx.MakeTransprent(false);
    }
}
