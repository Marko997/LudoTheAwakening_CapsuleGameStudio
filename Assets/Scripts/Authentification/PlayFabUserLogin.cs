using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine.SceneManagement;

public class PlayFabUserLogin : MonoBehaviour
{
    public bool ClearPlayerPrefs;

    [Header("REGISTER PARAMS")]
    [SerializeField] private TMP_InputField usernameInputField = default;
    [SerializeField] private TMP_InputField registerEmailInputField = default; 
    [SerializeField] private TMP_InputField registerPasswordInputField = default;
    [SerializeField] private TMP_Text registerErrorMessageText = default;
    public Button registerButton;

    [Header("LOGIN PARAMS")]
    [SerializeField] private TMP_InputField loginEmailInputField = default;
    [SerializeField] private TMP_InputField loginPasswordInputField = default;
    [SerializeField] private TMP_Text loginErrorMessageText = default;

    public static string SessionTicket;
    public static string EntityId;

    public Button loginButton;
    public Button playAsGuestButton;
    public Button loginWithFacebookButton;
    public Button loginWithGoogleButton;
    public Button loginWithAppleIDButton;
    
    public Button cancelRegisterButton;

    //string encryptedPassword;

    private void Awake()
    {//SHOW GOOGLE OR APPLE LOGIN BUTTON
#if UNITY_ANDROID && !UNITY_EDITOR
        loginWithGoogleButton.gameObject.SetActive(true);
        loginWithAppleIDButton.gameObject.SetActive(false);
#endif

#if UNITY_IOS //&& !UNITY_EDITOR
        loginWithGoogleButton.gameObject.SetActive(false);
        loginWithAppleIDButton.gameObject.SetActive(true);
#endif
        registerErrorMessageText.text = string.Empty;
        loginErrorMessageText.text = string.Empty;

    }

    string Encrypt (string pass)
    {
        System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] bs = System.Text.Encoding.UTF8.GetBytes(pass);
        bs = x.ComputeHash(bs);
        System.Text.StringBuilder s = new System.Text.StringBuilder();

        foreach (byte b in bs)
        {
            s.Append(b.ToString("x2").ToLower());
        }

        return s.ToString();
    }

    public void SignUp()
    {
        var registerRequest = new RegisterPlayFabUserRequest
        {
            Email = registerEmailInputField.text,
            Username = usernameInputField.text,
            Password = Encrypt(registerPasswordInputField.text),
        };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, RegisterSuccess, RegisterError);
    }

    public void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        SessionTicket = result.SessionTicket;
        EntityId = result.EntityToken.Entity.Id;
        registerErrorMessageText.text = "";
        Debug.Log("suc");
        StartGame();
    }

    public void RegisterError(PlayFabError error)
    {
        if (error.Error == PlayFabErrorCode.UsernameNotAvailable) 
        {
            registerErrorMessageText.text = "Username already exists!";
        }else if (error.Error == PlayFabErrorCode.EmailAddressNotAvailable)
        {
            registerErrorMessageText.text = "Email already exists!";
        }
        

    }
    public void LoginUser() {
        
        var request = new LoginWithEmailAddressRequest {
            Email = loginEmailInputField.text,
            Password = Encrypt(loginPasswordInputField.text)
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, LoginSuccess, LoginError);
    }
    public void LoginSuccess(LoginResult result)
    {
        SessionTicket = result.SessionTicket;
        EntityId = result.EntityToken.Entity.Id;
        loginErrorMessageText.text = string.Empty;
        Debug.Log("suc");
        StartGame();
    }
    public void LoginError(PlayFabError error)
    {
        loginErrorMessageText.text = error.ErrorMessage;
        Debug.Log(loginEmailInputField.text);
        Debug.Log(loginPasswordInputField.text);
        Debug.Log(error.GenerateErrorReport());
    }
    private void StartGame()
    {
        //Load next scene
        SceneManager.LoadScene("MainScene");

    }


}
