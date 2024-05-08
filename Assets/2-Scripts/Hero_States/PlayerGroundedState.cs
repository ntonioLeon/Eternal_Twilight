using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        player.canDoubleJump = true;
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword()) // falta algo como esto: && player.rb.velocity==Vector2.zero
        {
            stateMachine.ChangeState(player.aimSwordState);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            stateMachine.ChangeState(player.counterAttackState);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            stateMachine.ChangeState(player.primaryAttackState);
        }

        if (!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
        }
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected()) //&& player.rb.velocity.magnitude >=0 Quiza haga falta
        {
            stateMachine.ChangeState(player.jumpState);
        }
    }

    private bool HasNoSword()
    {
        if (!player.sword)
        {
            return true;
        }
        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }
}
