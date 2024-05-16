using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Platformas : MonoBehaviour
{
    public static Platformas instance;
    private GameObject player;
    private CapsuleCollider2D ccPlayer;
    private Collider2D ccPlatform;
    private Bounds ccPlatformBounds;
    private float topPlatform, footPlayer;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ccPlayer = player.GetComponent<CapsuleCollider2D>();
        ccPlatform = GetComponent<Collider2D>();
        ccPlatformBounds = ccPlatform.bounds;
        

        topPlatform = ccPlatformBounds.center.y + ccPlatformBounds.extents.y;
    }

    void Update()
    {
        footPlayer = player.transform.position.y - (ccPlayer.size.y / 2);   
        if(footPlayer >= topPlatform)
        {
            ccPlatform.isTrigger = false;
            gameObject.tag = "SpecialGround";
            gameObject.layer = LayerMask.NameToLayer("Ground");
        }
        if(!ccPlatform.isTrigger && (footPlayer < (topPlatform -.1f)))
        {
            ccPlatform.isTrigger = true;
            gameObject.tag = "Untagged";
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

 
}
