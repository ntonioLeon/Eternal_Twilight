using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateMachine
{
    public BossState currentState { get; private set; }

    public void Initialize(BossState startState)
    {
        currentState = startState;
        currentState.Enter();
    }

    public void ChangeState(BossState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
