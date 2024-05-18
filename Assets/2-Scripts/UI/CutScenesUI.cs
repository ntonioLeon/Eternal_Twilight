using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;

public class CutScenesUI : MonoBehaviour
{
    public Canvas canvas;

    public void TurnOffCanvas()
    {
        canvas.enabled = false;
    }

    public void TurnOnCanvas()
    {
        canvas.enabled = true;
    }
}
