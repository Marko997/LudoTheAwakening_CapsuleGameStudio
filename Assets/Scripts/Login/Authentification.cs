using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Authentification : MonoBehaviour
{
    //public static Scene1 scene1;
    ////public Text UserNamen;
    //public Text playerName;

    //[SerializeField] TextMeshProUGUI TopText;
    [SerializeField] TextMeshProUGUI MessageText;

    [Header("Login")]   
    [SerializeField]
    TMP_InputField EmailLoginInput;
    [SerializeField] TMP_InputField PasswordLoginInput;
    [SerializeField] GameObject LoginPage;

    [Header("Register")]
    [SerializeField]
    TMP_InputField UserNameRegisterInput;
    [SerializeField] TMP_InputField EmailRegisterInput;
    [SerializeField] TMP_InputField PasswordRegisterInput;
    [SerializeField] GameObject RegisterPage;

    [Header("Recovery")]
    [SerializeField]
    TMP_InputField EmailRecoverryInput;
    [SerializeField] GameObject RecoveryPage;

    [Header("WelcomeUserName")]
    [SerializeField]
    public GameObject WelcomeUSerName;
    [SerializeField]
    public Text WelcomeUserNameText;

#region Button Functions

public void RegisterUser()
{
    //if statement if password is less than 6 message text  = Too short password;

    var request = new RegisterPlayFabUserRequest
    {

        DisplayName = UserNameRegisterInput.text,
        Email = EmailRegisterInput.text,
        Password = PasswordRegisterInput.text,

        RequireBothUsernameAndEmail = false

    };

    PlayFabClientAPI.RegisterPlayFabUser(request, OnregisterSucces, OnError);

}

    //public void LoginAsGuest()
    //{
    //    var request = new LoginWithEmailAddressRequest
    //    {
    //        Email = EmailLoginInput.text,
    //        Password = PasswordLoginInput.text,

    //        InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
    //        {

    //            GetPlayerProfile = true

    //        }

    //    };
    //    PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSucess, OnError);
    //}

    public void Login()
{
    var request = new LoginWithEmailAddressRequest
    {
        Email = EmailLoginInput.text,
        Password = PasswordLoginInput.text,

        InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
        {

            GetPlayerProfile = true

        }

    };
    PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSucess, OnError);
}


private void OnLoginSucess(LoginResult result)
{
    string name = null;

    if (result.InfoResultPayload != null)
    {
        name = result.InfoResultPayload.PlayerProfile.DisplayName;
    }

    WelcomeUserNameText.text = "Welcome " + name;
        PlayerData.Name.text = name;
        Debug.Log(PlayerData.Name.text);

    // MessageText.text = "Login in";
    //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    StartCoroutine(LoadNextScene());
}

IEnumerator LoadNextScene()
{
    WelcomeUSerName.SetActive(true);
    MessageText.text = "Login in";
    yield return new WaitForSeconds(0);
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

}

public void RecoverUser()
{
    var request = new SendAccountRecoveryEmailRequest
    {
        Email = EmailRecoverryInput.text,
        TitleId = "F9B15",
    };
    PlayFabClientAPI.SendAccountRecoveryEmail(request, OnReciverySucces, OnError);
}

private void OnReciverySucces(SendAccountRecoveryEmailResult obj)
{
    MessageText.text = "Recovery Email send.Go to your email";
    OpenLoginPage();
}

private void OnError(PlayFabError Error)
{
    MessageText.text = Error.ErrorMessage;
    Debug.Log(Error.GenerateErrorReport());
}

private void OnregisterSucces(RegisterPlayFabUserResult Result)
{
    MessageText.text = "New Acoocunt is Created";
    OpenLoginPage();
}

public void OpenLoginPage()
{

    LoginPage.SetActive(true);
    RegisterPage.SetActive(false);
    RecoveryPage.SetActive(false);

}

public void OpenRegisterPage()
{

    LoginPage.SetActive(false);
    RegisterPage.SetActive(true);
    RecoveryPage.SetActive(false);

}

public void OpenRecoveryPage()
{

    LoginPage.SetActive(false);
    RegisterPage.SetActive(false);
    RecoveryPage.SetActive(true);

}
    #endregion

}
