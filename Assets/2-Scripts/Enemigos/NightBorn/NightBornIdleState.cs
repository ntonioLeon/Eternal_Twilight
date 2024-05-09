using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightBornIdleState : EnemyState
{
    private Enemy_NightBorn enemy;
    public NightBornIdleState(Enemigo enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_NightBorn enemy) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
        enemy.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.K))
        {
            stateMachine.ChangeState(enemy.teleportState);
        }
    }
}
