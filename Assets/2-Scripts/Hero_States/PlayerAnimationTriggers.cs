using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        player.AnimacionTriiger();
    }

    private void AttackTrigger()
    {
        Collider2D[] coliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in coliders)
        {
            if (hit.GetComponent<Boss>() != null)
            {
                //Inventory.instance.GetEquipment(EquipmentType.Weapon).ExecuteItemEffect();
                //EnemyStats target = hit.GetComponent<EnemyStats>();
                //player.stats.DoDamage(target);
                //hit.GetComponentInParent<CharacterStats>().TakeDamage(player.stats.damage.GetValue());
            }
            else if (hit.GetComponent<Enemigo>() != null)
            {

                //EL LA BORRA// Debug.Log(" recibe  da�o de: " + player.stats.damage.GetValue() + " " + player.stats.nameChar);  ///////
                //EL LA BORRA// hit.GetComponentInParent<CharacterStats>().TakeDamage(player.stats.damage.GetValue()); //////// esto deberia esta al mismo nivel no uno por encima
                EnemyStats target = hit.GetComponent<EnemyStats>();
                player.stats.DoDamage(target);

                ItemData_Equipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);        

                if (weaponData != null)
                {
                    weaponData.Effect(target.transform);
                }
            }
        }
    }

    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
