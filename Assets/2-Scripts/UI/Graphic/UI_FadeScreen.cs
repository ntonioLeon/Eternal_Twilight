using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadeScreen : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private GameObject textoFin;
    [SerializeField] private GameObject textoPuntuacion;

    void Start()
    {
        anim = GameObject.Find("DarkScreen").GetComponent<Animator>();
    }

    public void FadeOut()
    {
        anim.SetTrigger("fadeOut");
    }

    public void FadeIn()
    {
        anim.SetTrigger("fadeIn");
    }

    public void ActivarTextFin()
    {
        FadeOut(); 

        StartCoroutine(Creditos());
    }

    private IEnumerator Creditos()
    {
        yield return new WaitForSeconds(1.5f);

        if (textoFin != null)
            textoFin.SetActive(true);

        Debug.Log(textoPuntuacion != null);
        if (textoPuntuacion != null)
        {
            textoPuntuacion.SetActive(true);

            yield return new WaitForSeconds(.5f);
            if (!PlayerPrefs.GetString("Logged").Equals("N")) 
                PlayFabManager.instance.GetArroundU2();
        }
    }
}
