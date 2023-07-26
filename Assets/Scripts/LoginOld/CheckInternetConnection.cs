using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class CheckInternetConnection : MonoBehaviour
{
    public CanvasGroup InternetCanvasGroup;

    public void Update()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            InternetCanvasGroup.alpha = 1;
            InternetCanvasGroup.interactable = true;
            InternetCanvasGroup.blocksRaycasts = true;
            AudioListener.volume = 0;
        }
        else
        {
            InternetCanvasGroup.alpha = 0;
            InternetCanvasGroup.interactable = false;
            InternetCanvasGroup.blocksRaycasts = false;
            AudioListener.volume = 1;
        }
    }
}
