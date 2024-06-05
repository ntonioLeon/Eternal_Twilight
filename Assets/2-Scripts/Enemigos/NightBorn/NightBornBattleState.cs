using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightBornBattleState : EnemyState
{
    private Enemy_NightBorn enemy;
    private Transform player;
    private int moveDir;

    public NightBornBattleState(Enemigo enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_NightBorn enemy) : base(enemyBase, stateMachine, animBoolName)
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
                if (enemy.CanAttack())
                {
                    stateMachine.ChangeState(enemy.attackState);
                }
                else
                {
                    stateMachine.ChangeState(enemy.idleState);
                }
            }
        }
        else
        {
            enemy.Flip();
        }

        StatePorEnemigo();

        if (enemy.IsGroundDetected() && enemy.IsPlayerDetected().distance < enemy.attackDistance - .5f)
        {
            return;
        }

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
        {
            stateMachine.ChangeState(enemy.idleState);

        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void StatePorEnemigo()
    {
       if (player.position.x > enemy.transform.position.x)
       {
            moveDir = 1;
        }
       else if (player.position.x < enemy.transform.position.x) 
       {
            moveDir = -1;
       }
    }
}
