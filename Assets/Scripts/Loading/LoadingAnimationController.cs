using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingAnimationController : MonoBehaviour
{
    public GameObject loadingCanvas;
    public GameObject logoCanvas;

    public PlayFabAuthService AuthService = PlayFabAuthService.Instance;

    private void Start()
    {
        StartCoroutine(ChangeCanvas());
    }

    private IEnumerator ChangeScene()
    {
        AuthService.Authenticate(Authtypes.Silent);

        Debug.Log(PlayerPrefs.GetInt("PlayFabLoginRemember"));

        yield return new WaitForSeconds(4);

        if (PlayerPrefs.GetInt("PlayFabLoginRemember") == 0) // player not remembered
        {
            SceneManager.LoadScene("LoginScene");
        }
        else
        {
            SceneManager.LoadScene("MainScene");
        }
    }

    private IEnumerator ChangeCanvas()
    {
        yield return new WaitForSeconds(4);

        loadingCanvas.SetActive(true);
        logoCanvas.SetActive(false);
        StartCoroutine(ChangeScene());
    }

}
