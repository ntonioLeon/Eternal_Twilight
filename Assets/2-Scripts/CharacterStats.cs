using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat strenght;
    public Stat damage;
    public Stat maxHealth;
    public string nameChar;

    [SerializeField] private float currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();
        //damage.AddModifier(5.5f);
        //damage.AddModifier(5.5f);
        //damage.AddModifier(5.5f);
    }

    public virtual void DoDamage(CharacterStats character)
    {
        float totalDMG = damage.GetValue()+strenght.GetValue();

        Debug.Log(totalDMG);

        character.TakeDamage(totalDMG);
    }

    public virtual void TakeDamage(float dmg)
    {
        currentHealth -= dmg;

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual private void Die()
    {
        Destroy(gameObject);
    }
}
