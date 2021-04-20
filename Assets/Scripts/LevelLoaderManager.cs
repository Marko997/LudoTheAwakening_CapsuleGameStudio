using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoaderManager : MonoBehaviour
{
    public GameObject loadingScreen;
    public string sceneToLoad;
    AsyncOperation loadingOperation;
    public Slider slider;
    public Text percentLoaded;
    //public int numberOfplayers;

    void Start() 
    {
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


    // public void LoadLevel(int sceneIndex){

    //     StartCoroutine(LoadAsync(sceneIndex));
        

    // }

    // IEnumerator LoadAsync (int sceneIndex){

    //     AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

    //     loadingScreen.SetActive(True);

    //     while(operation.isDone==false){

    //         float progress = Mathf.Clamp01(operation.progress / .9f);

    //         Debug.Log(progress);

    //         yield return null;
    //     }
    // }
}
