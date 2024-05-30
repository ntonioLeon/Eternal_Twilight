using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    [SerializeField] private UI_FadeScreen fadeScreen;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine(Morirse2(collision));
            
        }
    }

    private IEnumerator Morirse2(Collider2D collision)
    {
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(1f);

        collision.GetComponent<Player>().Die();
    }
}
