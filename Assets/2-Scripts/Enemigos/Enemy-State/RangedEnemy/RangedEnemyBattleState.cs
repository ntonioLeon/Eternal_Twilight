using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class RangedEnemyBattleState : EnemyState
{
    private RangedEnemy enemy;
    private int movingDirection;

    public RangedEnemyBattleState(Enemigo enemyBase, EnemyStateMachine stateMachine, string animBoolName, RangedEnemy enemy) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }
    public override void Update()
    {
        base.Update();

        /*
        StatePorEnemigo();

        float distance = Vector2.Distance(enemy.transform.position, enemy.player.position);        

        if (enemy.attackDistance < distance && !enemy.CanAttack())
        {
            Debug.Log("0");
            stateTimer = enemy.battleTime;
            if ((enemy.attackDistance + 0.5f) < distance)
            {
                enemy.SetVelocity(enemy.moveSpeed * movingDirection, rb.velocity.y);
            }
            
        }
        else if (enemy.attackDistance < distance && enemy.CanAttack())
        {
            Debug.Log("1");
            enemy.SetVelocity(enemy.moveSpeed * movingDirection, rb.velocity.y);
        }
        else if (enemy.attackDistance > distance && enemy.CanAttack())
        {
            Debug.Log("2");
            stateTimer = enemy.battleTime;
            stateMachine.ChangeState(enemy.attackState);
        }
        else if (enemy.attackDistance > distance && !enemy.CanAttack())
        {
            Debug.Log("3");
            stateTimer = enemy.battleTime;
            if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
            {
                
                stateMachine.ChangeState(enemy.guardState);
            }
            else 
            {                
                enemy.SetVelocity((enemy.moveSpeed * (movingDirection) * -1), rb.velocity.y);
            }
        }

        if (stateTimer < 0 || distance > enemy.aggroDistance)
        {
            Debug.Log("5");
            stateMachine.ChangeState(enemy.idleState);
        }*/

        StatePorEnemigo();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            if (enemy.AceptableAttackDistance() && enemy.CanAttack())  //Si puede atacar y tiene rango ataca
            {
                stateMachine.ChangeState(enemy.attackState);
            }
            else if (enemy.AceptableAttackDistance() && !enemy.CanAttack()) //Si tiene rango pero no puede atacar retrocede
            {
                if (enemy.backCollisionDetected())
                {
                    stateMachine.ChangeState(enemy.guardState);                    
                }
                else
                {
                    enemy.Retroceder(enemy.moveSpeed * (movingDirection * -1), rb.velocity.y);
                }                
            }
            else if (!enemy.AceptableAttackDistance() && enemy.CanAttack()) //Si no tiene rango y si puede atacar avanza
            {
                if (enemy.IsPlayerDetected().distance > 1f)
                {
                    enemy.SetVelocity(enemy.moveSpeed * movingDirection, rb.velocity.y);
                }
                else
                {
                    enemy.SetZeroVelocity();
                    stateMachine.ChangeState(enemy.guardState);
                }
            }
            else if (!enemy.AceptableAttackDistance() && !enemy.CanAttack()) //Si no tiene rango y no puede atacar se para y se pone en guardia
            {
                enemy.SetZeroVelocity();
                stateMachine.ChangeState(enemy.guardState);                
            }
            else if (enemy.IsWallDetected() || !enemy.IsGroundDetected()) //Si se le fuerza a ir por terreno intransitable se retira.
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
            if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
            {
                enemy.SetVelocity(enemy.moveSpeed * movingDirection, rb.velocity.y);
            }                
        }
    }

    public override void Enter()
    {
        base.Enter();
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
