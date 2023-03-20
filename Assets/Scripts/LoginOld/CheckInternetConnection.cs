using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class CheckInternetConnection : MonoBehaviour
{
    public Canvas InternetCanvas;
    public GameObject InternetImage;
    
    public void Update()
    {
        //StartCoroutine(CheckInternet());
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            InternetImage.SetActive(true);
            InternetCanvas.enabled = true;
            AudioListener.volume = 0;
        }
        else
        {
            InternetImage.SetActive(false);
            InternetCanvas.enabled = false;
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
