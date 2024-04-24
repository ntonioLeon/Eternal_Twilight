using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemigo
{
    #region
    public RangedEnemyIdleState idleState { get; private set; }
    public RangedEnemyMoveState moveState { get; private set; }
    public RangedEnemyBattleState battleState { get; private set; }
    public RangedEnemyAttackState attackState { get; private set; }
    public RangedEnemyGuardState guardState { get; private set; }
    public RangedEnemyStunnedState stunnedState { get; private set; }
    #endregion

    public Transform proyectilPos;
    [SerializeField] private Transform backCheck;
    [SerializeField] private float backCheckDistance;

    protected override void Awake()
    {
        base.Awake();

        idleState = new RangedEnemyIdleState(this, stateMachine, "Idle", this);
        moveState = new RangedEnemyMoveState(this, stateMachine, "Move", this);
        battleState = new RangedEnemyBattleState(this, stateMachine, "Move", this);
        attackState = new RangedEnemyAttackState(this, stateMachine, "Attack", this);
        guardState = hasGuard ? new RangedEnemyGuardState(this, stateMachine, "Guard", this) : new RangedEnemyGuardState(this, stateMachine, "Idle", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initioalize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.RightAlt))
        {
            stateMachine.ChangeState(stunnedState);
        }
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }

        return false;
    }

    public virtual bool backCollisionDetected()
    {
        return Physics2D.Raycast(backCheck.position, Vector2.right * (facingDir * -1), backCheckDistance, whatIsGround) || !Physics2D.Raycast(backCheck.position, Vector2.down, backCheckDistance*3, whatIsGround);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(backCheck.position, new Vector3(backCheck.position.x + backCheckDistance * -facingDir, backCheck.position.y));
        Gizmos.DrawLine(backCheck.position, new Vector3(backCheck.position.x, backCheck.position.y + (backCheckDistance * 3) * -1));
    }
}
