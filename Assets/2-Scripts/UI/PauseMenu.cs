using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    #region Pause Options
    public static PauseMenu instance;
    public GameObject pauseMenu;
    public GameObject fondo;
    public GameObject openBook;
    public GameObject closeBook;
    public GameObject derecha_izquierda;
    public GameObject izquierda_derecha;
    public Canvas canvas;
    public List<GameObject> buutonsList = new List<GameObject>();
    #endregion
    public List<GameObject> menuList = new List<GameObject>();
    private int indexMenu;
    private Entity[] entidades;
    [HideInInspector]public bool isPaused;

    private void Awake()
    {        
        instance = this;
        indexMenu = 0;
    }

    void Start()
    {
        pauseMenu.SetActive(false);
        fondo.SetActive(false);
        openBook.SetActive(false);
        closeBook.SetActive(false);
        menuList[indexMenu].SetActive(false);
        isPaused = false;
        entidades = FindObjectsOfType<Entity>();
    }

    void Update()
    {
        Pause();
    }

    public void Pause()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            foreach(Entity ent in entidades)
            {
                if (ent != null)
                {
                    ent.Stop(true);
                }
            }
            isPaused = true;
            openBook.SetActive(true);
            Instantiate(openBook, canvas.transform);
            AudioManager.instance.StopSFX();            
            StartCoroutine(OpenCourutine());
            openBook.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            closeBook.SetActive(true);
            Instantiate(closeBook, canvas.transform);
            StartCoroutine(CloseCourutine());
            closeBook.SetActive(false);
            isPaused = false;
            indexMenu = 0;
            foreach (Entity ent in entidades)
            {
                if (ent != null)
                {
                    ent.Stop(false);
                }
            }
        }

    }
    IEnumerator OpenCourutine()
    {
        yield return new WaitForSeconds(0.6f);
        fondo.SetActive (true);
        menuList[indexMenu].SetActive(true);
        pauseMenu.SetActive(true);
    }
    IEnumerator CloseCourutine()
    {
        fondo.SetActive(false);
        menuList[indexMenu].SetActive(false);

        pauseMenu.SetActive(false);
        yield return new WaitForSeconds(0.5f);       
    }

    public void SwitchMenu()
    {
        indexMenu= 1;
        //Debug.Log("Hola");
        SwitchPage(true);
    }

    public void ToInvetory() 
    {
        indexMenu = 2;
        //buutonsList[1].SetActive(false);
        SwitchPage(true);

    }

    public void ToCrafting()
    {
        indexMenu = 3;
        //buutonsList[2].SetActive(false);
        SwitchPage(true);
    }
    public void ToMain()
    {
        indexMenu = 0;
        //buutonsList[0].SetActive(false);
        SwitchPage(false);
    }

    public void SwitchPage(bool derecha)
    {
        if (derecha)
        {
            derecha_izquierda.SetActive(true);
            Instantiate(derecha_izquierda, canvas.transform);
            StartCoroutine(OpenCourutine());
            derecha_izquierda.SetActive(true);
        }
        else
        {
            izquierda_derecha.SetActive(true);
            Instantiate(izquierda_derecha, canvas.transform);
            StartCoroutine(OpenCourutine());
            izquierda_derecha.SetActive(true);
        }
        PaintMenu();
    }

    public void PaintMenu()
    {
        foreach (var menu in menuList) 
        {
            menu.SetActive(false);
        }
        menuList[indexMenu].SetActive(true);
    }
}
