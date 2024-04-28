using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void  AnimationTrigger()
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
                hit.GetComponent<Boss>().Damage();
                hit.GetComponent<CharacterStats>().TakeDamage(player.stats.damage);
            }
            else if (hit.GetComponent<Enemigo>() != null)
            {
                hit.GetComponent<Enemigo>().Damage();
                Debug.Log("Deberia recibir daño de: " + player.stats.damage + " " + player.stats.nameChar);
                hit.GetComponent<CharacterStats>().TakeDamage(player.stats.damage); //aqui no funciona
            }
        }
    }
    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
