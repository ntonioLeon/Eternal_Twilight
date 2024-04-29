using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeEnemy : Enemigo
{
    #region
    public MeleeEnemyIdleState idleState { get; private set; }
    public MeleeEnemyMoveState moveState { get; private set; }
    public MeleeEnemyBattleState battleState { get; private set; }
    public MeleeEnemyAttackState attackState { get; private set; }
    public MeleeEnemyGuardState guardState { get; private set; }
    public MeleeEnemyStunnedState stunnedState { get; private set; }
    public MeleeEnemyDeadState deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();                

        idleState = new MeleeEnemyIdleState(this, stateMachine, "Idle", this);
        moveState = new MeleeEnemyMoveState(this, stateMachine, "Move", this);
        battleState = new MeleeEnemyBattleState(this, stateMachine, "Move", this);
        attackState = new MeleeEnemyAttackState(this, stateMachine, "Attack", this);
        guardState = hasGuard ? new MeleeEnemyGuardState(this, stateMachine, "Guard", this) : new MeleeEnemyGuardState(this, stateMachine, "Idle", this);
        stunnedState = new MeleeEnemyStunnedState(this, stateMachine, "Stunned", this);
        deadState = new MeleeEnemyDeadState(this, stateMachine, "Idle", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initioalize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.RightAlt))
        {
            stateMachine.ChangeState(stunnedState);
        }
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }

        return false;
    }

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }
}
