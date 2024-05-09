using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HiddenRooms : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] Tilemap hidden;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Color color = hidden.color;
            color.a = 0.5f;
            hidden.color = color;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Color color = hidden.color;
            color.a = 1f;
            hidden.color = color;
        }
    }
}
