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
            PlayerController.instance.stopInput = true;
            pauseMenu.SetActive(true);
            Instantiate(openBook, canvas.transform);
            AudioManager.instance.StopSFX();
            isPaused = true;
            StartCoroutine(OpenCourutine());
            //Time.timeScale = 0f;

        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            //Time.timeScale = 1.0f;

            pauseMenu.SetActive(false);
            isPaused = false;
            PlayerController.instance.stopInput = false;
        }
    }
    IEnumerator OpenCourutine()
    {
        yield return new WaitForSeconds(0.85f);
        fondo.SetActive (true);
        defaultMenu.SetActive(true);    
    }

}
