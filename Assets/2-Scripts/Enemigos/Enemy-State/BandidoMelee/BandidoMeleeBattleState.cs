using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandidoMeleeBattleState : EnemyState
{
    private BandidoMelee enemy;
    private Transform player;
    private int movingDirection;

    public BandidoMeleeBattleState(Enemigo enemyBase, EnemyStateMachine stateMachine, string animBoolName, BandidoMelee enemy) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy; 
    }
    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack()) 
                {
                    stateMachine.ChangeState(enemy.attackState);
                }             
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > enemy.aggroDistance)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }

        if (player.position.x > enemy.transform.position.x)
        {
            movingDirection = 1;
        } 
        else if (player.position.x < enemy.transform.position.x)
        {
            movingDirection = -1;
        }

        enemy.SetVelocity(enemy.moveSpeed * movingDirection, rb.velocity.y);
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

    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }
}
