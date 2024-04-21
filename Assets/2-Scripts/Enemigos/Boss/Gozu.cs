using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gozu : Boss
{
    #region
    public GozuIdleState idleState { get; private set; }
    public GozuMoveState moveState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new GozuIdleState(this, stateMachine, "Idle", this);
        moveState = new GozuMoveState(this, stateMachine, "Idle", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }
}
