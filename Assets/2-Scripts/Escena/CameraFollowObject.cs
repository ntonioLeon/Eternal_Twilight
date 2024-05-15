using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;

    [Header("Flip Rotation Stats")]
    [SerializeField] private float flipYRotationTime = 0.5f;

    private Coroutine turnCoroutine;
    private Player player;
    private bool isFacingRight;

    private void Awake()
    {
        player = playerTransform.gameObject.GetComponent<Player>();
        
        if (player.facingDir == -1)
        {
            isFacingRight = true;
        }
    }

    private void Start()
    {
        
    }


    private void Update()
    {
        transform.position = playerTransform.position;
    }

    public void CallTurn()
    {
        //turnCoroutine = StartCoroutine(FlipYLerp());

        LeanTween.rotateY(gameObject, DetermineEndRotation(), flipYRotationTime).setEaseInOutSine();
    }

    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation = 0f;

        float elapsedTime = 0f;
        while (elapsedTime < flipYRotationTime)
        {
            elapsedTime += Time.deltaTime;

            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / flipYRotationTime));
            transform.rotation = Quaternion.Euler(0,yRotation,0);

            yield return null;
        }
    }

    private float DetermineEndRotation()
    {
        isFacingRight = !isFacingRight;

        if (isFacingRight)
        {
            return 180;
        }
        else
        {
            return 0;
        }
    }
}
