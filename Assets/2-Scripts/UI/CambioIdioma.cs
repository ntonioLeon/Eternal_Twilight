using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class CambioIdioma : MonoBehaviour
{
    public static CambioIdioma instance;

    public string[] idioma = { "English", "Español" };
    public Text idiomaText;
    public int indiceIdioma;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        indiceIdioma = PlayerPrefs.GetInt("Idioma", 0);
        CambiarIdioma();
    }

    public void CambiarIdioma()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[indiceIdioma];
        PlayerPrefs.SetInt("Idioma", indiceIdioma);
    }

    public void CambiarDerecha()
    {

        if (indiceIdioma != idioma.Length - 1)
        {
            indiceIdioma += 1;
            idiomaText.text = idioma[indiceIdioma];
            CambiarIdioma();
        }
        else
        {
            indiceIdioma = 0;
            idiomaText.text = idioma[indiceIdioma];
            CambiarIdioma();
        }
    }

    public void CambiarIzquierda()
    {
        if (indiceIdioma != 0)
        {
            indiceIdioma -= 1;
            idiomaText.text = idioma[indiceIdioma];
            CambiarIdioma();
        }
        else
        {
            indiceIdioma = idioma.Length - 1;
            idiomaText.text = idioma[indiceIdioma];
            CambiarIdioma();
        }
    }

    public void lanzarFraseDeFin()
    {
        if (indiceIdioma == 0)
        {
            AudioManager.instance.PlaySFX(14);
        }
        else
        {
            AudioManager.instance.PlaySFX(13);
        }
    }
}
