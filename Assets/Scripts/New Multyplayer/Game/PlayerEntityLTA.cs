using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerEntityLTA : NetworkBehaviour
{
	[Header("Buttons")]
	public Button rollButton;
	public Button powerButton;
    [Header("Bools")]
    [SyncVar(hook = nameof(HandleOnRollButtonStateChanged))] public bool rollButtonState = true;
    [SyncVar(hook = nameof(HandleOnPowerButtonStateChanged))] public bool powerButtonState;
    [SyncVar] public bool isActive;
	[Header("Dice")]
	public DiceRollerLTA diceRoller;
    [Header("Player Data")]
    [SyncVar] public string playerName;
	public enum PlayerColors
	{
		BLUE,
		RED,
		YELLOW,
		GREEN
	}
	public List<PawnLTA> allPawns = new List<PawnLTA>(new PawnLTA[4]);
	[SyncVar] public bool hasTurn;
	public enum PlayerTypes
	{
		HUMAN,
		BOT,
		NO_PLAYER
	}
	public PlayerTypes playerTypes;
    [SyncVar] public PlayerColors playerColors;
    private PlayerColors lastColor;
    public bool hasWon;

	public void HandleOnRollButtonStateChanged(bool oldValue, bool newValue)
    {
		rollButton.interactable = newValue;
    }
    public void HandleOnPowerButtonStateChanged(bool oldValue, bool newValue)
    {
        powerButton.interactable = newValue;
    }
    public override void OnStartAuthority()
    {
        if (!hasAuthority) { return; }
		GetComponentInChildren<Canvas>().enabled = true;
		diceRoller = FindObjectOfType<DiceRollerLTA>();
    }
	public void CmdRollDiceFromPlayer()
    {
        if (!hasAuthority) { return; } //only runs on client which have turn
		diceRoller.currentVal = 0;
        diceRoller.currentVal = Mathf.RoundToInt(Random.Range(0, 6));
        diceRoller.Roll();
		HumanRollDice(diceRoller.currentVal);
    }
    [Command]
	public void HumanRollDice(int value)
    {
        GameManager2.instance.HumanRollDice(value+1);
    }

}
