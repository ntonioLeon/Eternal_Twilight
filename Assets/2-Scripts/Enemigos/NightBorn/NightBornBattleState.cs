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

            
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(enemy.player.transform.position, enemy.transform.position) > enemy.aggroDistance)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }

        StatePorEnemigo();

        
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
       
    }
}
