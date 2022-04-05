using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MP_playerEntity : NetworkBehaviour
{
	[SerializeField] private string playerName;
	[SerializeField] private enum PlayerColors
	{
		BLUE,
		RED,
		YELLOW,
		GREEN
	}
	[SerializeField] private PawnManager[] allPawns;
	[SerializeField] private bool hasTurn;
	[SerializeField] private enum PlayerTypes
	{
		HUMAN,
		BOT,
		NO_PLAYER
	}
	[SerializeField] private PlayerTypes playerTypes;
	[SerializeField] private PlayerColors playerColors;
	[SerializeField] private bool hasWon;

    public string PlayerName { get; set; }


}
