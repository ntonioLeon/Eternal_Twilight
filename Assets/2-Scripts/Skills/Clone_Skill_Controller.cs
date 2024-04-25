using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private float colorDecrease;
    private float cloneTimer;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
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
    public void SetUpClone(Transform newTransform, float cloneDuration)
    {
        transform.position = newTransform.position;
        cloneTimer = cloneDuration;
    }
}
