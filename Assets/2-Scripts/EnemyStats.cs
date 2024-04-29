using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemigo enemy;
    protected override void Start()
    {
        base.Start();

        enemy = GetComponent<Enemigo>();
    }

    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);

        enemy.DamageEffect();
    }

    public override void Die()
    {
        base.Die();

        enemy.Die();
    }
}

    
