using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Entity
{
     

    [Header("Move info")]
    public float moveSpeed;
    public float iddleTime;


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
