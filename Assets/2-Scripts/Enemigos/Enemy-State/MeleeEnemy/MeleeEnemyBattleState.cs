using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyBattleState : EnemyState
{
    private MeleeEnemy enemy;
    private int movingDirection;
    private Transform player;

    public MeleeEnemyBattleState(Enemigo enemyBase, EnemyStateMachine stateMachine, string animBoolName, MeleeEnemy enemy) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy; 
    }
    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            if (enemy.AceptableAttackDistance() && enemy.CanAttack())
            {
                stateMachine.ChangeState(enemy.attackState);
            }
            else if (enemy.AceptableAttackDistance() && !enemy.CanAttack())
            {
                stateMachine.ChangeState(enemy.guardState);
            }
            else if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
            {
                enemy.Flip();
                stateMachine.ChangeState(enemy.moveState);
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(enemy.player.transform.position, enemy.transform.position) > enemy.aggroDistance)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }

        StatePorEnemigo();

        enemy.SetVelocity(enemy.moveSpeed * movingDirection, rb.velocity.y);
    }    

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
        {
            stateMachine.ChangeState(enemy.moveState);

        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void StatePorEnemigo()
    {
        if (enemy.player.position.x > enemy.transform.position.x)
        {
            movingDirection = 1;
        }
        else if (enemy.player.position.x < enemy.transform.position.x)
        {
            movingDirection = -1;
        }
    }
}
