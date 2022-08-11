using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class DiceRollerLTA : NetworkBehaviour
{
    public Sprite[] DiceImages;
    public Image Dice;
    [SyncVar]public int currentVal;
    [SyncVar(hook = nameof(HandleDiceValueChanged))]
    public int DiceValue;

    public PlayerEntityLTA player;

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
         //sets currentVal
        player.CmdReturnRandomDiceNumber(currentVal);
        UpdateDiceValue();
        Dice.sprite = DiceImages[currentVal];
        player.CmdRollDiceFromPlayer(currentVal);
        //NetworkGameManagerLTA.instance.RollDice(currentVal);
        
    }

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
