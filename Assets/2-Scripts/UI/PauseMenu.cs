using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    public GameObject pauseMenu;

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
            Time.timeScale = 0f;
            AudioManager.instance.StopSFX();
            isPaused = true;
            pauseMenu.SetActive(true);

        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            Time.timeScale = 1.0f;

            pauseMenu.SetActive(false);
            isPaused = false;
        }
    }

}
