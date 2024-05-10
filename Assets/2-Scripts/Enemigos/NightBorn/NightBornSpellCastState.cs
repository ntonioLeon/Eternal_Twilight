using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightBornSpellCastState : EnemyState
{
    private Enemy_NightBorn enemy;

    private int amountOfSpells;
    private float spellTimer;

    public NightBornSpellCastState(Enemigo enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_NightBorn enemy) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        amountOfSpells = enemy.amountOfSpells;
        spellTimer = .5f;
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeCast = Time.time;
    }

    public override void Update()
    {
        base.Update();

        spellTimer -= Time.deltaTime;

        if (CanCast())
        { 
            enemy.CastSpell();
        }

        if (amountOfSpells <= 0)
        {
            stateMachine.ChangeState(enemy.teleportState);
        }
    }

    private bool CanCast()
    {
        if (amountOfSpells > 0 && spellTimer < 0)
        {
            amountOfSpells -= 1;
            spellTimer = enemy.spellCooldown;
            return true;
        }

        return false;
    }
}
