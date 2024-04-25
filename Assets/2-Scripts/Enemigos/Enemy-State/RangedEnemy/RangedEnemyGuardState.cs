using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyGuardState : EnemyState
{
    private RangedEnemy enemy;
    private int movingDirection;
    private float distancia;

    public RangedEnemyGuardState(Enemigo enemyBase, EnemyStateMachine stateMachine, string animBoolName, RangedEnemy enemy) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetZeroVelocity();

        distancia = Vector2.Distance(enemy.transform.position, enemy.player.transform.position);
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
            stateMachine.ChangeState(enemy.battleState);
        }

        if (Vector2.Distance(enemy.transform.position, enemy.player.transform.position) != distancia)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
