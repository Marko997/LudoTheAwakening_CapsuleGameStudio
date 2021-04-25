using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PawnCard", menuName = "Pawn/PawnCard", order = 1)]
public class PawnTemplate : ScriptableObject
{
    public PawnManager pawnObject;

    public string cardName;
    public Sprite cardSprite;

    public bool locked;

    [TextArea(1, 10)]
    public string cardHistory;

    [TextArea(1, 8)]
    public string cardDescription;

    //public int attackRange;

    public int currentExperience;
    public int upgradeCost;
    public int cardLevel;
    
}
