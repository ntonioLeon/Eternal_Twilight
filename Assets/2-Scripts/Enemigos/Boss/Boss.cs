using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Entity
{
    public BossStateMachine stateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new BossStateMachine();
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }
}
