using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.SceneManagement;

public class NetworkGameManagerLTA : NetworkBehaviour
{
    //public int playerID;

    //private PawnCardManager cardManager;
    public static NetworkGameManagerLTA instance;
    
    public enum States
    {
        WAITING,
        ROLL_DICE,
        ATTACK,
        SWITCH_PLAYER
    }

    [SerializeField] private CommonRouteLTA commonRoutePrefab;

    public List<PlayerEntityLTA> playerList = new List<PlayerEntityLTA>();

    //Izbacio template jer ne koristimo scriptable objects
    public GameObject redRoutePrefab;
    public GameObject redBasePrefab;
    public GameObject testPawn;
    public GameObject testSelector;


    [Header("PLAYER INFO")]
    public int activePlayer;// mozda treba da bude syncvar
    public int numberOfPlayers;

    [SyncVar(hook = nameof(OnStateChanged))] //-- napraviti metod sta se desi kada se promeni state
    public States state;

    //[Header("Buttons")]
    //public Button rollButton;
    //public Button powerButton;

    [Header("Bools")]
    bool switchingPlayer;
    bool turnPossible = true;
    bool pawnsSpawned = false;

    [Header("DICE")]
    //public DiceRollerLTA diceRoller;
    [HideInInspector] public int rolledHumanDice;//mozda bolje private

    GameObject commonRouteInstance;

