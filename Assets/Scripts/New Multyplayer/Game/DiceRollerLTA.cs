using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class DiceRollerLTA : NetworkBehaviour
{
    public Sprite[] DiceImages;
    public Image Dice;
    int currentVal;
    public int DiceValue;

    public void Roll()
    {
        Debug.Log("Rolling");

        Dice.gameObject.GetComponent<Animator>().enabled = true;
        Dice.gameObject.GetComponent<Animator>().ResetTrigger("RollDice");
        Dice.gameObject.GetComponent<Animator>().SetTrigger("RollDice");

        Dice.sprite = RollDiceSprites();
        DiceValue = currentVal;

    }


    public void RollDice()
    {
        Dice.gameObject.GetComponent<Animator>().enabled = false;
        Dice.sprite = RollDiceSprites();
        DiceValue = currentVal;
    }

    public Sprite RollDiceSprites()
    {
        int x = Mathf.RoundToInt(Random.Range(0, DiceImages.Length));

        currentVal = x + 1;

        Debug.Log(currentVal + " dice number");
        NetworkGameManagerLTA.instance.RollDice(currentVal);
        return DiceImages[x];
    }
}
