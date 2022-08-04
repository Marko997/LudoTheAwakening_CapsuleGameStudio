using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceLTA : MonoBehaviour
{
    //Animator anim;
    public DiceRollerLTA roller;

    public void Roller()
    {
        roller.RollDice();
    }
}
