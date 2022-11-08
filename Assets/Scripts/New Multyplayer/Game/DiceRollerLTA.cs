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
        Dice.sprite = DiceImages[currentVal];
    }
    public void ReturnRandomDiceNumber()
    {
        GameManager2.instance.ReturnRandomDiceNumber();
    }
}
