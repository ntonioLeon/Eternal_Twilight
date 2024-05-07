using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;
    public GameObject fondo;
    public GameObject openBook;
    public GameObject closeBook;
    public GameObject derecha_izquierda;
    public GameObject izquierda_derecha;
    public Canvas canvas;
    public List<GameObject> menuList = new List<GameObject>();
    private int indexMenu;

    private void Awake()
    {
        instance = this;
        indexMenu = 0;
    }

    void Start()
    {
        fondo.SetActive(false);
        openBook.SetActive(true);
        Instantiate(openBook, canvas.transform);
        //AudioManager.instance.StopSFX();
        StartCoroutine(OpenCorutine());
        openBook.SetActive(false);
    }

    IEnumerator OpenCorutine()
    {
        yield return new WaitForSeconds(0.6f);
        fondo.SetActive(true);
        menuList[indexMenu].SetActive(true);
    }
    
    public void ToPlay()
    {
        closeBook.SetActive(true);
        Instantiate(closeBook, canvas.transform);
        StartCoroutine(CloseCorutine());

    }
    IEnumerator CloseCorutine()
    {
        fondo.SetActive(false);
        menuList[indexMenu].SetActive(false);

        yield return new WaitForSeconds(0.6f);
    }

    public void WebLink(string link)
    {
        Application.OpenURL(link);
    }
}
