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
    void Start()
    {
        StartCoroutine(CheckInternet());
    }

    public IEnumerator CheckInternet()
    {

        UnityWebRequest request = new UnityWebRequest("http://google.com");
        yield return request.SendWebRequest();
        if (request.error != null)
        {
            AuthErrorCanvas.SetActive(true);
            //AuthCanvas.SetActive(false);
        }
        else
        {
            AuthErrorCanvas.SetActive(false);
            //AuthCanvas.SetActive(true);
        }

    }

    public void TryAgain()
    {
        //SceneManager.LoadScene("LogingScene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
