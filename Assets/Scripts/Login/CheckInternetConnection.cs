using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class CheckInternetConnection : MonoBehaviour
{

    //public GameObject AuthCanvas;
    public GameObject AuthErrorCanvas;

    // Start is called before the first frame update
    public void Start()
    {
        //StartCoroutine(CheckInternet());
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            AuthErrorCanvas.SetActive(true);
        }
        else
        {
            AuthErrorCanvas.SetActive(false);
        }
    }

    //public IEnumerator CheckInternet()
    //{

    //    UnityWebRequest request = new UnityWebRequest("http://google.com");
    //    yield return request.SendWebRequest();
    //    if (request.error != null)
    //    {
    //        //Debug.Log(request.error);
    //        AuthErrorCanvas.SetActive(true);        }
    //    else
    //    {
    //        AuthErrorCanvas.SetActive(false);
    //    }

    //}

    public void TryAgain()
    {
        //SceneManager.LoadScene("LogingScene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
