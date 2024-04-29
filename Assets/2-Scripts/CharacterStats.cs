using System;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major stats")]
    public Stat strenght;           // 1 punto == 1 en damage y 1% de crit.power
    public Stat agulity;            // 1 punto == 1% evasion y 1% de crit.chance
    public Stat inteligence;        // 1 punto == 3 de magic damage y 3 de magic resistance
    public Stat vitality;           // 1 punto == aumento de vida en [3-5]

    [Header("Ofensive stats")]
    public Stat damage;
    public Stat critChance;         
    public Stat critPower;          // Default 150%

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;

    [Header("Character info")]
    public string nameChar;

    [SerializeField] private float currentHealth;
    
    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = maxHealth.GetValue();
        //damage.AddModifier(5.5f);
        //damage.AddModifier(5.5f);
        //damage.AddModifier(5.5f);
    }

    public virtual void DoDamage(CharacterStats character)
    {
        //Comprobar si esquiva el ataque
        if (TargetCanAvoidAttack(character))
        {
            return;
        }

        //Calcular el daño recibido
        float totalDMG = damage.GetValue() + strenght.GetValue();

        //Calcular critico
        if (CanCrit())
        {
            totalDMG = CalculateCriticalDamage(totalDMG);
        }

        //Reducirlo por la armadura
        totalDMG = CheckTargetArmor(character, totalDMG);

        //Aplicar el daño
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

    public virtual void Die()
    {
        //Destroy(gameObject);
    }
    private float CheckTargetArmor(CharacterStats character, float totalDMG)
    {
        totalDMG -= character.armor.GetValue();
        totalDMG = Mathf.Clamp(totalDMG, 0, int.MaxValue);
        return totalDMG;
    }

    private bool TargetCanAvoidAttack(CharacterStats character)
    {
        float totalEvasion = character.evasion.GetValue() + character.agulity.GetValue();

        if (UnityEngine.Random.Range(0, 100) < totalEvasion)
        {
            return true;

        }

        return false;
    }

    private bool CanCrit()
    {
        float totalCriticalChance = critChance.GetValue() + agulity.GetValue();

        if (UnityEngine.Random.Range(0, 100) <= totalCriticalChance)
        {
            return true; 
        }

        return false;
    }

    private float CalculateCriticalDamage(float damage)
    {
        float totalCritPower = (critPower.GetValue() + strenght.GetValue()) * 0.01f;

        float critDamage = damage * totalCritPower;

        return critDamage;
    }
}
