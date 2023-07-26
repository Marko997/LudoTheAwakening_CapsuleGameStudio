using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BotPlayerTypes
{
    HUMAN,
    CPU,
    NO_PLAYER
}

[System.Serializable]
public class Player : MonoBehaviour
{

    public string playerName;
    public Pawn[] allPawns;

    public bool hasTurn;
    public bool hasWon;

    public Color playerColor;
    
    public BotPlayerTypes playerType;

}
