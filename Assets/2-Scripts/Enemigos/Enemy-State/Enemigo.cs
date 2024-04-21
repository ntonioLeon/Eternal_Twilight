using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : Entity
{

    [SerializeField] protected LayerMask whatIsPlayer;
    [SerializeField] private float detectionArea;

    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;

    [Header("Attack info")]
    public float attackDistance;
    public float attackCooldown;
    [HideInInspector] public float lastTimeAttacked;
    public float aggroDistance;
    public float detectDistance;

    [Header("Status info")]
    [SerializeField] protected bool hasGuard;

    public EnemyStateMachine stateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    public virtual void AnimationFinishTrigger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }

    public virtual RaycastHit2D IsPlayerDetected()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, detectionArea, whatIsPlayer);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance *facingDir, transform.position.y));
    }
}
