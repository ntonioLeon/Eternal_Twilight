using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    public float itemCooldown;
    public ItemEffect[] itemEffects;

    [Header("Major stats")]
    public float strength;
    public float agility;
    public float intelligence;
    public float vitality;

    [Header("Offensive stats")]
    public float damage;
    public float critChance;
    public float critPower;

    [Header("Defensive stats")]
    public float health;
    public float armor;
    public float magicResistance;
    public float evasion;

    [Header("Magic stats")]
    public float fireDamage;
    public float iceDamage;
    public float lightningDamage;

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials;

    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agulity.AddModifier(agility);
        playerStats.inteligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHealth.AddModifier(health);
        playerStats.currentHealth += health;
        playerStats.armor.AddModifier(armor);
        playerStats.magicResistance.AddModifier(magicResistance);
        playerStats.evasion.AddModifier(evasion);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightningDamage.AddModifier(lightningDamage);
    }

    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agulity.RemoveModifier(agility);
        playerStats.inteligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHealth.RemoveModifier(health);
        if (playerStats.currentHealth > health)
        {
            playerStats.currentHealth -= health;
        }
        else
        {
            playerStats.currentHealth = 1;
        }

        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightningDamage.RemoveModifier(lightningDamage);
    }

    public void Effect(Transform enemyPosition)
    {
        foreach (var item in itemEffects)
        {
            item.ExecutedEffect(enemyPosition);
        }
    }
}
