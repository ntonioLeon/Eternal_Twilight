using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemigo enemy;

    [Header("Level details")]
    [SerializeField] private int level = 1;

    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier = .33f;

    protected override void Start()
    {
        ApplyLevelModifiers();

        base.Start();

        enemy = GetComponent<Enemigo>();
    }

    private void ApplyLevelModifiers()
    {
        //Modify(strength);  //Estos modifiers son muchos numeros, el balance deberia ser con las minors stats
        //Modify(agulity);
        //Modify(inteligence);
        //Modify(vitality);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);

        Modify(maxHealth);
        Modify(armor);
        Modify(magicResistance);
        Modify(evasion);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightningDamage);
    }

    private void Modify(Stat stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = stat.GetValue() * percentageModifier;

            stat.AddModifier(modifier);
        }
    }

    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);
    }

    public override void Die()
    {
        base.Die();

        enemy.Die();
    }
}

    
