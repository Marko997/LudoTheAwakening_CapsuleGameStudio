using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class DiceRollerLTA : NetworkBehaviour
{
    public Sprite[] DiceImages;
    public Image Dice;
    [SyncVar]int currentVal;
    [SyncVar(hook = nameof(HandleDiceValueChanged))]
    public int DiceValue;

    private void HandleDiceValueChanged(int oldValue, int newValue) => UpdateDiceValue();

    public void UpdateDiceValue()
    {
        DiceValue = currentVal;
    }

    public void Roll()
    {
        Debug.Log("Rolling");

        Dice.gameObject.GetComponent<Animator>().enabled = true;
        Dice.gameObject.GetComponent<Animator>().ResetTrigger("RollDice");
        Dice.gameObject.GetComponent<Animator>().SetTrigger("RollDice");

        //Dice.sprite = RollDiceSprites();
        //currentVal = ReturnRadnomDiceNumber();
        //Dice.sprite = DiceImages[currentVal];

        //UpdateDiceValue();
    }


    public void RollDice()
    {
        Dice.gameObject.GetComponent<Animator>().enabled = false;
        CmdReturnRadnomDiceNumber(); //sets currentVal
        Dice.sprite = DiceImages[currentVal];
        NetworkGameManagerLTA.instance.RollDice(currentVal);
        UpdateDiceValue();
    }
    //Ne radi u mp, prebacio u RollDice
    //public Sprite RollDiceSprites()
    //{
    //    int x = Mathf.RoundToInt(Random.Range(0, DiceImages.Length));
    //    currentVal = x + 1;

    //    NetworkGameManagerLTA.instance.RollDice(currentVal);
    //    return DiceImages[x];
    //}
    [Command(requiresAuthority = false)]
    public void CmdReturnRadnomDiceNumber()
    {
        currentVal = Mathf.RoundToInt(Random.Range(0, DiceImages.Length));
    }

    [Command(requiresAuthority = false)]
    private void CmdSendCurrentVal(int currentVal)
    {
        NetworkGameManagerLTA.instance.RollDice(currentVal);
    }


}
