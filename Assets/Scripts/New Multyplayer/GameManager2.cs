using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.CullingGroup;
using Unity.Collections.LowLevel.Unsafe;

public enum States
{
    WAITING,
    ROLL_DICE,
    ATTACK,
    SWITCH_PLAYER
}

public class GameManager2 : NetworkBehaviour
{
    public static GameManager2 instance;

    [SyncVar] public int activePlayer;
    [SyncVar] private int currentDiceRoll;
    [SyncVar] private bool moveCompleted;
    [SyncVar] private bool switchingPlayer;
    [SyncVar] private bool turnPossible = true;

    //[SyncVar] private int activePlayer;

    [SerializeField] private GameObject testPawnPrefab;
    [SerializeField] private GameObject commonRoutePrefab;
    [SerializeField] private GameObject redRoutePrefab;
    [SerializeField] private GameObject redBasePrefab;
    [SerializeField] private GameObject blueRoutePrefab;
    [SerializeField] private GameObject blueBasePrefab;
    [SerializeField] private Transform commonRoute;
    [SerializeField] private Transform startPositionContainer;

    //Server side values
    public List<PlayerEntityLTA> playerList = new List<PlayerEntityLTA>();
    public Dictionary<int, PlayerEntityLTA> playerDictionary = new Dictionary<int, PlayerEntityLTA>();
    private Dictionary<int, bool> playerReady = new Dictionary<int, bool>();
    private Dictionary<int, PawnLTA[]> playerPawns = new Dictionary<int, PawnLTA[]>();
    private Dictionary<int, bool> playerCompleted = new Dictionary<int, bool>();
    private Dictionary<int, int[]> paths;
    private Dictionary<int, Vector3[]> startPosition;
    private NodeLTA[] board;
    private int[] finalTiles = new int[4];
    private int rollCountThisTurn;
    private bool canRollAgain;

    //Client side values
    private PawnLTA[] localPaws = new PawnLTA[4];
    private bool pawnsSpawned = false;
    [SerializeField] private GameObject commonRouteInstance;
    [SyncVar] public int rolledHumanDice;
    [SyncVar(hook =nameof(OnStateChanged))]
    public States state;

    //Consts
    //Mislim da ih nece biti... Videcemo..

    //Callbacks
    public override void OnStartServer()
    {
        instance = this;

        commonRouteInstance = Instantiate(commonRoutePrefab);
        NetworkServer.Spawn(commonRouteInstance);

        moveCompleted = true;
        playerReady = new Dictionary<int, bool>();

    }

    //[Server]
    private void Update()
    {
        if (!isServer) { return; }
        if (pawnsSpawned == false)
        {
            Debug.Log(playerList.Count);
            int randomPlayer = Random.Range(0, playerList.Count);
            activePlayer = randomPlayer;
            playerList[activePlayer].hasTurn = true;
            ActivateRollButton(playerList[activePlayer], true);
            foreach (var player in playerDictionary)
            {
                AddPawnsToPlayer(player.Value);
            }
            pawnsSpawned = true;
        }
    }


