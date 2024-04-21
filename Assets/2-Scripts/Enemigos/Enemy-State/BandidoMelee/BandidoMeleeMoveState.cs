using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandidoMeleeMoveState : BandidoMeleeGroundState
{
    public BandidoMeleeMoveState(Enemigo enemyBase, EnemyStateMachine stateMachine, string animBoolName, BandidoMelee enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.velocity.y);

        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
