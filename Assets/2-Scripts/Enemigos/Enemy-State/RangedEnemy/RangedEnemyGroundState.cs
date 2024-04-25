using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyGroundState : EnemyState
{
    protected RangedEnemy enemy;

    protected Transform player;

    public RangedEnemyGroundState(Enemigo enemyBase, EnemyStateMachine stateMachine, string animBoolName, RangedEnemy enemy) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.battleMode = false;
        player = GameObject.Find("Player").transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
