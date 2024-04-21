using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BossState 
{
    protected BossStateMachine stateMachine;
    protected Boss boss;

    protected bool triggerCalled;
    private string animBoolName;
    protected float stateTimmer;

    public BossState(Boss boss, BossStateMachine stateMachine, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.boss = boss;
        this.animBoolName = animBoolName;
    }
    public virtual void Update()
    {
        stateTimmer -= Time.deltaTime;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        boss.anim.SetBool(animBoolName, true);
    }

    public virtual void Exit()
    {
        boss.anim.SetBool(animBoolName, false);
    }
}
