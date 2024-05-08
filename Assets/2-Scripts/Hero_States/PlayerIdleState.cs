using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (xInput == player.facingDir && player.IsWallDetected())
        {
            Debug.Log("Hay muro");
            return;
        }
        else if (xInput != player.facingDir && player.IsWallDetected())
        {
            Debug.Log("me giro   "+player.rb.velocity);
            player.FlipController(-player.facingDir);
            stateMachine.ChangeState(player.idleState);

        }/*
        else if (xInput == player.facingDir && !player.IsWallDetected())
        {
            Debug.Log("No hay muro");

            stateMachine.ChangeState(player.idleState);
        }
        */
            if (xInput != 0 && !player.IsWallDetected() && !player.isBusy)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }
}
