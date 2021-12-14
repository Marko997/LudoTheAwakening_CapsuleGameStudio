using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;


public class MpGameManager : NetworkBehaviour
{
    [SerializeField] private int playerID;

    //public static GameManager instance;
    [SerializeField] private enum States
    {
        WAITING,
        ROLL_DICE,
        ATTACK,
        SWITCH_PLAYER
    }
    [SerializeField] private CommonRouteManager commonRoute;

    [SerializeField] private List<Entity> playerList = new List<Entity>();

    public List<MP_playerEntity> PlayerList { get; set; }

    [Header("TEMPLATES")]
    [SerializeField] private AllPawnsTemplates templates;
    [SerializeField] private EntityTemplate playerEntity;

    [Header("PLAYER INFO")]
    [SerializeField] private int activePlayer;
    [SerializeField] private int numberOfPlayers;
    [SerializeField] private States state;

    [Header("BUTTONS")]
    [SerializeField] private Button rollButton;
    [SerializeField] private Button powerButton;

    [Header("BOOLS")]
    [SerializeField] private bool switchingPlayer;
    [SerializeField] private bool turnPossible = true;

    [Header("DICE")]
    [SerializeField] private Roller dice;
    [SerializeField] private int rolledHumanDice;


}
