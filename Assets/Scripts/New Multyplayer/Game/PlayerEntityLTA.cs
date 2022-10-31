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
    [SyncVar(hook =nameof(HandleOnRollButtonStateChanged))]
	public bool rollButtonState = true;
    [SyncVar(hook = nameof(HandleOnPowerButtonStateChanged))]
	public bool powerButtonState;


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

	public void HandleOnRollButtonStateChanged(bool oldValue, bool newValue)
    {
		rollButton.interactable = newValue;
		//Debug.Log($"Roll button state changed to {rollButton.interactable} for {playerColors}");
    }
    public void HandleOnPowerButtonStateChanged(bool oldValue, bool newValue)
    {
        powerButton.interactable = newValue;
        //Debug.Log($"Power button state changed to {rollButton.interactable}");
    }

    public override void OnStartAuthority()
    {
        if (!hasAuthority) { return; }
		//CmdChangeButtons(false, false);
		GetComponentInChildren<Canvas>().enabled = true;
		diceRoller = FindObjectOfType<DiceRollerLTA>();
		//CheckForTurn();
    }
	[Command]
	public void CmdChangeButtons(bool rollButtonState, bool powerButtonState)
    {

	}
	[ClientRpc]
	public void RpcChangeButtons(bool rollButtonState, bool powerButtonState)
    {

	}

	//Check if player has turn and turn on his buttons
	public void CheckForTurn()
    {
   }

	//[Command]
	public void CmdRollDiceFromPlayer()
    {
        if (!hasAuthority) { return; } //only runs on client which have turn
									   //RollDice();
		diceRoller.currentVal = 0;
        diceRoller.currentVal = Mathf.RoundToInt(Random.Range(0, 6));
        diceRoller.Roll();
		//RpcRoll();
		HumanRollDice(diceRoller.currentVal);
        //diceRoller.Roll();
        //RpcRoll();
        //Debug.Log(diceRoller.currentVal + 1);
        //NetworkGameManagerLTA.instance.HumanRollDice(diceRoller.currentVal + 1);
    }
    [ClientRpc]
	public void RpcRoll()
    {
        //diceRoller.currentVal = GameManager2.instance.rolledHumanDice;
        //diceRoller.Roll();
        diceRoller.currentVal = Mathf.RoundToInt(Random.Range(0, 6));
    }

    [Command]
	public void RollDice()
	{ 
        //GameManager2.instance.rolledHumanDice = Mathf.RoundToInt(Random.Range(0, 6));
		//RpcRoll();
        //diceRoller.currentVal = GameManager2.instance.rolledHumanDice;
        //GameManager2.instance.HumanRollDice(diceRoller.currentVal + 1);
    }
    [Command]
	public void HumanRollDice(int value)
    {
        GameManager2.instance.HumanRollDice(value+1);
    }

}
