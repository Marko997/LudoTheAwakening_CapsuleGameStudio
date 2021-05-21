using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Roller : MonoBehaviour
{
    public Sprite[] DiceImages;
    public Image One;
    int currentVal;
    public int DiceOneValue;

    public void Roll()
    {
        One.gameObject.GetComponent<Animator>().enabled = true;
        One.gameObject.GetComponent<Animator>().ResetTrigger("RollDice");
        One.gameObject.GetComponent<Animator>().SetTrigger("RollDice");
    }
    public void RollDiceOneFuntion()
    {
        One.gameObject.GetComponent<Animator>().enabled = false;
        One.sprite = RollDice();
        DiceOneValue = currentVal;
        
    }

    public Sprite RollDice()
    {
        int x = Mathf.RoundToInt(Random.Range(0, DiceImages.Length));
        //int x = 5;
        currentVal = x + 1;
        GameManager.instance.RollDice(currentVal);

        return DiceImages[x];
    }
}