    //Server side methods
    [Server]
    private List<PawnLTA> CreatePawns(GameObject routePrefab, GameObject basePrefab, int rotation, GameObject pawn, NetworkConnectionToClient sender = null)
    {
        // Assign the values (owner side only)
        //playerPawns = new Dictionary<int, PawnLTA[]>(4);
        //playerCompleted = new Dictionary<int, bool>(4);
        playerDictionary = new Dictionary<int, PlayerEntityLTA>(playerList.Count);

        var finalRoute = Instantiate(routePrefab).GetComponent<CommonRouteLTA>();
        var finalBase = Instantiate(basePrefab);

        Quaternion baseRotation = Quaternion.Euler(0, rotation, 0); //sets spawn rotation

        List<PawnLTA> pawns = new List<PawnLTA>(new PawnLTA[4]);

        for (int i = 0; i < pawns.Count; i++)
        {
            pawns[i] = Instantiate(pawn.GetComponent<PawnLTA>(), new Vector3(finalBase.transform.GetChild(i).transform.position.x, 0,
                    finalBase.transform.GetChild(i).transform.position.z),
                    baseRotation);
            pawns[i].baseNode = finalBase.transform.GetChild(i).GetComponent<NodeLTA>();
            pawns[i].transform.position = finalBase.transform.GetChild(i).GetComponent<NodeLTA>().transform.position;

            pawns[i].commonRoute = commonRouteInstance.GetComponent<CommonRouteLTA>();
            pawns[i].finalRoute = finalRoute;

            pawns[i].startNode = commonRouteInstance.transform.GetChild(i).GetComponent<NodeLTA>();
            pawns[i].selector = Instantiate(pawns[i].selector, new Vector3(pawns[i].transform.position.x, pawns[i].transform.position.y, pawns[i].transform.position.z), Quaternion.identity);
            pawns[i].selector.SetActive(false);
            pawns[i].BaseRotation = baseRotation;
            pawns[i].PawnId = i;

            //pawns[i].glowShader = null;
            pawns[i].Init();

            //Spawn pawn on clients
            NetworkServer.Spawn(pawns[i].gameObject, sender);
            NetworkServer.Spawn(pawns[i].selector, sender);

            //playerDictionary[netConn.Key].allPawns.AddRange(pawns);
            //playerPawns.Add(sender.connectionId, pawns);
            //playerCompleted.Add(sender.connectionId, false);
            //playerList[playerIndex].allPawns.AddRange(playerPawns[netConn.Key]);

        }

        return pawns;
    }
    [Server]
    private void AddPawnsToPlayer(PlayerEntityLTA player)
    {

        switch (player.playerColors)
        {
            case PlayerEntityLTA.PlayerColors.BLUE:

                player.allPawns.AddRange(CreatePawns(redRoutePrefab, redBasePrefab, 90, testPawnPrefab,player.netIdentity.connectionToClient)); //Spawn and adds pawns to player
                //player.powerButtonState = false;
                Debug.Log("Blue pawn instantiated!");
                break;
            case PlayerEntityLTA.PlayerColors.RED:
                Debug.Log("Red pawn instantiated!");
                player.allPawns.AddRange(CreatePawns(redRoutePrefab, redBasePrefab, 90, testPawnPrefab, player.netIdentity.connectionToClient));
                //player.powerButtonState = false;
                break;
            case PlayerEntityLTA.PlayerColors.YELLOW:
                Debug.Log("Yellow pawn instantiated!");
                player.allPawns.AddRange(CreatePawns(blueRoutePrefab, blueBasePrefab, 90, testPawnPrefab, player.netIdentity.connectionToClient));
                //player.powerButtonState = false;
                break;
            case PlayerEntityLTA.PlayerColors.GREEN:
                Debug.Log("Green pawn instantiated!");
                player.allPawns.AddRange(CreatePawns(redRoutePrefab, redBasePrefab, 90, testPawnPrefab, player.netIdentity.connectionToClient));
                //player.powerButtonState = false;
                break;
        }

    }

    private void ActivateRollButton(PlayerEntityLTA player, bool buttonState)
    {
        player.rollButtonState = buttonState;

    }
    private void ActivatePowerButton(PlayerEntityLTA player, bool buttonState)
    {
        player.powerButtonState = buttonState;

    }

