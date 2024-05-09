using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake instance;

    private CinemachineVirtualCamera cinemachine;
    private CinemachineBasicMultiChannelPerlin perlin;
    private float shakeTimer;
    private float shakeTotalTimer;
    private float startingIntensity;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        cinemachine = GetComponent<CinemachineVirtualCamera>();
        perlin = cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            perlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1-shakeTimer / shakeTotalTimer);
        }
        
    }

    public void ShakeCamera(float intensity, float duration)
    {
        perlin.m_AmplitudeGain = intensity;
        startingIntensity = intensity;
        shakeTimer = duration;
        shakeTotalTimer = duration;
    }
}
