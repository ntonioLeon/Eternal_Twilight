using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectilDestroyer : MonoBehaviour
{
    public float segundos;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, segundos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
