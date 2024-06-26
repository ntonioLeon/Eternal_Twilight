using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightBornDeadState : EnemyState
{
    private Enemy_NightBorn enemy;

    public NightBornDeadState(Enemigo enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_NightBorn enemy) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.StopMusic();
        AudioManager.instance.PlayGameMusic();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.Morir();
        enemy.SelfDestroy();
    }
}
