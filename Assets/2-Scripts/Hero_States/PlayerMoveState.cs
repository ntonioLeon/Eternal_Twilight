using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        // Player en suelo
        if (!player.enPendiente)
        {
            player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);
        }
        // Player en pendiente
        else if (player.puedeCaminar && player.enPendiente)
        {
            player.SetVelocity(player.moveSpeed * player.anguloPer.x * -xInput, player.moveSpeed * player.anguloPer.y * -xInput);
        }
        //

        if (xInput == 0 || player.IsWallDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
