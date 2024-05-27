using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayFabManager : MonoBehaviour
{
    [Header("UI")]
    public Text messageText;
    public InputField usernameInput;
    public InputField passwordInput;
    public InputField emailInput;

    void Start()
    {

    }

    #region Login
    public void RegisterButton()
    {
        if (passwordInput.text.Length < 6)
        {
            messageText.text = "Password is too short";
        }

        var request = new RegisterPlayFabUserRequest
        {
            Username = usernameInput.text,
            DisplayName = usernameInput.text,
            Password = passwordInput.text,
            Email = emailInput.text,
            RequireBothUsernameAndEmail = true
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageText.text = "Registered and logged in";
    }

    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    void OnLoginSuccess(LoginResult result)
    {
        messageText.text = "Logged as " + result.ToString();
        Debug.Log("Logged as " + result.ToString());        
        //Aqui cargar el inventario, posición y currency.
    }

    public void RequestPasswordButton()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInput.text,
            TitleId = "2C588"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        messageText.text = "Password RESET MAIL SENDT";
    }
    
    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnnSuccess, OnError);
    }

    void OnnSuccess(LoginResult result)
    {
        Debug.Log("Login bien");
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("error.\n" + error);
    }
    #endregion

    #region LeaderBoard
    //Mandar un registro al leaderboard
    public void SendLeaderBoard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "LeaderboardTest",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, onLeaderBoardUpdate, OnError);
    }

    void onLeaderBoardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Se mando bien la puntuacion");
    }

    //Ver los 10 primeros
    public void GetLeaderBoard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "LeaderboardTest",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderBoardGet, OnError);
    }

    void OnLeaderBoardGet(GetLeaderboardResult result)
    {
        foreach (var item in result.Leaderboard)
        {
            Debug.Log((item.Position + 1) + " - " + item.PlayFabId + " - " + item.StatValue + " - " + item.DisplayName);
        }
    }

    public event Action<string> OnSuccess;

    //Verte a ti.
    public void GetLeaderboardAroundPlayer(string playerId, int maxResoultsCount, string leaderboardName)
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            PlayFabId = "AFEAB4936BAC82C5",
            MaxResultsCount = maxResoultsCount, //11
            StatisticName = leaderboardName
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnGetLeaderboardArroundPlayerSuccess, OnGetLeaderboardArroundPlayerFailure);
    }

    private void OnGetLeaderboardArroundPlayerSuccess(GetLeaderboardAroundPlayerResult result)
    {
        var leaderboard = new StringBuilder();
        foreach (var playerLeaderboardEntry in result.Leaderboard)
        {
            //leaderboard.AppendLine($"{playerLeaderboardEntry.Position}.- {playerLeaderboardEntry.DisplayName} {playerLeaderboardEntry.StatValue}");
            Debug.Log($"{playerLeaderboardEntry.Position}.- {playerLeaderboardEntry.PlayFabId}.- {playerLeaderboardEntry.DisplayName}.- {playerLeaderboardEntry.StatValue}");
        }
        OnSuccess?.Invoke(leaderboard.ToString());
    }

    private void OnGetLeaderboardArroundPlayerFailure(PlayFabError error)
    {
        Debug.LogError($"Debug informa: {error.GenerateErrorReport()}");
    }
    #endregion

    public void GetAllInventory(GameData gameData)
    {
        /*
        StringBuilder inventoryParaGuardar = new StringBuilder();
        StringBuilder equipoParaGuardar = new StringBuilder();

        foreach (KeyValuePair<ItemData, InventoryItem> pair in inventory)
        {
            inventoryParaGuardar.Append(pair.Key.name);
            inventoryParaGuardar.Append("#");
            inventoryParaGuardar.Append(pair.Value.stackSize);
            inventoryParaGuardar.Append("|");
        }

        foreach (KeyValuePair<ItemData, InventoryItem> pair in stash)
        {
            inventoryParaGuardar.Append(pair.Key.name);
            inventoryParaGuardar.Append("#");
            inventoryParaGuardar.Append(pair.Value.stackSize);
            inventoryParaGuardar.Append("|");
        }
        if (inventoryParaGuardar.Length > 0)
        {
            inventoryParaGuardar.Remove(inventoryParaGuardar.Length - 1, 1);
        }

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> pair in equipment)
        {
            equipoParaGuardar.Append(pair.Key.name);
            equipoParaGuardar.Append("|");
        }
        if (equipoParaGuardar.Length > 0)
        {
            equipoParaGuardar.Remove(equipoParaGuardar.Length - 1, 1);
        }

        var request = new UpdateUserDataRequest
        {
            Data = new Jsi
            {
                {"Inventory", inventoryParaGuardar.ToString()},                
                {"Equipment", equipoParaGuardar.ToString()}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
        */

        string dataToStore = JsonUtility.ToJson(gameData, true);

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"PlayerData", dataToStore}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("Data saved");
    }


}
