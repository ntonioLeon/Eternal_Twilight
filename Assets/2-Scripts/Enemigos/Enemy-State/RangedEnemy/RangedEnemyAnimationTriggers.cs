using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAnimationTriggers : MonoBehaviour
{
    private RangedEnemy enemy => GetComponentInParent<RangedEnemy>();
    [SerializeField] GameObject proyectil;

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger()  //Cambiar a lanzamiento de proyectil.
    {
        
        Instantiate(proyectil, enemy.proyectilPos.position, Quaternion.identity);
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
