using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandidoMeleeIdleState : BandidoMeleeGroundState
{
    public BandidoMeleeIdleState(Enemigo enemyBase, EnemyStateMachine stateMachine, string animBoolName, BandidoMelee enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
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
