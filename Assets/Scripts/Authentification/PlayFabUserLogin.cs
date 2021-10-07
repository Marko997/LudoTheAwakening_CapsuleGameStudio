using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class PlayFabUserLogin : MonoBehaviour
{
    public bool ClearPlayerPrefs;

    [SerializeField] private TMP_InputField usernameInputField = default;
    [SerializeField] private TMP_InputField emailInputField = default;
    [SerializeField] private TMP_InputField passwordInputField = default;
    [SerializeField] private TMP_InputField confirmPasswordInputField = default;

    public Button loginButton;
    public Button playAsGuestButton;
    public Button loginWithFacebookButton;
    public Button loginWithGoogleButton;
    public Button registerButton;
    public Button cancelRegisterButton;

    [SerializeField] GameObject registerPanel, loginPanel, startPanel;

    string encryptedPassword;
    public void SwitchToSignUpTab()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
    }

    public void SwitchLoginTab()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
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
            Email = emailInputField.text,
            Username = usernameInputField.text,
            Password = Encrypt(passwordInputField.text),
        };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, RegisterSuccess, RegisterError);
    }

    public void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        //errorSignUp.text = "";
        Debug.Log("suc");
        StartGame();
    }

    public void RegisterError(PlayFabError error)
    {
        //errorSignUp.text = error.GenerateErrorReport(); 
        Debug.Log(error.GenerateErrorReport());
    }
    public void LoginUser() {
        
        var request = new LoginWithEmailAddressRequest {
            Email = emailInputField.text,
            Password = Encrypt(passwordInputField.text)
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, LoginSuccess, LoginError);
    }
    public void LoginSuccess(LoginResult result)
    {
        //errorLogin.text = "";
        Debug.Log("suc");
        StartGame();
    }
    public void LoginError(PlayFabError error)
    {
        //errorLogin.text = error.GenerateErrorReport(); 
        Debug.Log(error.GenerateErrorReport());
    }
    private void StartGame()
    {
        startPanel.SetActive(false);

    }





}
