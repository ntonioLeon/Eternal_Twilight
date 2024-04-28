using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorDecrease;

    private float cloneTimer;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f;
    private Transform closestEnemy;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0 ) 
        {
            sr.color = new Color(1, 1, 1, sr.color.a -(Time.deltaTime * colorDecrease));

            if (sr.color.a < 0f)
            {
                Destroy(gameObject);
            }
        }

        
    }
    public void SetUpClone(Transform newTransform, float cloneDuration, bool canAttack)
    {
        if (canAttack)
        {
            anim.SetInteger("AttackNumber", Random.Range(1, 3));
        }

        transform.position = newTransform.position;
        cloneTimer = cloneDuration;

        FaceClossestTarget();
    }

    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] coliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in coliders)
        {
            if (hit.GetComponent<Boss>() != null)
            {
                hit.GetComponent<Boss>().Damage();
            }
            else if (hit.GetComponent<Enemigo>() != null)
            {
                hit.GetComponent<Enemigo>().Damage();
            }
        }
    }

    private void FaceClossestTarget()
    {
        Collider2D[] Colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closetsDistance = Mathf.Infinity;

        foreach (var hit in Colliders) 
        {
            if (hit.GetComponent<Enemigo>() != null)
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);

                if (distance < closetsDistance)
                {
                    closetsDistance = distance;
                    closestEnemy = hit.transform;
                }
            }
        }

        if (closestEnemy != null)
        {
            if(transform.position.x > closestEnemy.position.x)
            {
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
