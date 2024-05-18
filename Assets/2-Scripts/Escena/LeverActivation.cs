using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LeverActivation : MonoBehaviour
{
    private SpriteRenderer sr => GetComponent<SpriteRenderer>();
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private Sprite palancaBajada;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            sr.sprite = palancaBajada;
            playableDirector.Play();
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
