using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [Header("Componentes")]
    public Transform target, room, middleground, background;

    [Header("Minimos y maximos")]
    [Range(-15,15)]
    public float minX;
    [Range(-15, 15)]
    public float maxX;
    [Range(-15, 15)]
    public float minY;
    [Range(-15, 15)]
    public float maxY;
    public float velocidadCamara;
    public bool stopFollow;

    private Vector2 lastPos;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopFollow)
        {
            var minPosY = room.GetComponent<BoxCollider2D>().bounds.min.y + minY;
            var minPosX = room.GetComponent<BoxCollider2D>().bounds.min.x + minX;
            var maxPosY = room.GetComponent<BoxCollider2D>().bounds.max.y + maxY;
            var maxPosX = room.GetComponent<BoxCollider2D>().bounds.max.x + maxX;

            Vector3 posicionAcotada = new Vector3(
                Mathf.Clamp(target.position.x, minPosX, maxPosX),
                Mathf.Clamp(target.position.y, minPosY, maxPosY),
                Mathf.Clamp(target.position.z, -10f, -10f)
                );
            Vector3 cambioSuave = Vector3.Lerp(transform.position, posicionAcotada, velocidadCamara * Time.deltaTime);
            transform.position = cambioSuave;

            Vector2 amountV = new Vector2(transform.position.x - lastPos.x, transform.position.y - lastPos.y);
            background.position = background.position + new Vector3(amountV.x, amountV.y, 0f);
            middleground.position = background.position + new Vector3(amountV.x, amountV.y, 0f) *.5f;
            lastPos = transform.position;
        }
    }
}
