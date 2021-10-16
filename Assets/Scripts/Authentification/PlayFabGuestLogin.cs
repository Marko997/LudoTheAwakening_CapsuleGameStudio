using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabGuestLogin : MonoBehaviour
{
    public GameObject randomImage;
    public GameObject randomImage1;
    public void GuestLogin()
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnMobileID(), CreateAccount = true };
        PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnMobileLoginSuccess, OnMobileLoginFailure);

#endif
#if UNITY_IOS && !UNITY_EDITOR
        var requestIOS = new LoginWithIOSDeviceIDRequest { DeviceId =  ReturnMobileID(), CreateAccount = true};
        PlayFabClientAPI.LoginWithIOSDeviceID(requestIOS, OnMobileLoginSuccess, OnMobileLoginFailure);
#endif
    }

    private void OnMobileLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        randomImage.SetActive(true);
    }


    private void OnMobileLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
        randomImage1.SetActive(true);
    }

    public static string ReturnMobileID()
    {
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        return deviceID;
    }

    //If user is logged with deviceId and wants to make account
    public void OnClickAddUserOnMobile()
    {
        //var addLoginRequest = new AddUsernamePasswordRequest { Email = userEmail.text, Password = Encrypt userPassword.text, Username = username.text};
        //PlayFabClientAPI.LoginWithAndroidDeviceID(addLoginRequest, OnMobileAddLoginSuccess, OnMobileAddLoginFailure);
    }
}