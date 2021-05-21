using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Entity : MonoBehaviour
{
	public string playerName;
    public enum PlayerColors {
        BLUE,
        RED,
        YELLOW,
        GREEN
    }
	public PawnManager[] allPawns;
	public bool hasTurn;
	public enum PlayerTypes
	{
		HUMAN,
		BOT,
		NO_PLAYER
	}
	public PlayerTypes playerTypes;
    public PlayerColors playerColors;
	public bool hasWon;



}
