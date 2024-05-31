using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static System.Net.WebRequestMethods;

public class ButtonsMain : MonoBehaviour
{
    public static ButtonsMain instance;

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
    [SerializeField] private GameObject ranking;

    private bool isConnected = false;
    public bool isRegistered = false;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
    void Start()
    {
        CheckInternet();
        if (isConnected)
        {
            newAccount.SetActive(true);
            exisAccount.SetActive(true);
            resetPasword.SetActive(true);
        }
        else
        {
            newAccount.SetActive(false);
            exisAccount.SetActive(false);
            resetPasword.SetActive(false);
        }

        register.SetActive(false);
        logIn.SetActive(false);
        reset.SetActive(false);
        userName.SetActive(false);
        email.SetActive(false);
        password.SetActive(false);

        newGame.SetActive(false);
        continueGame.SetActive(false);
        settings.SetActive(false);
        ranking.SetActive(false);
    }

    void Update()
    {

    }
    private void CheckInternet()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            isConnected = false;
        }
        else
        {
            isConnected = true;
        }
    }

    public void CheckRegister(bool value)
    {
        isRegistered = value;
        newGame.SetActive(value);
        continueGame.SetActive(value);
        settings.SetActive(value);
        ranking.SetActive(value);
    }
    #region Buttons
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

        newGame.SetActive(true);
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

        newGame.SetActive(false);
        continueGame.SetActive(false);
        settings.SetActive(false);
        ranking.SetActive(false);

        PlayFabManager.instance.messageText.text = "";
    }
    #endregion
}
