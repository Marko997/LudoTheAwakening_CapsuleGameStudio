using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public enum CanvasType
{
    MainMenu,
    GameUI,
    EndScreen,
	PlayScreen,
	LoadoutScreen,
	ShopScreen,
	EventsScreen,
	PremiumPassScreen,
	SettingsScreen,
	MessagesScreen,
	FriendsScreen,
	ProfileScreen,
	OfflineSettingsScreen,
    OfflineSettingsScreen2,
	LoginScreen,
	RegisterScreen,
	StartScreen
}


public class CanvasManager : Singleton<CanvasManager>
{
    List<CanvasController> canvasControllerList;
	public CanvasController lastActiveCanvas;

	//static Scene currentScene = SceneManager.GetActiveScene();
	//string nameOfCurrentScene = currentScene.name;

	protected override void Awake()
	{
		base.Awake();
		canvasControllerList = GetComponentsInChildren<CanvasController>().ToList();

		canvasControllerList.ForEach(x => x.gameObject.SetActive(false));

		if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainScene"))
		{
			SwitchCanvas(CanvasType.MainMenu);
		}
		else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("LoginScene"))
		{
			SwitchCanvas(CanvasType.StartScreen);
		}
	}

    public void SwitchCanvas(CanvasType _type)
	{
		if(lastActiveCanvas != null)
		{
			lastActiveCanvas.gameObject.SetActive(false);
		}
		CanvasController desiredCanvas = canvasControllerList.Find(x => x.canvasType == _type);

		if (desiredCanvas != null)
		{
			desiredCanvas.gameObject.SetActive(true);
			lastActiveCanvas = desiredCanvas;
		}
		else
		{
			Debug.LogWarning("The desired canvas was not found");
		}
	}
}
