using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandidoMeleeAttackState : EnemyState
{
    private BandidoMelee enemy;

    public BandidoMeleeAttackState(Enemigo enemyBase, EnemyStateMachine stateMachine, string animBoolName, BandidoMelee enemy) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
