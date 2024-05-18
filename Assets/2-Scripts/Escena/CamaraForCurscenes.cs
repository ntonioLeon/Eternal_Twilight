using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamaraForCurscenes : MonoBehaviour
{
    private CinemachineVirtualCamera camara => GetComponent<CinemachineVirtualCamera>();

    public void AjustesParaCutScene()
    {
        camara.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.3f;
        camara.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.55f;
    }

    public void AjustesParaJugar()
    {
        camara.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.5f;
        camara.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.75f;
    }
}
