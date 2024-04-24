using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyGuardState : EnemyState
{
    private RangedEnemy enemy;
    private int movingDirection;

    public RangedEnemyGuardState(Enemigo enemyBase, EnemyStateMachine stateMachine, string animBoolName, RangedEnemy enemy) : base(enemyBase, stateMachine, animBoolName)
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

        if (enemy.AceptableAttackDistance() && enemy.CanAttack())
        {
            stateMachine.ChangeState(enemy.attackState);
        }
        if (!enemy.AceptableAttackDistance() && enemy.CanAttack())
        {
            stateMachine.ChangeState(enemy.battleState);
        }
        if (!enemy.AceptableAttackDistance() && !enemy.CanAttack())
        {
            stateMachine.ChangeState(enemy.battleState);
        }
        //StatePorEnemigo();
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
