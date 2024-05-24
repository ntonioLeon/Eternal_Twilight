using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "Test0";
    [SerializeField] private GameObject continueButton;
    [SerializeField] UI_FadeScreen fadeSceen;

    private void Start()
    {
        if (!SaveManager.instance.HasSavedData())
        {
            continueButton.SetActive(false);
            //Buscar por loggin too.
        }
    }

    public void LaunchGame()
    {
        SaveManager.instance.DeleteSavedData();
        StartCoroutine(LoadScreenWithFadeEffect(1.5f));
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadScreenWithFadeEffect(1.3f));        
    }

    public void ExitGame()
    {
        Application.Quit();
    }    

    IEnumerator LoadScreenWithFadeEffect(float delay)
    {
        fadeSceen.FadeOut();

        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(sceneName);
    }
}
