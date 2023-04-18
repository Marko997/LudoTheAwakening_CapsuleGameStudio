using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum States
{
    WAITING,
    ROLL_DICE,
    ATTACK,
    SWITCH_PLAYER
}

public class BotGameManager : MonoBehaviour
{
    public int playerID;

    public static BotGameManager instance;

    public LudoTile[] board;

    public List<PlayerController> playerList = new List<PlayerController>();

    [Header("PLAYER INFO")]
    public int activePlayer;
    public int numberOfPlayers;
    public States state;

    [Header("BUTTONS")]
    public Button rollButton;
    public Button attackButton;

    [Header("BOOLS")]
    bool switchingPlayer;
    bool turnPossible = true;

    [Header("DICE")]
    [HideInInspector] public int rolledHumanDice;

    void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
