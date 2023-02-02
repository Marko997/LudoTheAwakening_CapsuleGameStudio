using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class CheckInternetConnection : MonoBehaviour
{

    public GameObject AuthErrorCanvas;

    public void Update()
    {
        //StartCoroutine(CheckInternet());
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            AuthErrorCanvas.SetActive(true);
            AudioListener.volume = 0;
        }
        else
        {
            AuthErrorCanvas.SetActive(false);
            AudioListener.volume = 1;
        }
    }
    ////Ovo nam pravi problem sa kanvasom
    //public void TryAgain()
    //{
    //    //SceneManager.LoadScene("LogingScene");
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //}
}