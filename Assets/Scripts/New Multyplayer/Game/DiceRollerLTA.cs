using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class DiceRollerLTA : MonoBehaviour
{
    public Sprite[] DiceImages;
    public Image Dice;
    public int currentVal;
    //public int DiceValue;

    //public PlayerEntityLTA player;

    public void Roll()
    {
        Debug.Log("Rolling "+currentVal);

        Dice.gameObject.GetComponent<Animator>().enabled = true;
        Dice.gameObject.GetComponent<Animator>().ResetTrigger("RollDice");
        Dice.gameObject.GetComponent<Animator>().SetTrigger("RollDice");
    }


    public void RollDice()
    {
        Dice.gameObject.GetComponent<Animator>().enabled = false;

        //GameManager2.instance.ReturnRandomDiceNumber();
        //currentVal = GameManager2.instance.rolledHumanDice;
        Dice.sprite = DiceImages[currentVal];
        //GameManager2.instance.HumanRollDice(currentVal + 1);
        //Use line down bellow when BOTS are added
        //NetworkGameManagerLTA.instance.RollDice(currentVal);

    }

    public void ReturnRandomDiceNumber()
    {
        GameManager2.instance.ReturnRandomDiceNumber();
    }


}
