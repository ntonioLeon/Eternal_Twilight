using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsMain : MonoBehaviour
{
    [Header("Principals")]
    [SerializeField] private GameObject newAccount;
    [SerializeField] private GameObject exisAccount;
    [SerializeField] private GameObject resetPasword;
    [SerializeField] private GameObject noConnection;
    

    [Header("Account Management")]
    [SerializeField] private GameObject userName;
    [SerializeField] private GameObject email;
    [SerializeField] private GameObject password;
    [SerializeField] private GameObject register;
    [SerializeField] private GameObject logIn;
    [SerializeField] private GameObject reset;

    [Header("Right Page")]
    [SerializeField] private GameObject newGame;
    [SerializeField] private GameObject continueGame;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject ranking;

    void Start()
    {
        register.SetActive(false);
        logIn.SetActive(false);
        reset.SetActive(false);
        userName.SetActive(false);
        email.SetActive(false);
        password.SetActive(false);
        //newGame.SetActive(false);
        //continueGame.SetActive(false);
        //settings.SetActive(false);
        //shop.SetActive(false);
        //ranking.SetActive(false);
    }

    void Update()
    {
        
    }

    public void OnCreateNewAccount()
    {
        exisAccount.SetActive(false);
        resetPasword.SetActive(false);
        noConnection.SetActive(false);

        userName.SetActive(true);
        email.SetActive(true);
        password.SetActive(true);
        register.SetActive(true);
    }

    public void OnUsingAccount()
    {
        newAccount.SetActive(false);
        resetPasword.SetActive(false);
        noConnection.SetActive(false);

        email.SetActive(true);
        password.SetActive(true);
        logIn.SetActive(true);
    }

    public void OnResetAccount()
    {
        newAccount.SetActive(false);
        exisAccount.SetActive(false);
        noConnection.SetActive(false);

        email.SetActive(true);
        reset.SetActive(true);
    }

    public void OnNoConnection()
    {
        newAccount.SetActive(false);
        exisAccount.SetActive(false);
        resetPasword.SetActive(false);

    }

    public void GoBack()
    {
        newAccount.SetActive(true);
        exisAccount.SetActive(true);
        resetPasword.SetActive(true);
        noConnection.SetActive(true);

        register.SetActive(false);
        logIn.SetActive(false);
        reset.SetActive(false);
        userName.SetActive(false);
        email.SetActive(false);
        password.SetActive(false);
    }


}
