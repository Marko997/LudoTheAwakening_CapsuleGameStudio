using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
	PawnSelectionScreen,
	Pawn2SelectionScreen,
	Pawn3SelectionScreen,
	Pawn4SelectionScreen,
	RedSelectionScreen,
	Red2SelectionScreen,
	Red3SelectionScreen,
	Red4SelectionScreen,
	GreenSelectionScreen,
	Green2SelectionScreen,
	Green3SelectionScreen,
	Green4SelectionScreen,
	BlueSelectionScreen,
	Blue2SelectionScreen,
	Blue3SelectionScreen,
	Blue4SelectionScreen,
	YellowSelectionScreen,
	Yellow2SelectionScreen,
	Yellow3SelectionScreen,
	Yellow4SelectionScreen
}


public class CanvasManager : Singleton<CanvasManager>
{
    List<CanvasController> canvasControllerList;
	CanvasController lastActiveCanvas;

	protected override void Awake()
	{
		base.Awake();
		canvasControllerList = GetComponentsInChildren<CanvasController>().ToList();

		canvasControllerList.ForEach(x => x.gameObject.SetActive(false));
		SwitchCanvas(CanvasType.MainMenu);
		
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
