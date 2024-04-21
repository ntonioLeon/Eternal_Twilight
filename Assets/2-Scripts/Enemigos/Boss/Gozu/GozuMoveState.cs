using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GozuMoveState : BossState
{
    private Gozu boss;
    public GozuMoveState(Boss bossBase, BossStateMachine stateMachine, string animBoolName, Gozu boss) : base(bossBase, stateMachine, animBoolName)
    {
        this.boss = boss;
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

        boss.SetVelocity(boss.moveSpeed * boss.facingDir, boss.rb.velocity.y);

        if (boss.IsWallDetected())
        {
            boss.Flip();
            stateMachine.ChangeState(boss.idleState);
        }
    }
}
