using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : CharacterStats
{
    [SerializeField] private Text maxHealthT;
    [SerializeField] private Text magicResistT;
    [SerializeField] private Text fireDMGT;
    [SerializeField] private Text iceDMGT;
    [SerializeField] private Text lightDMGT;
    [SerializeField] private Text critPOT;
    [SerializeField] private Text critCT;
    [SerializeField] private Text damageT;
    [SerializeField] private Text armorT;
    [SerializeField] private Text evasionT;

    private Player player;
    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    protected override void Update()
    {
        base.Update();
        RefreshStats();
    }

    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);
    }

    public override void Die()
    {
        base.Die();
        AudioManager.instance.StopGameMusic();
        AudioManager.instance.PlayBattle();
        player.Die();
        GetComponent<PlayerItemDrop>().GenerateDrop();
    }

    public void RefreshStats()
    {
        maxHealthT.text = maxHealth.GetValue().ToString();
        magicResistT.text = magicResistance.GetValue().ToString();
        fireDMGT.text = fireDamage.GetValue().ToString();
        iceDMGT.text = iceDamage.GetValue().ToString();
        lightDMGT.text = lightningDamage.GetValue().ToString();
        critPOT.text = critPower.GetValue().ToString();
        critCT.text = critChance.GetValue().ToString();
        damageT.text = damage.GetValue().ToString();
        armorT.text = armor.GetValue().ToString();
        evasionT.text = evasion.GetValue().ToString();
    }
}
