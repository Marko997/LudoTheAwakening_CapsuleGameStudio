using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
public class LogOut : MonoBehaviour
{
    public GameObject Canvas;
    public void LogOutt()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        PlayerPrefs.DeleteAll();

        SceneManager.LoadScene("Login");
        //SceneManager.LoadScene("LoginScene");
    }

}
