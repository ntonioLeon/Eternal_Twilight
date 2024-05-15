using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();        

        player.skill.clone.CreateClone(player.transform);

        stateTimer = player.dashDuration;

        Physics2D.IgnoreLayerCollision(10, 9, true);
    }

    public override void Exit()
    {
        base.Exit();        

        player.SetVelocity(0, rb.velocity.y);

        Physics2D.IgnoreLayerCollision(10, 9, false);
    }

    public override void Update()
    {
        base.Update();

        if (player.isSpeaking) { 
            stateMachine.ChangeState(player.idleState);
        }

        if (!player.IsGroundDetected() && player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallSlideSate);
        }

        player.SetVelocity(player.dashSpeed * player.dashDir, 0);

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
