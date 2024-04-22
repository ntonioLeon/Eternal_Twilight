using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyGuardState : EnemyState
{
    private MeleeEnemy enemy;
    private int movingDirection;

    public MeleeEnemyGuardState(Enemigo enemyBase, EnemyStateMachine stateMachine, string animBoolName, MeleeEnemy enemy) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.CanAttack())
        {
            stateMachine.ChangeState(enemy.attackState);
        }
        
        if (!enemy.AceptableAttackDistance())
        {
            stateMachine.ChangeState(enemy.battleState);
        }

        StatePorEnemigo();
    }

    private void StatePorEnemigo()
    {
        if (enemy.player.position.x > enemy.transform.position.x)
        {
            movingDirection = 1;
            enemy.SetVelocity(0.01f * movingDirection, rb.velocity.y);
            enemy.SetZeroVelocity();
        }
        else if (enemy.player.position.x < enemy.transform.position.x)
        {
            movingDirection = -1;
            enemy.SetVelocity(0.01f * movingDirection, rb.velocity.y);
            enemy.SetZeroVelocity();
        }
    }
}
