using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandidoMeleeGroundState : EnemyState
{
    protected BandidoMelee enemy;

    protected Transform player;

    public BandidoMeleeGroundState(Enemigo enemyBase, EnemyStateMachine stateMachine, string animBoolName, BandidoMelee enemy) : base(enemyBase, stateMachine, animBoolName)
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
        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < enemy.detectDistance)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