    private void OnStateChanged(States oldState, States newState)
    {
        if (!isServer) { return; }
        if (playerList[activePlayer].playerTypes == PlayerEntityLTA.PlayerTypes.HUMAN)
        {
            switch (state)
            {
                case States.ROLL_DICE:
                    if (turnPossible)
                    {
                        //DEACTIVATE HIGHLIGHTS
                        ActivateRollButton(playerList[activePlayer], true);
                        ActivatePowerButton(playerList[activePlayer], false);
                        state = States.WAITING;
                    }
                    break;
                case States.WAITING:
                    //IDLE
                    break;
                case States.ATTACK:
                    if (turnPossible)
                    {
                        ActivateRollButton(playerList[activePlayer], false);
                        ActivatePowerButton(playerList[activePlayer], true);
                        //StartCoroutine(WaitForAttack());

                        state = States.WAITING;
                    }
                    break;
                case States.SWITCH_PLAYER:
                    if (turnPossible)
                    {
                        ActivateRollButton(playerList[activePlayer], false);
                        ActivatePowerButton(playerList[activePlayer], false);
                        playerList[activePlayer].hasTurn = false;
                        for (int i = 0; i < playerList[activePlayer].allPawns.Count; i++)
                        {
                            var activePawn = playerList[activePlayer].allPawns[i];
                            activePawn.isSelected = false;
                        }
                        StartCoroutine(SwitchPlayer());
                        state = States.WAITING;
                    }
                    break;
            }
        }
    }
    IEnumerator SwitchPlayer()
    {
        if (switchingPlayer)
        {
            yield break;
        }
        switchingPlayer = true;

        yield return new WaitForSeconds(1);


        //SET NEXT PLAYER
        SetNextActivePlayer();

        switchingPlayer = false;
    }

    void SetNextActivePlayer()
    {
        activePlayer++;
        activePlayer %= playerList.Count;
        playerList[activePlayer].hasTurn = true;

        int available = 0;
        for (int i = 0; i < playerList.Count; i++)
        {
            if (!playerList[i].hasWon)
            {
                available++;
            }
        }

        if (playerList[activePlayer].hasWon && available > 1)
        {
            SetNextActivePlayer();
            return;
        }
        else if (available < 2)
        {
            //GAME OVER SCREEN

            //SwitchScene("EndScene");
            state = States.WAITING;
            return;
        }

        //UpdateDiceBackground();

        //InfoText.instance.ShowMessage(playerList[activePlayer].playerName+ " has turn!");
        //DiceBackgoundSwitcher.instance.ChangeBackgroundImage(1);
        state = States.ROLL_DICE;
    }

    public void ReportTurnPossible(bool possible)
    {
        turnPossible = possible;
    }
    public void ReturnRandomDiceNumber()
    {
        rolledHumanDice = Mathf.RoundToInt(Random.Range(0, 6));
    }


    public void HumanRollDice(int diceNumber)
    {
        rolledHumanDice = diceNumber;
        Debug.Log($"Human Roll Dice {rolledHumanDice}");

        //MOVABLE PAWN LIST
        List<PawnLTA> movablePawns = new List<PawnLTA>();

        //START NODE FULL CHECK
        //is start node occupied
        bool startNodeFull = false;
        for (int i = 0; i < playerList[activePlayer].allPawns.Count; i++)
        {
            if (playerList[activePlayer].allPawns[i].currentNode == playerList[activePlayer].allPawns[i].startNode)
            {
                startNodeFull = true;
                break; //we found a match
            }
        }

        //NUMBER <6
        if (rolledHumanDice < 6)
        {
            movablePawns.AddRange(PossiblePawns());

        }

        //NUMBER ==6 && !startnode
        if (rolledHumanDice == 6 && !startNodeFull)
        {
            Debug.Log("6");
            //INSIDE BASE CHECK
            for (int i = 0; i < playerList[activePlayer].allPawns.Count; i++)
            {
                if (!playerList[activePlayer].allPawns[i].ReturnIsOut())
                {
                    movablePawns.Add(playerList[activePlayer].allPawns[i]);
                }
            }
            //OUTSIDE CHECK
            movablePawns.AddRange(PossiblePawns());

            //NUMBER == 6 && startnode
        }
        else if (rolledHumanDice == 6 && startNodeFull)
        {

            movablePawns.AddRange(PossiblePawns());

        }

        //ACTIVATE ALL POSSIBLE SELECTORS
        if (movablePawns.Count > 0)
        {
            for (int i = 0; i < movablePawns.Count; i++)
            {
                movablePawns[i].SetSelector(true);
            }
        }
        else
        {
            state = States.SWITCH_PLAYER;
        }



    }

