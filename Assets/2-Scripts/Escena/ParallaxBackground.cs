using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject cam;

    [SerializeField] private float parallaxEffect;

    private float xPosition;
    private float lenght;
    private float yPosition;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;

        xPosition = transform.position.x;
        yPosition = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);
        float distanceToMoveX = cam.transform.position.x * parallaxEffect;
        float distanceToMoveY = cam.transform.position.y * parallaxEffect * .5f;

        transform.position = new Vector3(xPosition + distanceToMoveX, yPosition + distanceToMoveY);

        if(distanceMoved > xPosition + lenght)
        {
            xPosition = xPosition + lenght;
        }
        else if(distanceMoved < xPosition - lenght)
        {
            xPosition = xPosition - lenght;
        }
    }
}
