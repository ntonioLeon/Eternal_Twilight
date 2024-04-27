using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchState : PlayerState
{
    private Transform sword;
    public PlayerCatchState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        sword = player.sword.transform;

        //player.fx.PlayDustFX();
        //player.fx.ScreenShake(player.fx.shakeSwordImpact);

        if (player.transform.position.x > sword.position.x && player.facingDir == 1)
        {
            player.Flip();
        }    
        else if (player.transform.position.x < sword.position.x && player.facingDir == -1)
        {
            player.Flip();
        } 

        rb.velocity = new Vector2(player.swordreturnImpact * -player.facingDir, rb.velocity.y); // No esta Bien

    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .1f);
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
