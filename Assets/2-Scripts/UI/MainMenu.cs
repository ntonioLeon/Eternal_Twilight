using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Info Escenas")]
    [SerializeField] private string sceneName = "Test0";
    public GameObject continueButton;
    public UI_FadeScreen fadeSceen;

    [Header("Info Objetos")]
    public static MainMenu instance;
    public GameObject fondo;
    public GameObject openBook;
    public GameObject closeBook;
    public GameObject derecha_izquierda;
    public GameObject izquierda_derecha;
    public Canvas canvas;
    public List<GameObject> menuList = new List<GameObject>();


    private int indexMenu;
    private int altIndex;
    public bool isLogged;


    private void Awake()
    {
        AudioManager.instance.PlayMain();
        instance = this;
        indexMenu = 0;
    }

    void Start()
    {
        PlayerPrefs.SetString("Logged", "N");
        PlayerPrefs.SetString("Inventario", "");
        PlayerPrefs.SetString("ShopCatalog", "");
        PlayerPrefs.SetString("PlayerID", "");

        //Debug.Log(PlayerPrefs.GetString("Logged"));
        //if (!SaveManager.instance.HasSavedData())
        //{
        //    continueButton.SetActive(false);            
        //}

        fondo.SetActive(false);
        openBook.SetActive(true);
        Instantiate(openBook, canvas.transform);
        //AudioManager.instance.StopSFX();
        StartCoroutine(OpenCorutine());
        openBook.SetActive(false);
    }

    public IEnumerator OpenCorutine()
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
    public IEnumerator CloseCorutine()
    {
        fondo.SetActive(false);
        menuList[indexMenu].SetActive(false);

        yield return new WaitForSeconds(0.6f);
    }

    public void WebLink(string link)
    {
        Application.OpenURL(link);
    }

    IEnumerator LoadScreenWithFadeEffect(float delay)
    {
        fadeSceen.FadeOut();

        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(sceneName);
    }

    #region Buttons
    public void OnLaunchGame()
    {
        AudioManager.instance.StopMain();
        SaveManager.instance.DeleteSavedData();
        StartCoroutine(LoadScreenWithFadeEffect(1.5f));
        ToPlay();
    }

    public void OnContinueGame()
    {
        AudioManager.instance.StopMain();
        StartCoroutine(LoadScreenWithFadeEffect(1.3f));
        if (PlayerPrefs.GetString("Logged").Equals("S"))
        {
            PlayFabManager.instance.DownloadInventory();
        }
        ToPlay();
    }

    public void OnBack()
    {
        altIndex = indexMenu;
        indexMenu = 0;
        //buutonsList[0].SetActive(false);
        SwitchPage(altIndex);
    }
    public void OnSettings()
    {
        altIndex = indexMenu;
        indexMenu = 1;
        //buutonsList[2].SetActive(false);
        SwitchPage(altIndex);
    }
    public void OnRanking()
    {
        altIndex = indexMenu;
        indexMenu = 2;
        //buutonsList[2].SetActive(false);
        SwitchPage(altIndex);
    }
    public void OnExitGame()
    {
        AudioManager.instance.StopMain();
        ToPlay();
        Application.Quit();
    }
    public void SwitchPage(int altIndex)
    {
        if (altIndex < indexMenu)
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
    IEnumerator OpenCourutine()
    {
        yield return new WaitForSeconds(0.6f);
        fondo.SetActive(true);
        menuList[indexMenu].SetActive(true);
    }
    #endregion
}
