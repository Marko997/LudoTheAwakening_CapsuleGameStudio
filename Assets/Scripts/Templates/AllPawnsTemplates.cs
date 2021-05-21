using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pawn", menuName = "Pawns/AllPawnsTemplate", order = 1)]
public class AllPawnsTemplates : ScriptableObject
{
    //public List<GameObject> pawnTemplates;
    public Transform cameraPivot;

    public GameObject commonRoute;

    public GameObject yellowRoute;
    public GameObject yellowBase;
    public GameObject yellowPawn;
    public GameObject yellowSelector;

    public GameObject redRoute;
    public GameObject redBase;
    public GameObject redPawn;
    public GameObject redSelector;

    public GameObject blueRoute;
    public GameObject blueBase;
    public GameObject bluePawn;
    public GameObject blueSelector;

    public GameObject greenRoute;
    public GameObject greenBase;
    public GameObject greenPawn;
    public GameObject greenSelector;


    
}
