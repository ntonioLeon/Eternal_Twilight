using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class Player : Entity
{
    [Header("Attack details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = .2f;

    public bool isBusy { get; private set; }

    [Header("Move info")]
    public float moveSpeed = 12f;
    public float jumpForce;
    public bool canDoubleJump;
    public float swordreturnImpact;
    [HideInInspector]public float defaultMoveSpeed;
    private float defaultJumpSpeed;

    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    private float defaultDashSpeed;

    #region Angularidad
    public float anguloMax;
    [HideInInspector] public Vector2 capsuleSize;
    [HideInInspector] public Vector2 posPies;
    [HideInInspector] public bool enPendiente;
    [HideInInspector] public float anguloLateral;
    [HideInInspector] public float anguloPendiente;
    [HideInInspector] public float anguloAnterior;
    [HideInInspector] public Vector2 anguloPer;
    [HideInInspector] public bool puedeCaminar;
    public PhysicsMaterial2D sinFriccion;
    public PhysicsMaterial2D maxFriccion;
    #endregion
    
    public float dashDir { get; private set; }

    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }

    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallSlideSate wallSlideSate { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }

    public PlayerAimSwordState aimSwordState { get; private set; }

    public PlayerCatchState catchSword { get; private set; }
    public PlayerDeadState deadState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        wallSlideSate = new PlayerWallSlideSate(this, stateMachine, "WallSlide");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSword = new PlayerCatchState(this, stateMachine, "CatchSword");
        defaultMoveSpeed = moveSpeed;
        deadState = new PlayerDeadState(this, stateMachine, "Die");
    }

    protected override void Start()
    { 
        base.Start();
        
        skill = SkillManager.instance;
        stateMachine.Initialize(idleState);

        
        defaultJumpSpeed = jumpForce;
        defaultDashSpeed = dashSpeed;
        capsuleSize = capsule.size;
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();

        CheckForDashInput();        
    }

    public override void SlowEntityBy(float slowPercentage, float slowDuration)
    {
        moveSpeed = moveSpeed * (1 - slowPercentage);
        jumpForce = jumpForce * (1 - slowPercentage);
        dashSpeed = dashSpeed * (1 - slowPercentage);
        anim.speed = anim.speed * (1- slowPercentage);

        Invoke("ReturnDefaultSpeed", slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpSpeed;
        dashSpeed = defaultDashSpeed;
    }

    public void AssignNewSword(GameObject newSword) 
    {
        sword = newSword;   
    }

    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSword);
        Destroy(sword); 
    }

    public IEnumerator BusyFor(float seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(seconds);

        isBusy = false;
    }

    public void AnimacionTriiger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }

    private void CheckForDashInput()
    {
        if (IsWallDetected())
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {

            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
            {
                dashDir = facingDir;
            }

            stateMachine.ChangeState(dashState);
        }
    }

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }    
}
