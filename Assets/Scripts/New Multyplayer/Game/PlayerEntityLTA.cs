using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerEntityLTA : NetworkBehaviour
{
	[Header("Buttons")]
	public Button rollButton;
	public Button powerButton;

	[SyncVar]
	public bool isActive;

	[Header("Dice")]
	public DiceRollerLTA diceRoller;

	[SyncVar]
	public string playerName;
	public enum PlayerColors
	{
		BLUE,
		RED,
		YELLOW,
		GREEN
	}
	public List<PawnLTA> allPawns = new List<PawnLTA>(new PawnLTA[4]);
	[SyncVar] public bool hasTurn;
	public enum PlayerTypes
	{
		HUMAN,
		BOT,
		NO_PLAYER
	}
	public PlayerTypes playerTypes;
	[SyncVar]
	public PlayerColors playerColors;
	public bool hasWon;

	private PlayerColors lastColor;

	private NetworkManagerLTA room;

	private NetworkManagerLTA Room
	{
		get
		{
			if (room != null) { return room; }
			return room = NetworkManager.singleton as NetworkManagerLTA;
		}
	}
    public override void OnStartAuthority()
    {
        if (!hasAuthority) { return; }
		CmdChangeButtons(false, false);
		GetComponentInChildren<Canvas>().enabled = true;
		//CheckForTurn();
    }
	[Command]
	public void CmdChangeButtons(bool rollButtonState, bool powerButtonState)
    {
		RpcChangeButtons(rollButtonState, powerButtonState);
	}
	[ClientRpc]
	public void RpcChangeButtons(bool rollButtonState, bool powerButtonState)
    {
		Debug.Log("butons changed");
		rollButton.interactable = rollButtonState;
		powerButton.interactable = powerButtonState;
	}
	[ClientRpc]
	public void RpcWriteToClients()
    {
		Debug.Log("Everyone should see!");
    }

    [Client]
    private void Update()
    {
        if (!hasAuthority) { return; }
		CheckForTurn();
    }
	//Check if player has turn and turn on his buttons
	public void CheckForTurn()
    {
        if (!hasTurn) { return; }
        
		Debug.Log("My turn");
		rollButton.interactable = true;
		powerButton.interactable = false;
		hasTurn = false;
    }
}
