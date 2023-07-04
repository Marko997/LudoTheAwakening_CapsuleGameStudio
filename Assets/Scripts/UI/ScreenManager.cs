using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour//: MonoSingleton<ScreenManager>
{
    public List<ScreenCustom> screenList;
    private ScreenCustom lastActiveScreen;

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
        screenList = GetComponentsInChildren<ScreenCustom>().ToList();

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
        ScreenCustom desiredScreen = screenList.Find(x => x.screenType == type);

        if (desiredScreen == null)
        {
            Debug.LogWarning($"The desired canvas was not found {desiredScreen}");
            return;
        }
        desiredScreen.gameObject.SetActive(true);
        lastActiveScreen = desiredScreen;

    }


}
