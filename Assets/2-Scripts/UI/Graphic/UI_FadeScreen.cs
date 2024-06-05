using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadeScreen : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private GameObject textoFin;
    [SerializeField] private GameObject textoPuntuacion;
    private String mssg;


    void Start()
    {
        anim = GameObject.Find("DarkScreen").GetComponent<Animator>();
    }
    private void OnEnable()
    {
        Sing4TheMemes();
        Debug.Log(mssg);
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

    private void Sing4TheMemes()
    {
        int result = UnityEngine.Random.Range(0, 5);

        switch (result)
        {
            case 0:
                mssg = "Standing here\nI realize\nYou are just like me\nTrying to make history";
                break;
            case 1:
                mssg = "But who’s to judge\nThe right from wrong\nWhen our guard is down\nI think we’ll both agree";
                break;
            case 2:
                mssg = "That violence breeds violence\nBut in the end it has to be this way";
                break;
            case 3:
                mssg = "I’ve carved my own path\nYou followed your wrath\nBut maybe we’re both the same";
                break;
            case 4:
                mssg = "The world has turned\nAnd so many have burned\nBut nobody is to blame";
                break;
            case 5:
                mssg = "Yet staring across this barren wasted land\nI feel new life will be born\nBeneath the blood stained sand";
                break;

        }
    }
}
