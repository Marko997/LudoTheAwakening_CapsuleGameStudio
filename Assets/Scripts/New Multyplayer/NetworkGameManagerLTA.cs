using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.SceneManagement;

public class NetworkGameManagerLTA : NetworkBehaviour
{
    public int playerID;

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
    private GameObject redRoutePrefab;
    private GameObject redBasePrefab;


    [Header("PLAYER INFO")]
    public int activePlayer;// mozda treba da bude syncvar
    public int numberOfPlayers;
    public States state;

    [Header("Buttons")]
    public Button rollButton;
    public Button powerButton;

    [Header("Bools")]
    bool switchingPlayer;
    bool turnPossible = true;
    bool pawnsSpawned = false;

    [Header("DICE")]
    public Roller dice;
    [HideInInspector] public int rolledHumanDice;//mozda bolje private

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

        var commonRouteInstance = Instantiate(commonRoutePrefab);// vratio na gameObject jer nmg da spawnujem na serveru.GetComponent<CommonRouteLTA>();
        NetworkServer.Spawn(commonRouteInstance.gameObject);

        int randomPlayer = Random.Range(0, playerList.Count);
        activePlayer = randomPlayer;

        //Players added in list in spawn system script

        //TO DO add BOTS, now only HUMANS can exists
    }

    public override void OnStartClient()
    {

        CmdTurnOffButtons();

        UpdateDiceBackground();
    }
    //Cmd and Rpc for turning off buttons on clients on start
    #region Buttons
    [Command(requiresAuthority = false)]
    private void CmdTurnOffButtons()
    {
        RpcTurnOffButtons();
    }

    [ClientRpc]
    private void RpcTurnOffButtons()
    {
        Debug.Log("Buttons deactivated");
        ActivateRollButton(false);
        ActivatePowerButton(false);
    }
    #endregion 

    private void Update()
    {
        if (!isServer) { return; }
        //TO DO SPAWN PAWS
        if(pawnsSpawned == false)
        {
            foreach(var player in playerList)
            {
                //CreatePawn(player.playerColors);
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
        if (playerList[activePlayer].playerTypes == PlayerEntityLTA.PlayerTypes.HUMAN)
        {
            switch (state)
            {
                case States.ROLL_DICE:
                    if (turnPossible)
                    {
                        //DEACTIVATE HIGHLIGHTS
                        //ActivateRollButton(true);

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
                        //ActivatePowerButton(true);
                        //StartCoroutine(WaitForAttack());

                        state = States.WAITING;
                    }
                    break;
                case States.SWITCH_PLAYER:
                    if (turnPossible)
                    {
                        //ActivatePowerButton(false);
                        //powerButton.SetActive(false);
                        for (int i = 0; i < playerList[activePlayer].allPawns.Length; i++)
                        {
                            var activePawn = playerList[activePlayer].allPawns[i];
                            activePawn.isSelected = false;
                        }
                        //StartCoroutine(SwitchPlayer());
                        state = States.WAITING;
                    }
                    break;
            }
        }

        //CHECK IF PAWN IS SWORDGIRL AND TURN POWER BUTTON ON
        for (int i = 0; i < playerList[activePlayer].allPawns.Length; i++)
        {
            if (playerList[activePlayer].allPawns[i].isSelected)
            {
                var activePawn = playerList[activePlayer].allPawns[i];
                //Debug.Log(activePawn);
                if (activePawn.spellType == PawnManager.SpellType.SWORDGIRL)
                {
                    //ActivatePowerButton(true);
                }
            }
        }

    }

    private void CreatePawn(PlayerEntityLTA.PlayerColors playerColor)
    {

        switch (playerColor)
        {
            case PlayerEntityLTA.PlayerColors.BLUE:
                Debug.Log("Blue pawn instantiated!");
                var finalRoute = Instantiate(redRoutePrefab).GetComponent<CommonRouteLTA>();
                var redBase = Instantiate(redBasePrefab);

                Quaternion baseRotation = Quaternion.Euler(0, 90, 0);

                //set pawns TO DO
                var redPawn0 = Instantiate(SaveSettings.loadOutPawns[0].GetComponent<PawnManager>(),
                    new Vector3(redBase.transform.GetChild(0).transform.position.x, 0,
                    redBase.transform.GetChild(0).transform.position.z),
                    baseRotation); //change it once pawn script is transfered to mp

                var redPawn1 = Instantiate(SaveSettings.loadOutPawns[1].GetComponent<PawnManager>());
                var redPawn2 = Instantiate(SaveSettings.loadOutPawns[2].GetComponent<PawnManager>());
                var redPawn3 = Instantiate(SaveSettings.loadOutPawns[3].GetComponent<PawnManager>());

                //Create pawns DONE

                redPawn0.baseNode = redBase.transform.GetChild(0).GetComponent<NodeManager>(); 
                redPawn1.baseNode = redBase.transform.GetChild(0).GetComponent<NodeManager>(); 
                redPawn2.baseNode = redBase.transform.GetChild(0).GetComponent<NodeManager>(); 
                redPawn3.baseNode = redBase.transform.GetChild(0).GetComponent<NodeManager>();

                redPawn0.transform.position = redBase.transform.GetChild(0).GetComponent<NodeManager>().transform.position;
                redPawn1.transform.position = redBase.transform.GetChild(1).GetComponent<NodeManager>().transform.position;
                redPawn2.transform.position = redBase.transform.GetChild(2).GetComponent<NodeManager>().transform.position;
                redPawn3.transform.position = redBase.transform.GetChild(3).GetComponent<NodeManager>().transform.position;

                //Create pawn TO DO

                redPawn0.commonRoute = null;
                redPawn0.finalRoute = null;

                redPawn0.startNode = null;
                redPawn0.selector = null;
                redPawn0.baseRotation = Quaternion.identity;
                redPawn0.pawnId = 0;
                redPawn0.glowShader = null;
                redPawn0.Init();

                //newPawn.commonRoute = commonRoute;
                //newPawn.finalRoute = finalRoute;

                //newPawn.startNode = commonRoute.transform.GetChild(startNode).GetComponent<NodeManager>();
                //newPawn.selector = Instantiate(templates.yellowSelector, new Vector3(newPawn.transform.position.x, newPawn.transform.position.y, newPawn.transform.position.z), Quaternion.identity);
                //newPawn.selector.transform.parent = newPawn.transform;
                //newPawn.baseRotation = baseRotation;
                //newPawn.pawnId = pawnId;
                //newPawn.glowShader = Instantiate(pawnShader, new Vector3(newPawn.transform.position.x, newPawn.transform.position.y + 3, newPawn.transform.position.z - 0.3f), Quaternion.identity);
                //newPawn.glowShader.transform.parent = newPawn.transform;
                //newPawn.Init();


                break;
            case PlayerEntityLTA.PlayerColors.RED:
                Debug.Log("Red pawn instantiated!");


                break;
            case PlayerEntityLTA.PlayerColors.YELLOW:
                Debug.Log("Yellow pawn instantiated!");


                break;
            case PlayerEntityLTA.PlayerColors.GREEN:
                Debug.Log("Green pawn instantiated!");


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
        ActivatePowerButton(false);

        for (int i = 0; i < playerList[activePlayer].allPawns.Length; i++)
        {
            if (playerList[activePlayer].allPawns[i].isSelected)
            {
                var activePawn = playerList[activePlayer].allPawns[i];
                //Debug.Log(activePawn);

                //---------SPEARMAN----------//
                if (activePawn.spellType == PawnManager.SpellType.SPEARMAN)
                {
                    activePawn.eatPower = 1;
                    activePawn.eatNode = activePawn.fullRoute[activePawn.routePosition + activePawn.eatPower];

                    if (activePawn.eatNode.isTaken)
                    {
                        if (activePawn.pawnId != activePawn.eatNode.pawn.pawnId)
                        {
                            //KICK THE OTHER STONE
                            activePawn.eatNode.pawn.ReturnToBase();
                        }
                    }
                }

                //-------------ARHCER----------//
                if (activePawn.spellType == PawnManager.SpellType.ARCHER)
                {
                    activePawn.eatPower = 3;
                    activePawn.eatNode = activePawn.fullRoute[activePawn.routePosition + activePawn.eatPower];
                    if (activePawn.eatNode.isTaken)
                    {
                        if (activePawn.pawnId != activePawn.eatNode.pawn.pawnId)
                        {
                            //KICK THE OTHER STONE
                            activePawn.eatNode.pawn.ReturnToBase();
                        }

                    }

                }
                //-----------MACEBARER----------//
                if (activePawn.spellType == PawnManager.SpellType.MACEBEARER)
                {
                    activePawn.eatPower = -1;
                    activePawn.eatNode = activePawn.fullRoute[activePawn.routePosition + activePawn.eatPower];
                    if (activePawn.eatNode.isTaken)
                    {
                        if (activePawn.pawnId != activePawn.eatNode.pawn.pawnId)
                        {
                            //KICK THE OTHER STONE
                            activePawn.eatNode.pawn.ReturnToBase();
                        }
                    }

                }
                //------------SWORDGIRL----------//
                if (activePawn.spellType == PawnManager.SpellType.SWORDGIRL)
                {

                    for (int j = activePawn.fullRoute.IndexOf(activePawn.currentNode) + 1; j < activePawn.fullRoute.IndexOf(activePawn.currentNode) + dice.DiceOneValue; j++)
                    {
                        //List<NodeManager> eatNodes = new List<NodeManager>();
                        //eatNodes.Add(activePawn.fullRoute[j]);
                        activePawn.eatNode = activePawn.fullRoute[j];

                        if (activePawn.eatNode.isTaken)
                        {
                            if (activePawn.pawnId != activePawn.eatNode.pawn.pawnId)
                            {
                                //KICK THE OTHER STONE
                                activePawn.eatNode.pawn.ReturnToBase();
                            }
                            ActivatePowerButton(false);
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
                    ActivatePowerButton(false);

                }
                //--------------SLINGSHOTMAN------------//
                if (activePawn.spellType == PawnManager.SpellType.SLINGSHOOTMAN)
                {
                    activePawn.eatPower = 2;
                    activePawn.eatNode = activePawn.fullRoute[activePawn.routePosition + activePawn.eatPower];

                    if (activePawn.eatNode.isTaken)
                    {
                        if (activePawn.pawnId != activePawn.eatNode.pawn.pawnId)
                        {
                            //KICK THE OTHER STONE
                            activePawn.eatNode.pawn.ReturnToBase();
                        }
                    }

                }
                //-----------WIZARD-----------------//
                if (activePawn.spellType == PawnManager.SpellType.WIZARD) { }
                {
                    //ActivatePowerButton(true);
                    //powerButton.SetActive(true);
                    //displaySpellButton = true;
                    activePawn.eatPower = 6;
                    activePawn.currentNode = activePawn.fullRoute[activePawn.routePosition + activePawn.eatPower];


                }

                activePawn.eatPower = 0;
                activePawn.isSelected = false;
                ActivatePowerButton(false);
                //powerButton.SetActive(false);
                activePawn.eatNode = null;
                activePawn = null;
            }
        }
    }


    void CPUDice()
    {
        dice.Roll();
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
            HumanRollDice();
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
        for (int i = 0; i < playerList[activePlayer].allPawns.Length; i++)
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
            for (int i = 0; i < playerList[activePlayer].allPawns.Length; i++)
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
        List<PawnManager> movablePawns = new List<PawnManager>();
        List<PawnManager> moveKickPawns = new List<PawnManager>();

        //FILL THE LISTS
        for (int i = 0; i < playerList[activePlayer].allPawns.Length; i++)
        {
            if (playerList[activePlayer].allPawns[i].ReturnIsOut())
            {
                //CHECK FOR POSSIBLE KICK
                if (playerList[activePlayer].allPawns[i].CheckPossibleKick(playerList[activePlayer].allPawns[i].pawnId, diceNumber))
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

        UpdateDiceBackground();

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

    void ActivateRollButton(bool buttonOn)
    {

        rollButton.interactable = buttonOn;
    }
    void ActivatePowerButton(bool buttonOn)
    {

        powerButton.interactable = buttonOn;
    }

    public void DeactivateAllSelectors()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            for (int j = 0; j < playerList[i].allPawns.Length; j++)
            {
                playerList[i].allPawns[j].SetSelector(false);

            }
        }
    }


    public void HumanRoll()
    {

        dice.Roll();
        ActivateRollButton(false);


    }

    //ON ROLL DICE BUTTON
    public void HumanRollDice()
    {


        //ROLL DICE
        //rolledHumanDice = Random.Range(1,7);
        //rolledHumanDice = 6;

        //MOVABLE PAWN LIST
        List<PawnManager> movablePawns = new List<PawnManager>();

        //START NODE FULL CHECK
        //is start node occupied
        bool startNodeFull = false;
        for (int i = 0; i < playerList[activePlayer].allPawns.Length; i++)
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
            //INSIDE BASE CHECK
            for (int i = 0; i < playerList[activePlayer].allPawns.Length; i++)
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

    List<PawnManager> PossiblePawns()
    {

        List<PawnManager> tempList = new List<PawnManager>();

        for (int i = 0; i < playerList[activePlayer].allPawns.Length; i++)
        {
            //MAKE SURE HE IS OUT ALREADY
            if (playerList[activePlayer].allPawns[i].ReturnIsOut())
            {
                if (playerList[activePlayer].allPawns[i].CheckPossibleKick(playerList[activePlayer].allPawns[i].pawnId, rolledHumanDice))
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



