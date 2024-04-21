using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }

    public BossStateMachine stateMachine { get; private set; }

    private void Awake()
    {
        stateMachine = new BossStateMachine();
    }

    private void Update()
    {
       stateMachine.currentState.Update();
    }
}
