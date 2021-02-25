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
    // Start is called before the first frame update

    public void Roll1()
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
        currentVal = x + 1;
        GameManager.instance.RollDice(currentVal);
        //Debug.Log(currentVal);
        return DiceImages[x];
    }
}
