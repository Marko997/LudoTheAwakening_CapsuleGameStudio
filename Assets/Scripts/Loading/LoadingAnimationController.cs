using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingAnimationController : MonoBehaviour
{
    //////public GameObject loadingCanvas;
    //////public GameObject logoCanvas;

    //////public PlayFabAuthService AuthService = PlayFabAuthService.Instance;
    //////private void Start()
    //////{
    //////    StartCoroutine(ChangeCanvas());
    //////}

    //////private IEnumerator ChangeScene()
    //////{
    //////    AuthService.Authenticate(Authtypes.Silent);

    //////    Debug.Log(PlayerPrefs.GetInt("PlayFabLoginRemember"));

    //////    yield return new WaitForSeconds(4);

    //////    if (PlayerPrefs.GetInt("PlayFabLoginRemember") == 0) // player not remembered
    //////    {
    //////        SceneManager.LoadScene("LoginScene");
    //////    }
    //////    else
    //////    {
    //////        SceneManager.LoadScene("MainScene");
    //////    }
    //////}

    //////private IEnumerator ChangeCanvas()
    //////{
    //////    yield return new WaitForSeconds(4);

    //////    loadingCanvas.SetActive(true);

    //////    StartCoroutine(ChangeScene());
    //////}

    public GameObject loadingCanvas;
    public GameObject logoCanvas;
    //public float sliderAnimationSpeed = 0.5f;
    //public float sliderMoveDelay = 0.1f;
    public float sliderAnimationSpeed;
    public float sliderMoveDelay;
    public PlayFabAuthService AuthService = PlayFabAuthService.Instance;


    private void Awake()
    {
        sliderAnimationSpeed = Random.Range(0.3f, 0.8f);
        sliderMoveDelay = Random.Range(0.05f, 0.3f);
    }

    private void Start()
    {
        StartCoroutine(ChangeCanvas());
    }

    private IEnumerator ChangeScene()
    {
        AuthService.Authenticate(Authtypes.Silent);

        Debug.Log(PlayerPrefs.GetInt("PlayFabLoginRemember"));

        yield return new WaitForSeconds(0.5f);

        if (PlayerPrefs.GetInt("PlayFabLoginRemember") == 0) // player not remembered
        {
            SceneManager.LoadScene("LoginScene");
        }
        else
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainScene");
            asyncLoad.allowSceneActivation = false;

            Slider loadingSlider = loadingCanvas.GetComponentInChildren<Slider>();
            loadingSlider.value = 0.1f;

            // Start slider animation
            float startTime = Time.time;
            while (Time.time - startTime < sliderMoveDelay)
            {
                loadingSlider.value = Mathf.Lerp(0.1f, 0.5f, (Time.time - startTime) / sliderMoveDelay);
                yield return null;
            }

            while (!asyncLoad.isDone)
            {
                float targetValue = asyncLoad.progress;
                while (loadingSlider.value < targetValue)
                {
                    loadingSlider.value += Time.deltaTime * sliderAnimationSpeed;
                    yield return null;
                }

                if (asyncLoad.progress >= 0.9f && loadingSlider.value >= 0.9f)
                {
                    loadingSlider.value = 1f;
                    asyncLoad.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }

    private IEnumerator ChangeCanvas()
    {
        yield return new WaitForSeconds(4);

        loadingCanvas.SetActive(true);

        yield return StartCoroutine(ChangeScene());
    }

}
