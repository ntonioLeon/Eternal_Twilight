using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Boss : MonoBehaviour
{
    public static UI_Boss instance;

    public GameObject bossPanel;
    public GameObject walls;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }    
    }

    void Start()
    {
        bossPanel.SetActive(false);
        walls.SetActive(false);
    }

    public void BossActivation()
    {
        bossPanel.SetActive(true);
        walls.SetActive(true);
    }
    public void BossDeactivation()
    {
        bossPanel.SetActive(false);
        walls.SetActive(false);
    }

}
