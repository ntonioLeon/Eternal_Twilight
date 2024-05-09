using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightBornTeleportState : EnemyState
{
    private Enemy_NightBorn enemy;


    public NightBornTeleportState(Enemigo enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_NightBorn enemy) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.FindPosition();

        stateTimer = 1;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
