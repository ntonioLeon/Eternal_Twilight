using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemigo
{
    #region
    public MeleeEnemyIdleState idleState { get; private set; }
    public MeleeEnemyMoveState moveState { get; private set; }
    public MeleeEnemyBattleState battleState { get; private set; }
    public MeleeEnemyAttackState attackState { get; private set; }
    public MeleeEnemyGuardState guardState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();                

        idleState = new MeleeEnemyIdleState(this, stateMachine, "Idle", this);
        moveState = new MeleeEnemyMoveState(this, stateMachine, "Move", this);
        battleState = new MeleeEnemyBattleState(this, stateMachine, "Move", this);
        attackState = new MeleeEnemyAttackState(this, stateMachine, "Attack", this);
        if (hasGuard)
        {
            guardState = new MeleeEnemyGuardState(this, stateMachine, "Guard", this);
        } 
        else 
        {
            guardState = new MeleeEnemyGuardState(this, stateMachine, "Idle", this);
        }
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initioalize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }
}
