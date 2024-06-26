using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        if (rb.velocity.y < 0 && !player.IsGroundDetected())
        {
            player.cayendo = true;
        }

        if (player.canDoubleJump && Input.GetKeyDown(KeyCode.Space))
        {
            player.canDoubleJump = false;
            stateMachine.ChangeState(player.jumpState);
            return;
        }

        if (player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallSlideSate);
        }

        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (xInput != 0)
        {
            player.SetVelocity(player.moveSpeed * .8f * xInput, rb.velocity.y);
        }
    }
}
