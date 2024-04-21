using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BossState 
{
    protected BossStateMachine stateMachine;
    protected Boss bossBase;

    protected bool triggerCalled;
    private string animBoolName;
    protected float stateTimmer;

    public BossState(Boss bossBase, BossStateMachine stateMachine, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.bossBase = bossBase;
        this.animBoolName = animBoolName;
    }
    public virtual void Update()
    {
        stateTimmer -= Time.deltaTime;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        bossBase.anim.SetBool(animBoolName, true);
    }

    public virtual void Exit()
    {
        bossBase.anim.SetBool(animBoolName, false);
    }
}
