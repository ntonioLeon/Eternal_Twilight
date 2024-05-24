using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class CambioIdioma : MonoBehaviour
{
    public string[] idioma = { "Español", "English" };
    public Text idiomaText;
    private int indiceIdioma;

    public void CambiarIdioma()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[indiceIdioma];
    }

    public void CambiarDerecha() 
    {
        
        if (!idiomaText.text.Equals(idioma[idioma.Length-1]))
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
        if (!idiomaText.text.Equals(idioma[0]))
        {
            indiceIdioma -= 1;
            idiomaText.text = idioma[indiceIdioma];
            CambiarIdioma();
        }
        else
        {
            indiceIdioma = idioma.Length-1;
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
