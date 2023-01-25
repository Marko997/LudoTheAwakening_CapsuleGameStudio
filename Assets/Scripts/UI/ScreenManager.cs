using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour//: MonoSingleton<ScreenManager>
{
    public List<Screen> screenList;
    private Screen lastActiveScreen;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        screenList = GetComponentsInChildren<Screen>().ToList();

        screenList.ForEach(x => x.gameObject.SetActive(false));

        //if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainScene"))
        //{
        //    SwitchScreen(ScreenType.MainMenu);
        //}
        //else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("LoginScene"))
        //{
        //    SwitchCanvas(CanvasType.StartScreen);
        //}
        SwitchScreen(ScreenType.MainMenu);
    }

    public void SwitchScreen(ScreenType type)
    {
        if(lastActiveScreen != null)
        {
            lastActiveScreen.gameObject.SetActive(false);
        }
        Screen desiredScreen = screenList.Find(x => x.screenType == type);

        if (desiredScreen == null)
        {
            Debug.LogWarning($"The desired canvas was not found {desiredScreen}");
            return;
        }
        desiredScreen.gameObject.SetActive(true);
        lastActiveScreen = desiredScreen;

    }


}
