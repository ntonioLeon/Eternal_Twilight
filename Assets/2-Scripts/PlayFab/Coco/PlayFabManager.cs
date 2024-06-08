using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

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
    public Text textoPuntuacion;

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
        messageText.text = "";

        if (string.IsNullOrEmpty(usernameInput.text) || string.IsNullOrWhiteSpace(usernameInput.text))
        {
            messageText.text = "Username cannot be empty.";
        }
        else if (usernameInput.text.Length < 3)
        {
            messageText.text = "Username is too short.\nIt must be at least 3 characters long.";
        }
        else if (string.IsNullOrEmpty(emailInput.text) || string.IsNullOrWhiteSpace(emailInput.text))
        {
            messageText.text = "Email cannot be empty.";
        }
        else if (!IsValidEmail(emailInput.text))
        {
            messageText.text = "Invalid email.\nPlease enter a valid email.";
        }
        else if (string.IsNullOrEmpty(passwordInput.text) || string.IsNullOrWhiteSpace(passwordInput.text))
        {
            messageText.text = "Password cannot be empty.";
        }
        else if (passwordInput.text.Length < 6)
        {
            messageText.text = "Password is too short.\nIt must be at least 6 characters long.";
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
        ButtonsMain.instance.CheckRegister(true);
    }

    public void LoginButton()
    {
        messageText.text = "";

        if (string.IsNullOrEmpty(emailInput.text) || string.IsNullOrWhiteSpace(emailInput.text))
        {
            messageText.text = "Email cannot be empty.";
        }
        else if (!IsValidEmail(emailInput.text))
        {
            messageText.text = "Invalid email.\nPlease enter a valid email.";
        }
        else if (string.IsNullOrEmpty(passwordInput.text) || string.IsNullOrWhiteSpace(passwordInput.text))
        {
            messageText.text = "Password cannot be empty.";
        }
        else if (passwordInput.text.Length < 6)
        {
            messageText.text = "Password is too short.\nIt must be at least 6 characters long.";
        }

        var request = new LoginWithEmailAddressRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    void OnLoginSuccess(LoginResult result)
    {
        PlayerPrefs.SetString("Logged", "S");
        PlayerPrefs.SetString("PlayerID", result.PlayFabId);
        messageText.text = "Successfully logged in!";

        mainMenu.isLogged = true;
        mainMenu.continueButton.SetActive(true);

        ButtonsMain.instance.CheckRegister(true);
        //Debug.Log("Logged as " + result.ToString());
        //Debug.Log(PlayerPrefs.GetString("Logged"));
        //Aqui cargar el inventario, posición y currency.
    }

    public void RequestPasswordButton()
    {
        messageText.text = "";

        if (string.IsNullOrEmpty(emailInput.text) || string.IsNullOrWhiteSpace(emailInput.text))
        {
            messageText.text = "Email cannot be empty.";
        }
        else if (!IsValidEmail(emailInput.text))
        {
            messageText.text = "Invalid email.\nPlease enter a valid email.";
        }
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInput.text,
            TitleId = "2C588"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnErrorPass);
    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        messageText.text = "We have sent you an email to reset your password.\nPlease check your email.";
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
        //Debug.Log("Login bien");
    }

    void OnError(PlayFabError error)
    {
        messageText.text += "\n" + error.ErrorMessage;
    }
    void OnErrorPass(PlayFabError error)
    {
        messageText.text += "\nThere is no account associated with this email.\nPlease verify that the entered email is correct.";
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
        //Debug.Log("Se mando bien la puntuacion");
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

    public void GetArroundU2()
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            PlayFabId = PlayerPrefs.GetString("PlayerID"),
            MaxResultsCount = 3, //11
            StatisticName = "LeaderboardTest"
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnGetLeaderboardArroundPlayerSuccess2, OnGetLeaderboardArroundPlayerFailure);
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

    public void OnGetLeaderboardArroundPlayerSuccess2(GetLeaderboardAroundPlayerResult result)
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

        textoPuntuacion.text = sb.ToString();
    }

    private void OnGetLeaderboardArroundPlayerFailure(PlayFabError error)
    {
        //Debug.LogError($"Debug informa: {error.GenerateErrorReport()}");
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
        //Debug.Log("Data saved");
        //Debug.Log(result.ToString());
    }

    public void DownloadInventory()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataObtained, OnError);
    }

    void OnDataObtained(GetUserDataResult result)
    {
        if (result.Data == null || !result.Data.ContainsKey("PlayerData") || result.Data["PlayerData"].Value.Length <= 0)
        {
            Debug.Log("No hay datos");
            return;
        }
        //Debug.Log(result.Data["PlayerData"].Value);
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

        //Debug.Log(result.Data["ItemCost"]);
        PlayerPrefs.SetString("ShopCatalog", result.Data["ItemCost"]);
    }
    #endregion

    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            // Expresión regular para validar el correo electrónico
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
}
