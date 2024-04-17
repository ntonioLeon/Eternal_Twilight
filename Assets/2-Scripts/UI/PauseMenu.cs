using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    public GameObject pauseMenu;
    public GameObject fondo;
    public GameObject openBook;
    public GameObject closeBook;
    public GameObject defaultMenu;
    public GameObject settingsMenu;
    public Canvas canvas;

    [HideInInspector]public bool isPaused;

    public Animator anim;

    private void Awake()
    {        
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        pauseMenu.SetActive(false);
        fondo.SetActive(false);
        openBook.SetActive(false);
        closeBook.SetActive(false);
        defaultMenu.SetActive(false);
        settingsMenu.SetActive(false);

        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {

        Pause();
    }

    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            isPaused = true;
            openBook.SetActive(true);
            PlayerController.instance.stopInput = true;
            Instantiate(openBook, canvas.transform);
            AudioManager.instance.StopSFX();            
            StartCoroutine(OpenCourutine());
            openBook.SetActive(true);
            //Time.timeScale = 0f;

        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {

            closeBook.SetActive(true);
            Instantiate(closeBook, canvas.transform);
            //Time.timeScale = 1.0f;
            StartCoroutine(CloseCourutine());
            PlayerController.instance.stopInput = false;
            closeBook.SetActive(false);
            isPaused = false;
        }
    }
    IEnumerator OpenCourutine()
    {
        yield return new WaitForSeconds(0.6f);
        fondo.SetActive (true);
        defaultMenu.SetActive(true);
        pauseMenu.SetActive(true);
    }
    IEnumerator CloseCourutine()
    {
        fondo.SetActive(false);
        defaultMenu.SetActive(false);
        pauseMenu.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        
    }
}