    List<PawnLTA> PossiblePawns()
    {

        List<PawnLTA> tempList = new List<PawnLTA>();

        for (int i = 0; i < playerList[activePlayer].allPawns.Count; i++)
        {
            //MAKE SURE HE IS OUT ALREADY
            if (playerList[activePlayer].allPawns[i].ReturnIsOut())
            {
                if (playerList[activePlayer].allPawns[i].CheckPossibleKick(playerList[activePlayer].allPawns[i].PawnId, rolledHumanDice))
                {
                    tempList.Add(playerList[activePlayer].allPawns[i]);
                    continue;
                }
                if (playerList[activePlayer].allPawns[i].CheckPossibleMove(rolledHumanDice))
                {
                    tempList.Add(playerList[activePlayer].allPawns[i]);
                }
            }
        }

        return tempList;
    }

    void CheckStartNode(int diceNumber)
    {
        //is start node occupied
        bool startNodeFull = false;
        for (int i = 0; i < playerList[activePlayer].allPawns.Count; i++)
        {
            if (playerList[activePlayer].allPawns[i].currentNode == playerList[activePlayer].allPawns[i].startNode)
            {
                startNodeFull = true;
                break; //we found a match
            }
        }
        if (startNodeFull)
        {//moving
            MoveAStone(diceNumber);
            Debug.Log("The start node is full!");
        }
        else
        { //Leave base
            for (int i = 0; i < playerList[activePlayer].allPawns.Count; i++)
            {
                if (!playerList[activePlayer].allPawns[i].ReturnIsOut())
                {

                    playerList[activePlayer].allPawns[i].LeaveBase();
                    state = States.WAITING;
                    return;
                }

            }
            MoveAStone(diceNumber);
        }
    }

    void MoveAStone(int diceNumber)
    {
        List<PawnLTA> movablePawns = new List<PawnLTA>();
        List<PawnLTA> moveKickPawns = new List<PawnLTA>();

        //FILL THE LISTS
        for (int i = 0; i < playerList[activePlayer].allPawns.Count; i++)
        {
            if (playerList[activePlayer].allPawns[i].ReturnIsOut())
            {
                //CHECK FOR POSSIBLE KICK
                if (playerList[activePlayer].allPawns[i].CheckPossibleKick(playerList[activePlayer].allPawns[i].PawnId, diceNumber))
                {
                    moveKickPawns.Add(playerList[activePlayer].allPawns[i]);
                    continue;
                }
                //CHECK FOR POSSIBLE MOVE
                if (playerList[activePlayer].allPawns[i].CheckPossibleMove(diceNumber))
                {
                    movablePawns.Add(playerList[activePlayer].allPawns[i]);

                }
            }
        }
        //PERFORM KICK IF POSSIBLE
        if (moveKickPawns.Count > 0)
        {
            int num = Random.Range(0, moveKickPawns.Count);
            moveKickPawns[num].StartTheMove(diceNumber);
            state = States.WAITING;
            return;
        }
        //PERFORM MOVE IF POSSIBLE
        if (movablePawns.Count > 0)
        {
            int num = Random.Range(0, movablePawns.Count);
            movablePawns[num].StartTheMove(diceNumber);

            if (playerList[activePlayer].playerTypes == PlayerEntityLTA.PlayerTypes.HUMAN)
            {
                state = States.ATTACK;
            }
            else
            {
                state = States.WAITING;
            }
            return;
        }
        //NONE IS POSSIBLE
        //SWITCH PLAYER

        state = States.SWITCH_PLAYER;
    }

    public void DeactivateAllSelectors()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            for (int j = 0; j < playerList[i].allPawns.Count; j++)
            {
                playerList[i].allPawns[j].SetSelector(false);

            }
        }
    }
    public void ReportWinning()
    {
        //SHOW UI
        playerList[activePlayer].hasWon = true;

        //SAVE WINNERS
        for (int i = 0; i < SaveSettings.winners.Length; i++)
        {
            if (SaveSettings.winners[i] == "")
            {
                SaveSettings.winners[i] = playerList[activePlayer].playerName;
                break;
            }
        }
    }
}
