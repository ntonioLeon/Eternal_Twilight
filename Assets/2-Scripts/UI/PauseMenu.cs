using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [HideInInspector] public bool isPaused;

    [Header("End Screen")]
    [SerializeField] private UI_FadeScreen fadeSceen;
    [SerializeField] private GameObject youDied;
    //[SerializeField] private GameObject tryAgainButton;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject quitButton;

    [Header("Dialog Intro")]
    [SerializeField] private GameObject colliderEs;
    [SerializeField] private GameObject colliderEn;

    private void Awake()
    {
        instance = this;
        indexMenu = 0;
    }

    void Start()
    {
        if (!PlayerPrefs.GetString("Logged").Equals("S"))
        {
            buutonsList[3].SetActive(false);
            buutonsList[4].SetActive(false);
        }

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

        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused && !GameObject.Find("Player").GetComponent<Player>().isSpeaking && !PlayerManager.instance.player.GetComponent<PlayerStats>().isDead)
        {
            AudioManager.instance.SoundsMute();
            if (PlayerPrefs.GetString("Logged").Equals("S"))
            {
                PlayFabManager.instance.GetObjectsPrices();
            }
            isPaused = true;
            openBook.SetActive(true);
            Instantiate(openBook, canvas.transform);
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
            AudioManager.instance.SoundsUnmute();
        }

    }
    IEnumerator OpenCourutine()
    {
        yield return new WaitForSeconds(0.6f);
        fondo.SetActive(true);
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
        indexMenu = 1;
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
    public void ToControls()
    {
        altIndex = indexMenu;
        indexMenu = 6;
        //buutonsList[1].SetActive(false);
        Shop.instance.SetValues();
        SwitchPage(altIndex);
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

    public void GoToMainMenu()
    {
        PlayerPrefs.SetString("Logged", "N");
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
        //tryAgainButton.SetActive(true);
        restartButton.SetActive(true);
        quitButton.SetActive(true);
    }

    public void SwichOffEndScreen()
    {
        fadeSceen.FadeIn();

        StartCoroutine(EndScreenOffCoroutine());
    }

    IEnumerator EndScreenOffCoroutine()
    {
        yield return new WaitForSeconds(1);
        youDied.SetActive(false);        
        //tryAgainButton.SetActive(false);
        restartButton.SetActive(false);
        quitButton.SetActive(false);
    }

    public void RestartGameButton()
    {
        PlayerManager.instance.player.GetComponent<PlayerStats>().isDead = false;
        GameManager.instance.RestartScene();
    }

    public void Respawn()
    {
        PlayerManager.instance.player.GetComponent<PlayerStats>().isDead = false;
        GameManager.instance.RespawnPlayer();
    }

    public void ActivarDialog()
    {
        if (CambioIdioma.instance.indiceIdioma == 1)
        {
            colliderEs.SetActive(true);
        }
        else
        {
            colliderEn.SetActive(true);
        }
    }
}