    private NetworkManagerLTA room;
    private NetworkManagerLTA Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerLTA;
        }
    }

    public override void OnStartServer()
    {
        instance = this;
        numberOfPlayers = NetworkManager.singleton.numPlayers; //setovanje broja igraca proveriti jel radi

        commonRouteInstance = Instantiate(commonRoutePrefab.gameObject);// vratio na gameObject jer nmg da spawnujem na serveru.GetComponent<CommonRouteLTA>();
        NetworkServer.Spawn(commonRouteInstance);

        //Players added in list in spawn system script

        //TO DO add BOTS, now only HUMANS can exists
    }

    [Server]
    private void Update()
    {
        //if (!isServer) { return; }
        if (pawnsSpawned == false)
        {
            int randomPlayer = Random.Range(0, playerList.Count);
            activePlayer = randomPlayer;
            Debug.Log(activePlayer);
            playerList[activePlayer].hasTurn = true;
            playerList[activePlayer].CheckForTurn();
            foreach (var player in playerList)
            {
                //UpdateDiceBackground();

                CreatePawns(player.playerColors, player);
            }
            pawnsSpawned = true;
        }

        //BOTS
        if (playerList[activePlayer].playerTypes == PlayerEntityLTA.PlayerTypes.BOT)
        {
            switch (state)
            {
                case States.ROLL_DICE:
                    if (turnPossible)
                    {
                        //StartCoroutine(RollDiceDelay());
                        state = States.WAITING;
                    }
                    break;
                case States.WAITING:
                    //IDLE
                    break;
                //TO DO BOTS ATTACKING
                //case States.ATTACK:
                //	if (turnPossible)
                //	{
                //		StartCoroutine(WaitForAttack());
                //		state = States.WAITING;
                //	}
                //	break;
                case States.SWITCH_PLAYER:
                    if (turnPossible)
                    {
                        //ActivatePowerButton(false);
                        //StartCoroutine(SwitchPlayer());
                        state = States.WAITING;
                    }
                    break;
            }
        }
        //HUMAN
        //OnStateChanged();

        //CHECK IF PAWN IS SWORDGIRL AND TURN POWER BUTTON ON
        for (int i = 0; i < playerList[activePlayer].allPawns.Count; i++)
        {
            if (playerList[activePlayer].allPawns[i].isSelected)
            {
                var activePawn = playerList[activePlayer].allPawns[i];
                //Debug.Log(activePawn);
                if (activePawn.spellType == PawnLTA.SpellType.SWORDGIRL)
                {
                    //ActivatePowerButton(true,playerList[activePlayer].powerButton);
                }
            }
        }

    }

    private void OnStateChanged(States oldState, States newState)
    {
        Debug.Log("OnStateChanged");
        if (playerList[activePlayer].playerTypes == PlayerEntityLTA.PlayerTypes.HUMAN)
        {
            switch (state)
            {

                case States.ROLL_DICE:
                    if (turnPossible)
                    {
                        //DEACTIVATE HIGHLIGHTS
                        //ActivateRollButton(true,playerList[activePlayer].rollButton);
                        //Debug.Log("Roll state");
                        playerList[activePlayer].RpcChangeButtons(true, false);
                        state = States.WAITING;

                    }
                    break;
                case States.WAITING:
                    //IDLE
                    break;
                case States.ATTACK:
                    if (turnPossible)
                    {
                        //powerButton.SetActive(true);
                        playerList[activePlayer].RpcChangeButtons(false, true);
                        StartCoroutine(WaitForAttack());

                        state = States.WAITING;
                    }
                    break;
                case States.SWITCH_PLAYER:
                    if (turnPossible)
                    {
                        Debug.Log("works");
                        //powerButton.SetActive(false);
                        playerList[activePlayer].RpcChangeButtons(false, false);
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

    private List<PawnLTA> PreparePawns(GameObject routePrefab, GameObject basePrefab, int rotation, GameObject pawn,NetworkConnectionToClient sender = null)
    {
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
            pawns[i].selector = Instantiate(testSelector ,new Vector3(pawns[i].transform.position.x, pawns[i].transform.position.y, pawns[i].transform.position.z), Quaternion.identity);
            pawns[i].BaseRotation = baseRotation;
            pawns[i].PawnId = i;
            //pawns[i].glowShader = null;
            pawns[i].Init();

            //Spawn pawn on clients
            NetworkServer.Spawn(pawns[i].gameObject,sender); //namestiti da pripada playeru
            //Add pawn to pawn list of each player

        }

        return pawns;

    }


    private void CreatePawns(PlayerEntityLTA.PlayerColors playerColor, PlayerEntityLTA player)
    {

        switch (playerColor)
        {
            case PlayerEntityLTA.PlayerColors.BLUE:

                player.allPawns.AddRange(PreparePawns(redRoutePrefab, redBasePrefab, 90, testPawn)); //Spawn and adds pawns to player

                Debug.Log("Blue pawn instantiated!");
                
                //var redPawn0 = Instantiate(SaveSettings.loadOutPawns[0].GetComponent<PawnLTA>(),

                //newPawn.selector = Instantiate(templates.yellowSelector, new Vector3(newPawn.transform.position.x, newPawn.transform.position.y, newPawn.transform.position.z), Quaternion.identity);
                //newPawn.selector.transform.parent = newPawn.transform;

                //newPawn.glowShader = Instantiate(pawnShader, new Vector3(newPawn.transform.position.x, newPawn.transform.position.y + 3, newPawn.transform.position.z - 0.3f), Quaternion.identity);
                //newPawn.glowShader.transform.parent = newPawn.transform;
 


                break;
            case PlayerEntityLTA.PlayerColors.RED:
                Debug.Log("Red pawn instantiated!");
                player.allPawns.AddRange(PreparePawns(redRoutePrefab, redBasePrefab, 90, testPawn));

                break;
            case PlayerEntityLTA.PlayerColors.YELLOW:
                Debug.Log("Yellow pawn instantiated!");
                player.allPawns.AddRange(PreparePawns(redRoutePrefab, redBasePrefab, 90, testPawn));

                break;
            case PlayerEntityLTA.PlayerColors.GREEN:
                Debug.Log("Green pawn instantiated!");
                player.allPawns.AddRange(PreparePawns(redRoutePrefab, redBasePrefab, 90, testPawn));

                break;
        }

    }


    IEnumerator WaitForAttack()
    {
        yield return new WaitForSeconds(5);
        state = States.SWITCH_PLAYER;
    }
    public void PlaySpell()
    {
        //displaySpellButton = false;
        //powerButton.SetActive(false);
        playerList[activePlayer].RpcChangeButtons(false, false);
        //ActivatePowerButton(false, playerList[activePlayer].powerButton);

        for (int i = 0; i < playerList[activePlayer].allPawns.Count; i++)
        {
            if (playerList[activePlayer].allPawns[i].isSelected)
            {
                var activePawn = playerList[activePlayer].allPawns[i];
                //Debug.Log(activePawn);

                //---------SPEARMAN----------//
                if (activePawn.spellType == PawnLTA.SpellType.SPEARMAN)
                {
                    activePawn.eatPower = 1;
                    activePawn.eatNode = activePawn.fullRoute[activePawn.routePosition + activePawn.eatPower];

                    if (activePawn.eatNode.isTaken)
                    {
                        if (activePawn.PawnId != activePawn.eatNode.pawn.PawnId)
                        {
                            //KICK THE OTHER STONE
                            activePawn.eatNode.pawn.ReturnToBase();
                        }
                    }
                }

                //-------------ARHCER----------//
                if (activePawn.spellType == PawnLTA.SpellType.ARCHER)
                {
                    activePawn.eatPower = 3;
                    activePawn.eatNode = activePawn.fullRoute[activePawn.routePosition + activePawn.eatPower];
                    if (activePawn.eatNode.isTaken)
                    {
                        if (activePawn.PawnId != activePawn.eatNode.pawn.PawnId)
                        {
                            //KICK THE OTHER STONE
                            activePawn.eatNode.pawn.ReturnToBase();
                        }

                    }

                }
                //-----------MACEBARER----------//
                if (activePawn.spellType == PawnLTA.SpellType.MACEBEARER)
                {
                    activePawn.eatPower = -1;
                    activePawn.eatNode = activePawn.fullRoute[activePawn.routePosition + activePawn.eatPower];
                    if (activePawn.eatNode.isTaken)
                    {
                        if (activePawn.PawnId != activePawn.eatNode.pawn.PawnId)
                        {
                            //KICK THE OTHER STONE
                            activePawn.eatNode.pawn.ReturnToBase();
                        }
                    }

                }
                //------------SWORDGIRL----------//
                if (activePawn.spellType == PawnLTA.SpellType.SWORDGIRL)
                {

                    for (int j = activePawn.fullRoute.IndexOf(activePawn.currentNode) + 1; j < activePawn.fullRoute.IndexOf(activePawn.currentNode) + playerList[activePlayer].diceRoller.currentVal; j++)
                    {
                        //List<NodeManager> eatNodes = new List<NodeManager>();
                        //eatNodes.Add(activePawn.fullRoute[j]);
                        activePawn.eatNode = activePawn.fullRoute[j];

                        if (activePawn.eatNode.isTaken)
                        {
                            if (activePawn.PawnId != activePawn.eatNode.pawn.PawnId)
                            {
                                //KICK THE OTHER STONE
                                activePawn.eatNode.pawn.ReturnToBase();
                            }
                            ActivatePowerButton(false,playerList[activePlayer].powerButton);
                        }

                        //foreach (NodeManager node in eatNodes){
                        //    if (node.isTaken)
                        //    {
                        //        if (activePawn.pawnId != activePawn.currentNode.pawn.pawnId)
                        //        {
                        //            //KICK THE OTHER STONE
                        //            node.pawn.ReturnToBase();
                        //        }
                        //    }
                        //}


                    }
                    ActivatePowerButton(false, playerList[activePlayer].powerButton);

                }
                //--------------SLINGSHOTMAN------------//
                if (activePawn.spellType == PawnLTA.SpellType.SLINGSHOOTMAN)
                {
                    activePawn.eatPower = 2;
                    activePawn.eatNode = activePawn.fullRoute[activePawn.routePosition + activePawn.eatPower];

                    if (activePawn.eatNode.isTaken)
                    {
                        if (activePawn.PawnId != activePawn.eatNode.pawn.PawnId)
                        {
                            //KICK THE OTHER STONE
                            activePawn.eatNode.pawn.ReturnToBase();
                        }
                    }

                }
                //-----------WIZARD-----------------//
                if (activePawn.spellType == PawnLTA.SpellType.WIZARD) { }
                {
                    //ActivatePowerButton(true);
                    //powerButton.SetActive(true);
                    //displaySpellButton = true;
                    activePawn.eatPower = 6;
                    activePawn.currentNode = activePawn.fullRoute[activePawn.routePosition + activePawn.eatPower];


                }

                activePawn.eatPower = 0;
                activePawn.isSelected = false;
                //ActivatePowerButton(false, playerList[activePlayer].powerButton);
                playerList[activePlayer].RpcChangeButtons(false, false);
                //powerButton.SetActive(false);
                activePawn.eatNode = null;
                activePawn = null;
            }
        }
    }


    void CPUDice()
    {
        //TO DO BOTS
        //diceRoller.Roll();
    }

    public void RollDice(int _diceNumber)
    {
        int diceNumber = _diceNumber;//Random.Range(1,7);
        //int diceNumber = 6;
        if (playerList[activePlayer].playerTypes == PlayerEntityLTA.PlayerTypes.BOT)
        {

            if (diceNumber == 6)
            {
                //check start node
                CheckStartNode(diceNumber);

            }
            if (diceNumber < 6)
            {
                //check for move
                MoveAStone(diceNumber);

            }
        }
        if (playerList[activePlayer].playerTypes == PlayerEntityLTA.PlayerTypes.HUMAN)
        {
            rolledHumanDice = _diceNumber;
            //HumanRollDice();
        }

        //InfoText.instance.ShowMessage(playerList[activePlayer].playerName + " has rolled "+ _diceNumber);

    }

    IEnumerator RollDiceDelay()
    {
        yield return new WaitForSeconds(2);
        CPUDice();
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
            //SceneManager.LoadScene("LoadingScene");
            //LevelLoaderManager.sceneToLoad= "EndScene";
            SwitchScene("EndScene");
            state = States.WAITING;
            return;
        }

        //UpdateDiceBackground();

        //InfoText.instance.ShowMessage(playerList[activePlayer].playerName+ " has turn!");
        //DiceBackgoundSwitcher.instance.ChangeBackgroundImage(1);
        state = States.ROLL_DICE;
    }

    private void UpdateDiceBackground()
    {
        if (playerList[activePlayer].playerColors == PlayerEntityLTA.PlayerColors.BLUE)
        {
            DiceBackgoundSwitcher.instance.ChangeBackgroundImage(0);
        }
        else if (playerList[activePlayer].playerColors == PlayerEntityLTA.PlayerColors.RED)
        {
            DiceBackgoundSwitcher.instance.ChangeBackgroundImage(2);
        }
        else if (playerList[activePlayer].playerColors == PlayerEntityLTA.PlayerColors.GREEN)
        {
            DiceBackgoundSwitcher.instance.ChangeBackgroundImage(1);
        }
        else //if (playerList[activePlayer].playerColors == Entity.PlayerColors.RED)
        {
            DiceBackgoundSwitcher.instance.ChangeBackgroundImage(3);
        }
    }


    public void ReportTurnPossible(bool possible)
    {
        turnPossible = possible;
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

    //---------------------HUMAN INPUT ------------------------//

    void ActivateRollButton(bool buttonOn, Button rollButton)
    {

        rollButton.interactable = buttonOn;
    }
    void ActivatePowerButton(bool buttonOn, Button powerButton)
    {

        powerButton.interactable = buttonOn;
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

    //ON ROLL DICE BUTTON
    public void HumanRollDice(int diceNumber)
    {
        rolledHumanDice = diceNumber;
        Debug.Log($"Human Roll Dice {rolledHumanDice}");

        //ROLL DICE
        //rolledHumanDice = Random.Range(1,7);
        //rolledHumanDice = 6;

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
            Debug.Log("Less than 6");
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
                    Debug.Log("Avilable pawns: " + movablePawns.Count);
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
            Debug.Log("Switch player");
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


    public void SwitchScene(string sceneToSwitch)
    {
        SceneManager.LoadScene("LoadingScene");
        LevelLoaderManager.sceneToLoad = sceneToSwitch;
    }


}



