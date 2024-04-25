using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
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

        StatePorEnemigo();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            if (enemy.IsWallDetected() || !enemy.IsGroundDetected()) //Si se le fuerza a ir por terreno intransitable se retira.
            {
                Debug.Log("0");
                enemy.Flip();
                stateMachine.ChangeState(enemy.moveState);
                return;
            }

            if (enemy.AceptableAttackDistance()) // Si tiene rango
            {
                if (enemy.CanAttack()) // y puede atacar
                {
                    Debug.Log("1");
                    enemy.battleMode = true;
                    stateMachine.ChangeState(enemy.attackState);
                }
                else // y no puede atacar
                {
                    Debug.Log("2");
                    if (enemy.backCollisionDetected())
                    {
                        stateMachine.ChangeState(enemy.guardState);
                    }
                    else
                    {
                        enemy.Retroceder(enemy.moveSpeed * -movingDirection, rb.velocity.y);
                    }
                }
            }
            else // Si no tiene rango
            {
                if (enemy.CanAttack()) // y puede atacar
                {
                    Debug.Log("3");
                    enemy.SetVelocity(enemy.moveSpeed * movingDirection, rb.velocity.y);
                    
                }
                else // y no puede atacar
                {
                    Debug.Log("4");
                    if (enemy.battleMode)
                    {
                        
                        if (Vector2.Distance(enemy.transform.position, enemy.player.transform.position) > enemy.attackDistance + 0.5f)
                        {
                            enemy.SetVelocity(enemy.moveSpeed * movingDirection, rb.velocity.y);
                        }
                        else
                        {
                            stateMachine.ChangeState(enemy.guardState);
                        }
                    }
                    else
                    {
                        enemy.SetVelocity(enemy.moveSpeed * movingDirection, rb.velocity.y);
                    }                    
                }
            }                
        }
        else  // No contacto visual
        {
            Debug.Log("5");
            if (stateTimer < 0 || Vector2.Distance(enemy.player.transform.position, enemy.transform.position) > enemy.aggroDistance)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
            else 
            {
                if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
                {
                    enemy.Flip();    
                    stateMachine.ChangeState(enemy.moveState);
                }
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
