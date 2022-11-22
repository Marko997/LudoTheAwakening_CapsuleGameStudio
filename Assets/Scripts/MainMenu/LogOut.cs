using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
public class LogOut : MonoBehaviour
{
    
    public void LogOutt()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("LoginScene");
    }

}
