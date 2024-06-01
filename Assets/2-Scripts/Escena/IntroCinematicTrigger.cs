using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class IntroCinematicTrigger : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !PlayerManager.instance.CinematicaVista)
        {
            playableDirector.Play();
        }
    }
}
