using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideSate : PlayerState
{
    public PlayerWallSlideSate(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        if (!player.IsWallDetected() && !player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }

        if (xInput != 0 && player.facingDir != xInput)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (yInput < 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else 
        {
            rb.velocity = new Vector2(0, rb.velocity.y * 0.65f);
        }        

        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
