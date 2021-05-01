using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderPawn : MonoBehaviour
{
    public GameObject pawn;

    public SelectionTemplate template;

    public bool isOppened;
    public bool isRed;
    public bool isBlue;
    public bool isGreen;
    public bool isYellow;

    public void ReturnIsOppened(bool _isOppened)
	{
        isOppened = _isOppened;
	}
    public void ReturnIsRed(bool _isRed)
    {
        isRed = _isRed;
    }
    public void ReturnIsBlue(bool _isBlue)
    {
        isBlue = _isBlue;
    }
    public void ReturnIsGreen(bool _isGreen)
    {
        isGreen = _isGreen;
    }
    public void ReturnIsYellow(bool _isYellow)
    {
        isYellow = _isYellow;
    }
}
