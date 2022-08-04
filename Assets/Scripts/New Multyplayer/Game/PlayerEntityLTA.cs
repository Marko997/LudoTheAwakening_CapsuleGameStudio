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
	public bool hasTurn;
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

	private PlayerColors lastNumber;

	private NetworkManagerLTA room;

	private NetworkManagerLTA Room
	{
		get
		{
			if (room != null) { return room; }
			return room = NetworkManager.singleton as NetworkManagerLTA;
		}
	}

	[Command]
	public void CmdRollButton()
    {
		if (!hasAuthority) { return; }
		NetworkGameManagerLTA.instance.HumanRoll();
    }

}
