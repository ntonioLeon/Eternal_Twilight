using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;

public class CutScenesUI : MonoBehaviour
{

    public GameObject vida;
    public GameObject almas;
    public GameObject weapon;
    //public Canvas canvas;

    public void TurnOffCanvas()
    {
        //canvas.enabled = false;
        vida.SetActive(false);
        almas.SetActive(false);
        weapon.SetActive(false);
    }

    public void TurnOnCanvas()
    {
        //canvas.enabled = true;
        vida.SetActive(true);
        almas.SetActive(true);
        weapon.SetActive(true);
    }
}
