using Fungus;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager instance;

    [Header("UI")]
    public Text messageText;
    public Text display;
    public InputField usernameInput;
    public InputField passwordInput;
    public InputField emailInput;
    private StringBuilder sb;

    [SerializeField] private MainMenu mainMenu;    

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    void Start()
    {
        sb = new StringBuilder();
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
        PlayerPrefs.SetString("Logged", "S");
        PlayerPrefs.SetString("PlayerID", result.PlayFabId);
        mainMenu.isLogged = true;
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
        mainMenu.isLogged = true;
        mainMenu.continueButton.SetActive(true);
        PlayerPrefs.SetString("Logged", "S");
        PlayerPrefs.SetString("PlayerID", result.PlayFabId);
        Debug.Log(PlayerPrefs.GetString("Logged"));
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
    public void GetArroundU()
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            PlayFabId = PlayerPrefs.GetString("PlayerID"),
            MaxResultsCount = 3, //11
            StatisticName = "LeaderboardTest"
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnGetLeaderboardArroundPlayerSuccess, OnGetLeaderboardArroundPlayerFailure);
    }
    public void GetUrBest()
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            PlayFabId = PlayerPrefs.GetString("PlayerID"),
            MaxResultsCount = 1, //11
            StatisticName = "LeaderboardTest"
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnGetLeaderboardArroundPlayerSuccess, OnGetLeaderboardArroundPlayerFailure);
    }
    void OnLeaderBoardGet(GetLeaderboardResult result)
    {
        
        sb.Clear();
        sb.Append("Rank\t-\tUnsername\t-\tPoints\n\n");
        foreach (var item in result.Leaderboard)
        {
            sb.Append(item.Position + 1).Append(" - ").Append(item.DisplayName).Append(" - ").Append(item.StatValue).Append("\n");
            //sb.Append((item.Position + 1) + " - " + item.PlayFabId + " - " + item.StatValue + " - " + item.DisplayName+"/n");
        }
        display.text = sb.ToString();
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

    
    public void GetLeaderPlayerBest(string playerId, int maxResoultsCount, string leaderboardName)
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            PlayFabId = "AFEAB4936BAC82C5",
            MaxResultsCount = 1, //11
            StatisticName = leaderboardName
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnGetLeaderboardArroundPlayerSuccess, OnGetLeaderboardArroundPlayerFailure);
    }
    public void OnGetLeaderboardArroundPlayerSuccess(GetLeaderboardAroundPlayerResult result)
    {
        sb.Clear();
        if (result.Leaderboard.Count != 1)
        {
            sb.Append("Unsername\t-\tPoints\n\n");
            foreach (var item in result.Leaderboard)
            {
                sb.Append(item.DisplayName).Append(" - ").Append(item.StatValue).Append("\n");
                //sb.Append((item.Position + 1) + " - " + item.PlayFabId + " - " + item.StatValue + " - " + item.DisplayName+"/n");
            }
        }
        else
        {
            sb.Append(result.Leaderboard[0].DisplayName).Append(" - ").Append(result.Leaderboard[0].StatValue).Append("\n");
        }
        
        display.text = sb.ToString();
    }
    private void OnGetLeaderboardArroundPlayerFailure(PlayFabError error)
    {
        Debug.LogError($"Debug informa: {error.GenerateErrorReport()}");
    }
    #endregion

    #region Inventory
    public void UploadInventory(GameData gameData)
    {
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
        Debug.Log(result.ToString());
    }

    public void DownloadInventory()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataObtained, OnError);
    }

    void OnDataObtained(GetUserDataResult result) 
    {
        if (result.Data == null || !result.Data.ContainsKey("PlayerData") || result.Data["PlayerData"].Value.Length <=0)
        {
            Debug.Log("No hay datos");
            return;
        }
        Debug.Log(result.Data["PlayerData"].Value);

        PlayerPrefs.SetString("Inventario", result.Data["PlayerData"].Value);
    }

    public void GetObjectsPrices()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), OnPricesReceived, OnError);
    }

    void OnPricesReceived(GetTitleDataResult result)
    {
        if (result.Data == null || result.Data.ContainsKey("ItemCost") == false)
        {
            Debug.Log("No hay precios");
            return;
        }

        Debug.Log(result.Data["ItemCost"]);
        PlayerPrefs.SetString("ShopCatalog", result.Data["ItemCost"]);
    }
    #endregion
}
