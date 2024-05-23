using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadeScreen : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private GameObject textoFin;

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
        StartCoroutine(Creditos());
    }

    private IEnumerator Creditos()
    {
        yield return new WaitForSeconds(1.5f);

        textoFin.SetActive(true);
    }
}
