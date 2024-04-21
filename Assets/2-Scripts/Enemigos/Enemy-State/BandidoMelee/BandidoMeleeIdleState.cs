using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandidoMeleeIdleState : EnemyState
{
    private BandidoMelee enemy;

    public BandidoMeleeIdleState(Enemigo enemyNase, EnemyStateMachine stateMachine, string animBoolName, BandidoMelee enemy) : base(enemy, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.iddleTime;
        enemy.ZeroVelocity();
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
