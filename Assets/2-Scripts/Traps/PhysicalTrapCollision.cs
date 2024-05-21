using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalTrapCollision : MonoBehaviour
{
    private CharacterStats stats => GetComponent<CharacterStats>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStats player = collision.GetComponent<PlayerStats>();
            collision.GetComponent<Entity>().SetupKnockbackDir(transform);
            stats.DoDamage(player);
            StartCoroutine(StopCollision());
        }
    }

    private IEnumerator StopCollision()
    {
        Physics2D.IgnoreLayerCollision(10, 13, true);

        yield return new WaitForSeconds(.5f);

        Physics2D.IgnoreLayerCollision(10, 13, false);
    }
}
