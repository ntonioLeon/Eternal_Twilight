using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandidoMelee : Enemigo
{
    #region
    public BandidoMeleeIdleState idleState { get; private set; }
    public BandidoMeleeMoveState moveState { get; private set; }
    public BandidoMeleeBattleState battleState { get; private set; }
    public BandidoMeleeAttackState attackState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new BandidoMeleeIdleState(this, stateMachine, "Idle", this);
        moveState = new BandidoMeleeMoveState(this, stateMachine, "Move", this);
        battleState = new BandidoMeleeBattleState(this, stateMachine, "Move", this);
        attackState = new BandidoMeleeAttackState(this, stateMachine, "Attack", this);
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
