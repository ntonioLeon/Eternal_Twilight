using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightBornDeadState : EnemyState
{
    private Enemy_NightBorn enemy;

    public NightBornDeadState(Enemigo enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_NightBorn enemy) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
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
        if (triggerCalled)
        {
            enemy.SelfDestroy();

        }
    }
}
