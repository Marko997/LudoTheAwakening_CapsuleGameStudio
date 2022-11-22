using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScreenManager : MonoSingleton<ScreenManager>
{
    List<Screen> screenList;
    private Screen lastActiveScreen;

    //public CheckInternetConnection Check;
    void Awake()
    {
        //Check = FindObjectOfType<CheckInternetConnection>();

        screenList = GetComponentsInChildren<Screen>().ToList();

        screenList.ForEach(x => x.gameObject.SetActive(false));

        //if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainScene"))
        //{
        //    SwitchCanvas(CanvasType.MainMenu);
        //}
        //else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("LoginScene"))
        //{
        //    SwitchCanvas(CanvasType.StartScreen);
        //}
        SwitchScreen(ScreenType.MainMenu);
        
    }

    //private void Update()
    //{
    //    Check.StartCoroutine(Check.CheckInternet());
    //}

    public void SwitchScreen(ScreenType type)
    {
        if(lastActiveScreen != null)
        {
            lastActiveScreen.gameObject.SetActive(false);
        }
        Screen desiredScreen = screenList.Find(x => x.screenType == type);

        if(desiredScreen == null)
        {
            Debug.LogWarning($"The desired canvas was not found {desiredScreen}");
            return;
        }
        desiredScreen.gameObject.SetActive(true);
        lastActiveScreen = desiredScreen;

    }


}
