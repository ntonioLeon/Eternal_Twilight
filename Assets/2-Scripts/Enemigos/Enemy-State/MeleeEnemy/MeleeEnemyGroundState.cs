using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyGroundState : EnemyState
{
    protected MeleeEnemy enemy;

    protected Transform player;

    public MeleeEnemyGroundState(Enemigo enemyBase, EnemyStateMachine stateMachine, string animBoolName, MeleeEnemy enemy) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = GameObject.Find("Player").transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < enemy.detectDistance && (enemy.IsWallDetected() || !enemy.IsGroundDetected()))
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
