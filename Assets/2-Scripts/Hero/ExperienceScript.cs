using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;


public class ExperienceScript : MonoBehaviour
{
    public static ExperienceScript instance;
    #region Public Variables
    public Image expImage;
    public float currentExp;
    public float expTNL;
    public float incrementoVida;
    public int incrementoItems;
    //public Text textLvl;
    #endregion

    #region Private Variables
    int lvl;
    #endregion



    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        currentExp = PlayerPrefs.GetFloat("currentExp", 0f);
        expTNL = PlayerPrefs.GetFloat("expTNL", expTNL);
        lvl = PlayerPrefs.GetInt("lvl", 1);
        //textLvl.text = lvl.ToString();
        expImage.fillAmount = currentExp / expTNL;
    }
    public void ExpModifier(float exp)
    {
        //currentExp = PlayerPrefs.GetFloat("currentExp", 0f);
        currentExp += exp;
        //expTNL = PlayerPrefs.GetFloat("expTNL", expTNL);

        if (currentExp >= expTNL)
        {
            currentExp = currentExp - expTNL;//
            expTNL = expTNL * 2;
            float vida = (PlayerHealth.instance.health / PlayerHealth.instance.maxHealth);
            PlayerHealth.instance.maxHealth += incrementoVida;//
            PlayerHealth.instance.health = PlayerHealth.instance.maxHealth * vida;
            //SubItems.instance.maxTotal += incrementoItems;
            //AudioMannager.instance.PlayAudio(AudioMannager.instance.lvlUP);
            lvl++;
            //textLvl.text = lvl.ToString();
        }
        expImage.fillAmount = currentExp / expTNL;
    }

    public void DataToSave()
    {
        /*
        if (DataMannager.instance != null)
        {

            DataMannager.instance.Experience(currentExp);
            DataMannager.instance.Level(lvl);
            DataMannager.instance.ExpTNL(expTNL);
            DataMannager.instance.CurrentSubItem(SubItems.instance.total);
            DataMannager.instance.MaxSubItem(SubItems.instance.maxTotal);
            DataMannager.instance.CurrentCoins(BankAccount.instance.bank);
            DataMannager.instance.MaxHealth(PlayerHealt.instance.maxHealth);
            DataMannager.instance.CurrentHealth(PlayerHealt.instance.health);

            DataMannager.instance.CuurrentPosition(PlayerControler.instance.transform.position);

        }
        else
        {
            Debug.LogError("Fallo al guaradar");
        }

        */
    }
    public void DataToLoad()
    {
        currentExp = PlayerPrefs.GetFloat("currentExp", 0f);
        lvl = PlayerPrefs.GetInt("lvl", 1);
        expTNL = PlayerPrefs.GetFloat("expTNL", expTNL);

        //SubItems.instance.total = PlayerPrefs.GetInt("total", 0);
        //SubItems.instance.maxTotal = PlayerPrefs.GetInt("maxTotal", SubItems.instance.maxTotal);
        //BankAccount.instance.bank = PlayerPrefs.GetInt("bank", 0);

        PlayerHealth.instance.maxHealth = PlayerPrefs.GetFloat("maxHealth", PlayerHealth.instance.maxHealth);
        PlayerHealth.instance.health = PlayerPrefs.GetFloat("health", PlayerHealth.instance.health);
        //PlayerController.instance.transform.position = new Vector2(PlayerPrefs.GetFloat("x"), PlayerPrefs.GetFloat("y"));
    }
}
