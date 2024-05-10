using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabSignin
{
    public void Signin()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "42"; //Hardcode remove
        }

#if UNITY_ANDROID

        var androidRequest = new LoginWithAndroidDeviceIDRequest
        {
            AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithAndroidDeviceID(androidRequest, OnLoginSuccess, OnLoginFailure);
#elif UNITY_IOS
        var iosRequest = new LoginWithIOSDeviceIDRequest
        {
            DeviceId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithIOSDeviceID(iosRequest, OnLoginSuccess, OnLoginFailure);
#else
        var request = new LoginWithCustomIDRequest { CustomId = "GettingStartedGuide6", CreateAccount = true }; //hardcode remove
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
#endif
    }

    private void InitializeUser()
    {
        var request = new GetTitleDataRequest
        {
            Keys = new List<string> { "InitialUserData" }
        };
        PlayFabClientAPI.GetTitleData(request, dataResult =>
        {
            var data = dataResult.Data["InitialUserData"];
            Debug.Log(data);
            var initalUserData = JsonUtility.FromJson<InitialUserData>(data);
            //Debug.Log("SC Inicial       123= " + initalUserData.InitialSoftCurrency);
            //Debug.Log("TutorialEnabled true= " + initalUserData.TutorialEnabled);

            PlayFabClientAPI.AddUserVirtualCurrency(new AddUserVirtualCurrencyRequest
            {
                Amount = initalUserData.InitialSoftCurrency,
                VirtualCurrency = "SC"
            },
                result => { },
                error => { });

            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
                {
                    { "Tutorial", JsonUtility.ToJson(new TutorialCofiguration{ isEnabled = initalUserData.TutorialEnabled}) },
                    { "IsInitialized", JsonUtility.ToJson(new UserInitialized{ IsInitialized = true})}
                }
            },
                result => { },
                error => { });
        }, error => { });
    }
    public void OnLoginSuccess(LoginResult result)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest { Keys = new List<string> { "IsInitialized" } },
            dataResult =>
            {
                if (!dataResult.Data.ContainsKey("IsInitialized"))
                {
                    InitializeUser();
                }
            }, error => { });

        Debug.Log("Registered");
    }
    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Fail");
        Debug.LogError(error.GenerateErrorReport());
    }
}
