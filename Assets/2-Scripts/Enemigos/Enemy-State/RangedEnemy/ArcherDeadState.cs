﻿using System.Collections;
using UnityEngine;


public class ArcherDeadState : EnemyState
{
    private Enemy_Archer enemy;

    public ArcherDeadState(Enemigo enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer enemy) : base(enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }
    public override void Enter()
    {
        base.Enter();

        //enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        //enemy.anim.speed = 0;
        //enemy.capsule.enabled = false;

        stateTimer = .15f;
    }

    public override void Update()
    {
        base.Update();

        enemy.Morir();
        enemy.SelfDestroy();
    }
}
