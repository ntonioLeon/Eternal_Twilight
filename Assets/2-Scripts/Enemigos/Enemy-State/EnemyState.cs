using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemigo enemyBase;
    protected Rigidbody2D rb;

    protected bool triggerCalled;
    private string animBoolName;
    protected float stateTimer;

    public EnemyState(Enemigo enemyBase, EnemyStateMachine stateMachine, string animBoolName)
    {
        this.enemyBase = enemyBase;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Update()
    {
        if (PauseMenu.instance.isPaused)
        {
            enemyBase.moveSpeed=0;
            enemyBase.SetZeroVelocity();
            return;
        }
        else
        {
            enemyBase.moveSpeed = enemyBase.defaultMoveSpeed;
            stateTimer -= Time.deltaTime;
        }
        
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        rb = enemyBase.rb;
        enemyBase.anim.SetBool(animBoolName, true);
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);
        enemyBase.AssignLastAnimName(animBoolName);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
