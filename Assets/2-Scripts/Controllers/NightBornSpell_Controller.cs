using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightBornSpell_Controller : MonoBehaviour
{
    [SerializeField] private Transform check;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private LayerMask whatIsPlayer;

    private CharacterStats myStats;

    public void SetupSpell(CharacterStats characterStats)
    {
        myStats = characterStats;
    }

    private void AnimationTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(check.position, boxSize, whatIsPlayer);

        foreach (var collider in colliders)
        {
            if (collider.GetComponent<Player>() != null)
            {
                collider.GetComponent<Entity>().SetupKnockbackDir(transform);
                myStats.DoDamage(collider.GetComponent<CharacterStats>());
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(check.position, boxSize);
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
