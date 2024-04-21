using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GozuIdleState : BossState
{
    private Gozu boss;
    public GozuIdleState(Boss bossBase, BossStateMachine stateMachine, string animBoolName, Gozu boss) : base(boss, stateMachine, animBoolName)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        base.Enter();
        boss.ZeroVelocity();
        stateTimmer = 1f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(stateTimmer < 0)
        {
            stateMachine.ChangeState(boss.moveState);
        }
    }
}
