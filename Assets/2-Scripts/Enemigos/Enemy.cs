using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public string enemyName;
    public float damageToGive;
    public float healthPoints;
    public float speed;
    public float knockbackForceX;
    public float knockbackForceY;
    public float expToGive;
    public bool shouldRespawn;
    private Animator anim;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
