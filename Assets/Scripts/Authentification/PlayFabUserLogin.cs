using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabUserLogin : MonoBehaviour
{
    [SerializeField] private GameObject signInDisplay = default;
    [SerializeField] private TMP_InputField usernameInputField = default;
    [SerializeField] private TMP_InputField emailInputField = default;
    [SerializeField] private TMP_InputField passwordInputField = default;

    public static string SessionTicket;

    public void CreateAccount()
    {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Username = usernameInputField.text,
            Email = emailInputField.text,
            Password = passwordInputField.text
        }, result =>
        {
            SessionTicket = result.SessionTicket;
            signInDisplay.SetActive(false);
        }, error =>
         {
             Debug.LogError(error.GenerateErrorReport());
         });
    }

    public void SignIn()
    {
        //With username
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = usernameInputField.text,
            Password = passwordInputField.text
        }, result =>
        {
            SessionTicket = result.SessionTicket;
            signInDisplay.SetActive(false);
        }, error =>
        {
            Debug.LogError(error.GenerateErrorReport());
        });
    }

}
