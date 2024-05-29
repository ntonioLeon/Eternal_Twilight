using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    //public GameObject darkScreen;
    private int indexMenu;
    private int altIndex;
    [HideInInspector]public bool isPaused;

    [Header("End Screen")]
    [SerializeField] private UI_FadeScreen fadeSceen;
    [SerializeField] private GameObject youDied;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject quitButton;

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
        //darkScreen.SetActive(false);
        isPaused = false;
    }

    void Update()
    {
        Pause();
    }

    public void Pause()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            if (PlayerPrefs.GetString("Logged").Equals("S"))
            {
                PlayFabManager.instance.GetObjectsPrices();
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
        altIndex = indexMenu;
        indexMenu= 1;
        //Debug.Log("Hola");
        SwitchPage(altIndex);
    }

    public void ToInvetory() 
    {
        altIndex = indexMenu;
        indexMenu = 2;
        //buutonsList[1].SetActive(false);
        SwitchPage(altIndex);
    }

    public void ToCrafting()
    {
        altIndex = indexMenu;
        indexMenu = 3;
        //buutonsList[2].SetActive(false);
        SwitchPage(altIndex);
    }
    public void ToMain()
    {
        PlayerPrefs.SetString("Logged", "N");
        altIndex = indexMenu;
        indexMenu = 0;
        //buutonsList[0].SetActive(false);
        SwitchPage(altIndex);
    }

    public void ToLeader()
    {
        altIndex = indexMenu;
        indexMenu = 4;
        //buutonsList[1].SetActive(false);
        SwitchPage(altIndex);
    }
    public void ToInventoryOnline()
    {
        altIndex = indexMenu;
        indexMenu = 5;
        //buutonsList[1].SetActive(false);
        Shop.instance.SetValues();
        SwitchPage(altIndex);
    }


    public void SwitchPage(int altIndex)
    {
        if (altIndex<indexMenu)
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

    public void GoToMainMenu()
    {
        fadeSceen.FadeOut();
        StartCoroutine(CloseCourutine());
        SceneManager.LoadScene(0);
    }

    public void SwichOnEndScreen()
    {
        fadeSceen.FadeOut();

        StartCoroutine(EndScreenCoroutine());
    }

    IEnumerator EndScreenCoroutine()
    {
        yield return new WaitForSeconds(1);
        youDied.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        restartButton.SetActive(true);
        quitButton.SetActive(true);
    }

    public void RestartGameButton()
    {
        GameManager.instance.RestartScene();
    }
}
