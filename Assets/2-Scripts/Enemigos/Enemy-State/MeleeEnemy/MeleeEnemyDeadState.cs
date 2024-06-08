using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyDeadState : EnemyState
{
    private MeleeEnemy enemy;
    public MeleeEnemyDeadState(Enemigo enemyBase, EnemyStateMachine stateMachine, string animBoolName, MeleeEnemy enemy) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        //enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        //enemy.anim.speed = 0;
        //enemy.capsule.enabled = false;

        stateTimer = .1f;
    }

    public override void Update()
    {
        base.Update();

        enemy.Morir();
        enemy.SelfDestroy();
    }
}
