using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoaderManager : MonoBehaviour
{
    public GameObject loadingScreen;
    public static string sceneToLoad = null;
    AsyncOperation loadingOperation;
    public Slider slider;
    public Text percentLoaded;
    //public int numberOfplayers;

    void Start() 
    {
        //sceneToLoad = "MainScene";
        if(sceneToLoad == null)
		{
            loadingOperation = SceneManager.LoadSceneAsync("MainScene");
        }
        loadingOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        //SaveSettings.numberOfPlayers = numberOfplayers;
        //if token is present to main menu

        //if not loging-register scene 
    }

    void Update()
    {
        float progressValue = Mathf.Clamp01(loadingOperation.progress / 0.9f);
        slider.value = progressValue;
        percentLoaded.text = Mathf.Round(progressValue * 100) + "%";
    }

}
