using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyIdleState : MeleeEnemyGroundState
{
    public MeleeEnemyIdleState(Enemigo enemyBase, EnemyStateMachine stateMachine, string animBoolName, MeleeEnemy enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
        enemy.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }
}
