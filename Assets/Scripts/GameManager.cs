using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public enum PlayerSelection
{
    Spearman = 0, Macebearer = 1, Swordgirl = 2, Archergirl = 3
}

public class GameManager : MonoBehaviour
{
    public PlayerSelection playerSelection;
    public int playerID;
    public GameObject emptyPawn;
    //public PawnManager activePawn;

    public TextMeshProUGUI redName;
    public TextMeshProUGUI blueName;
    public TextMeshProUGUI greenName;
    public TextMeshProUGUI yellowName;

    private PawnCardManager cardManager;
    public static GameManager instance;
    public enum States
    {
        WAITING,
        ROLL_DICE,
        ATTACK,
        SWITCH_PLAYER
    }
    CommonRouteManager commonRoute;

    public List<Entity> playerList = new List<Entity>();

    [Header("TEMPLATES")]
    public AllPawnsTemplates templates;
    public SelectionTemplate templateYellow;
    public SelectionTemplate templateBlue;
    public SelectionTemplate templateGreen;
    public SelectionTemplate templateRed;
    public EntityTemplate playerEntity;

    [Header("PLAYER INFO")]
    public int activePlayer;
    public int numberOfPlayers;
    public States state;

    [Header("BUTTONS")]
    public Button rollButton;
    public Button powerButton;

    [Header("BOOLS")]
    //[HideInInspector] public bool displaySpellButton;
    bool switchingPlayer;
    bool turnPossible = true;

    [Header("DICE")]
    public Roller dice;
    [HideInInspector] public int rolledHumanDice;

    void Awake()
	{
		instance = this;
        numberOfPlayers = SaveSettings.numberOfPlayers;
		commonRoute = Instantiate(templates.commonRoute).GetComponent<CommonRouteManager>();

        redName.text = SaveSettings.playerNames[0];
        greenName.text = SaveSettings.playerNames[1];
        blueName.text = SaveSettings.playerNames[2];
        yellowName.text = SaveSettings.playerNames[3];

        //INSERT DATA FROM STARTGAME SCENE
        InsertPlayerData();
    }

	private void InsertPlayerData()
	{
        if(numberOfPlayers == 2)
		{
            playerList.Add(Instantiate(playerEntity.player));
            playerList.Add(Instantiate(playerEntity.player));

            playerList[0].playerName = SaveSettings.playerNames[0];
            playerList[0].playerColors = Entity.PlayerColors.RED;
            playerList[1].playerName = SaveSettings.playerNames[1];
            playerList[1].playerColors = Entity.PlayerColors.BLUE;


            //playerList[0].playerTypes = Entity.PlayerTypes.SaveSettings.players[0];
            //playerList[1].playerTypes = Entity.PlayerTypes.BOT;
        }
        if (numberOfPlayers == 4)
		{
            playerList.Add(Instantiate(playerEntity.player));
            playerList.Add(Instantiate(playerEntity.player));
            playerList.Add(Instantiate(playerEntity.player));
            playerList.Add(Instantiate(playerEntity.player));

            playerList[0].playerName = SaveSettings.playerNames[0];
            playerList[0].playerColors = Entity.PlayerColors.RED;
            playerList[1].playerName = SaveSettings.playerNames[1];
            playerList[1].playerColors = Entity.PlayerColors.BLUE;
            playerList[2].playerName = SaveSettings.playerNames[2];
            playerList[2].playerColors = Entity.PlayerColors.GREEN;
            playerList[3].playerName = SaveSettings.playerNames[3];
            playerList[3].playerColors = Entity.PlayerColors.YELLOW;

            //playerList[0].playerName = "Yellow";
            //playerList[1].playerName = "Red";
            //playerList[2].playerName = "Green";
            //playerList[3].playerName = "Blue";
        }


        for (int i = 0; i < playerList.Count; i++)
		{
            if (SaveSettings.players[i] == "HUMAN")
			{
                playerList[i].playerTypes = Entity.PlayerTypes.HUMAN;

            }
			if (SaveSettings.players[i] == "BOT")
			{
				playerList[i].playerTypes = Entity.PlayerTypes.BOT;
			}

			if (playerList[i].playerColors == Entity.PlayerColors.RED)
			{

				var finalRoute = Instantiate(templates.redRoute, Vector3.zero, Quaternion.Euler(0, 0, 0)).GetComponent<CommonRouteManager>();
                //var redBase = Instantiate(templates.redBase, Vector3.zero, Quaternion.identity);
                var redBase = Instantiate(templates.redBase);
                //var redPawn = SaveSettings.pawn.GetComponent<PawnManager>();
                var redPawn = SaveSettings.redPawns[0].GetComponent<PawnManager>();
                var redPawn2 = SaveSettings.redPawns[1].GetComponent<PawnManager>();
                var redPawn3 = SaveSettings.redPawns[2].GetComponent<PawnManager>();
                var redPawn4 = SaveSettings.redPawns[3].GetComponent<PawnManager>();

                Quaternion baseRotation = Quaternion.Euler(0, 90, 0);

                CreatePawns(i, finalRoute, redBase, 40, redPawn, redPawn2, redPawn3, redPawn4, 90,baseRotation,i);


                //CreatePawns(i, finalRoute, redBase, 40, redPawn,90,0);

                playerList[i].playerName = SaveSettings.playerNames[0];

			}
			if (playerList[i].playerColors == Entity.PlayerColors.GREEN)
			{
				var finalRoute = Instantiate(templates.greenRoute, Vector3.zero, Quaternion.Euler(0, 270, 0)).GetComponent<CommonRouteManager>();
                //var greenBase = Instantiate(templates.greenBase, Vector3.zero, Quaternion.identity);
                var greenBase = Instantiate(templates.greenBase);
                var greenPawn = SaveSettings.greenPawns[0].GetComponent<PawnManager>();
                var greenPawn2 = SaveSettings.greenPawns[1].GetComponent<PawnManager>();
                var greenPawn3 = SaveSettings.greenPawns[2].GetComponent<PawnManager>();
                var greenPawn4 = SaveSettings.greenPawns[3].GetComponent<PawnManager>();

                Quaternion baseRotation = Quaternion.Euler(0, 0, 0);

                CreatePawns(i, finalRoute, greenBase, 27, greenPawn, greenPawn2, greenPawn3, greenPawn4, 0,baseRotation,i);

                //CreatePawns(i, finalRoute, greenBase, 27, greenPawn,0,1);

                playerList[i].playerName = SaveSettings.playerNames[1];

			}
			if (playerList[i].playerColors == Entity.PlayerColors.BLUE)
			{
				var finalRoute = Instantiate(templates.blueRoute, Vector3.zero, Quaternion.Euler(0, 180, 0)).GetComponent<CommonRouteManager>();
				//var blueBase = Instantiate(templates.blueBase, Vector3.zero, Quaternion.identity);
				var blueBase = Instantiate(templates.blueBase);
                //var bluePawn = templateBlue.leaderPawn.GetComponent<PawnManager>();
                var bluePawn = SaveSettings.bluePawns[0].GetComponent<PawnManager>();
                var bluePawn2 = SaveSettings.bluePawns[1].GetComponent<PawnManager>();
                var bluePawn3 = SaveSettings.bluePawns[2].GetComponent<PawnManager>();
                var bluePawn4 = SaveSettings.bluePawns[3].GetComponent<PawnManager>();

                Quaternion baseRotation = Quaternion.Euler(0, 270, 0);

                CreatePawns(i, finalRoute, blueBase, 14, bluePawn2, bluePawn3, bluePawn4, bluePawn, 270,baseRotation,i);

                //CreatePawns(i, finalRoute, blueBase, 14, bluePawn,270,2);



                playerList[i].playerName = SaveSettings.playerNames[2];

			}
			if (playerList[i].playerColors == Entity.PlayerColors.YELLOW)
			{
				var finalRoute = Instantiate(templates.yellowRoute, Vector3.zero, Quaternion.Euler(0, 90, 0)).GetComponent<CommonRouteManager>();
				//var yellowBase = Instantiate(templates.yellowBase, Vector3.zero, Quaternion.identity);
				var yellowBase = Instantiate(templates.yellowBase);
				var yellowPawn = SaveSettings.yellowPawns[0].GetComponent<PawnManager>();
                var yellowPawn2 = SaveSettings.yellowPawns[1].GetComponent<PawnManager>();
                var yellowPawn3 = SaveSettings.yellowPawns[2].GetComponent<PawnManager>();
                var yellowPawn4 = SaveSettings.yellowPawns[3].GetComponent<PawnManager>();

                Quaternion baseRotation = Quaternion.Euler(0, 180, 0);

                CreatePawns(i, finalRoute, yellowBase, 1, yellowPawn, yellowPawn2, yellowPawn3, yellowPawn4,180,baseRotation,i);


                playerList[i].playerName = SaveSettings.playerNames[3];

            }
        }
	}

	private void CreatePawns(int i, CommonRouteManager finalRoute, GameObject newBase, int startNode, PawnManager pawn,PawnManager pawn2, PawnManager pawn3, PawnManager pawn4, int pawnRotation, Quaternion baseRotation, int pawnId)
	{
		for (int j = 0; j < playerList[i].allPawns.Length-3; j++)
		{
			var newPawn = Instantiate(pawn, new Vector3(newBase.transform.GetChild(0).transform.position.x, 0, newBase.transform.GetChild(0).transform.position.z), baseRotation).GetComponent<PawnManager>();
			var newPawn2 = Instantiate(pawn2, new Vector3(newBase.transform.GetChild(1).transform.position.x, 0, newBase.transform.GetChild(1).transform.position.z), baseRotation).GetComponent<PawnManager>();
			var newPawn3 = Instantiate(pawn3, new Vector3(newBase.transform.GetChild(2).transform.position.x, 0, newBase.transform.GetChild(2).transform.position.z), baseRotation).GetComponent<PawnManager>();
			var newPawn4 = Instantiate(pawn4, new Vector3(newBase.transform.GetChild(3).transform.position.x, 0, newBase.transform.GetChild(3).transform.position.z), baseRotation).GetComponent<PawnManager>();

            newPawn.baseNode = newBase.transform.GetChild(0).GetComponent<NodeManager>();
            newPawn2.baseNode = newBase.transform.GetChild(1).GetComponent<NodeManager>();
            newPawn3.baseNode = newBase.transform.GetChild(2).GetComponent<NodeManager>();
            newPawn4.baseNode = newBase.transform.GetChild(3).GetComponent<NodeManager>();

            CreatePawn(finalRoute, newBase, startNode, 0, j, newPawn,baseRotation,pawnId);
            CreatePawn(finalRoute, newBase, startNode, 1, j, newPawn2,baseRotation, pawnId);
            CreatePawn(finalRoute, newBase, startNode, 2, j, newPawn3, baseRotation, pawnId);
            CreatePawn(finalRoute, newBase, startNode, 3, j, newPawn4, baseRotation, pawnId);

            playerList[i].allPawns[0] = newPawn;
			playerList[i].allPawns[1] = newPawn2;
			playerList[i].allPawns[2] = newPawn3;
			playerList[i].allPawns[3] = newPawn4;
		}
	}

	private void CreatePawn(CommonRouteManager finalRoute, GameObject newBase, int startNode, int spellType, int j, PawnManager newPawn, Quaternion baseRotation, int pawnId)
	{
        
		newPawn.commonRoute = commonRoute;
		newPawn.finalRoute = finalRoute;
		
		newPawn.startNode = commonRoute.transform.GetChild(startNode).GetComponent<NodeManager>();
		newPawn.selector = Instantiate(templates.yellowSelector, new Vector3(newPawn.transform.position.x, 0.1f, newPawn.transform.position.z), Quaternion.identity);
		newPawn.selector.transform.parent = newPawn.transform;
        newPawn.baseRotation = baseRotation;
        newPawn.pawnId = pawnId;

		newPawn.Init();
	}

	void Start() {
        ActivateRollButton(false);
        ActivatePowerButton(false);
        //powerButton.SetActive(false);


        int randomPlayer = Random.Range(0, playerList.Count);
        activePlayer = randomPlayer;
        UpdateDiceBackground();
        //InfoText.instance.ShowMessage(playerList[activePlayer].playerName + " starts first!");
    }

    void Update()
    {
        //CPU
        if (playerList[activePlayer].playerTypes == Entity.PlayerTypes.BOT)
        {
            switch (state)
            {
                case States.ROLL_DICE:
                    if (turnPossible)
                    {
                        StartCoroutine(RollDiceDelay());
                        state = States.WAITING;
                    }
                    break;
                case States.WAITING:
                    //IDLE
                    break;
                //COMMENTED BECAUSE BOTS DONT SHOW BUTTONS
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
                        //powerButton.SetActive(false);
                        ActivatePowerButton(false);
                        StartCoroutine(SwitchPlayer());
                        state = States.WAITING;
                    }
                    break;
            }
        }
        //HUMAN
        if (playerList[activePlayer].playerTypes == Entity.PlayerTypes.HUMAN)
        {
            switch (state)
            {
                case States.ROLL_DICE:
                    if (turnPossible)
                    {
                        //DEACTIVATE HIGHLIGHTS
                        ActivateRollButton(true);
                        //try
                        //{
                        //    if (activePawn.spellType == PawnManager.SpellType.SWORDGIRL)
                        //    {
                        //        ActivatePowerButton(true);
                        //    }
                        //}
                        //catch (System.NullReferenceException)
                        //{

                        //}

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
                        ActivatePowerButton(true);
                        StartCoroutine(WaitForAttack());

                        state = States.WAITING;
                    }
                    break;
                case States.SWITCH_PLAYER:
                    if (turnPossible)
                    {
                        ActivatePowerButton(false);
                        //powerButton.SetActive(false);
                        for (int i = 0; i < playerList[activePlayer].allPawns.Length; i++)
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

        //CHECK IF PAWN IS SWORDGIRL AND TURN POWER BUTTON ON
        for (int i = 0; i < playerList[activePlayer].allPawns.Length; i++)
        {
            if (playerList[activePlayer].allPawns[i].isSelected)
            {
                var activePawn = playerList[activePlayer].allPawns[i];
                //Debug.Log(activePawn);
                if (activePawn.spellType == PawnManager.SpellType.SWORDGIRL)
                {
                    ActivatePowerButton(true);
                }
            }
            //if (playerList[activePlayer].allPawns[i].spellType == PawnManager.SpellType.SWORDGIRL)
            //{
            //    ActivatePowerButton(true);
            //}
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
                    
                    for (int j = activePawn.fullRoute.IndexOf(activePawn.currentNode)+1;  j < activePawn.fullRoute.IndexOf(activePawn.currentNode) + dice.DiceOneValue;  j++)
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


    void CPUDice(){
        dice.Roll();
    }

    public void RollDice(int _diceNumber){
        int diceNumber = _diceNumber;//Random.Range(1,7);
        //int diceNumber = 6;
        if(playerList[activePlayer].playerTypes == Entity.PlayerTypes.BOT){
            
            if(diceNumber == 6){
                //check start node
                CheckStartNode(diceNumber);

            }
            if(diceNumber <6){
                //check for move
                MoveAStone(diceNumber);

            }
        }
        if(playerList[activePlayer].playerTypes == Entity.PlayerTypes.HUMAN){
            rolledHumanDice = _diceNumber;
            HumanRollDice();
        }

        //InfoText.instance.ShowMessage(playerList[activePlayer].playerName + " has rolled "+ _diceNumber);

    }

    IEnumerator RollDiceDelay(){
        yield return new WaitForSeconds(2);
        CPUDice();
    }

    void CheckStartNode(int diceNumber){
        //is start node occupied
        bool startNodeFull = false;
        for(int i=0;i<playerList[activePlayer].allPawns.Length;i++){
            if(playerList[activePlayer].allPawns[i].currentNode == playerList[activePlayer].allPawns[i].startNode){
                startNodeFull = true;
                break; //we found a match
            }
        }
        if(startNodeFull){//moving
            MoveAStone(diceNumber);
            Debug.Log("The start node is full!");
        }else{ //Leave base
            for(int i = 0;i<playerList[activePlayer].allPawns.Length;i++){
                if(!playerList[activePlayer].allPawns[i].ReturnIsOut()){
    
                    playerList[activePlayer].allPawns[i].LeaveBase();
                    state = States.WAITING;
                    return;
                }
                
            }   
            MoveAStone(diceNumber); 
        }
    }

    void MoveAStone(int diceNumber){
        List <PawnManager> movablePawns = new List<PawnManager>();
        List <PawnManager> moveKickPawns = new List<PawnManager>();

        //FILL THE LISTS
        for(int i = 0;i<playerList[activePlayer].allPawns.Length;i++){
            if(playerList[activePlayer].allPawns[i].ReturnIsOut()){
                //CHECK FOR POSSIBLE KICK
                if(playerList[activePlayer].allPawns[i].CheckPossibleKick(playerList[activePlayer].allPawns[i].pawnId,diceNumber)){
                    moveKickPawns.Add(playerList[activePlayer].allPawns[i]);
                    continue;
                }
                //CHECK FOR POSSIBLE MOVE
                if(playerList[activePlayer].allPawns[i].CheckPossibleMove(diceNumber)){
                    movablePawns.Add(playerList[activePlayer].allPawns[i]);
                    
                }
            }
        }
        //PERFORM KICK IF POSSIBLE
        if(moveKickPawns.Count>0){
            int num = Random.Range(0,moveKickPawns.Count);
            moveKickPawns[num].StartTheMove(diceNumber);
            state = States.WAITING;
            return;
        }
        //PERFORM MOVE IF POSSIBLE
        if(movablePawns.Count>0){
            int num = Random.Range(0,movablePawns.Count);
            movablePawns[num].StartTheMove(diceNumber);

            if(playerList[activePlayer].playerTypes == Entity.PlayerTypes.HUMAN)
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

    IEnumerator SwitchPlayer(){
        if(switchingPlayer){
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
            EmptyAllTemplates();
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
        if (playerList[activePlayer].playerColors == Entity.PlayerColors.BLUE)
        {
            DiceBackgoundSwitcher.instance.ChangeBackgroundImage(0);
        }
        else if (playerList[activePlayer].playerColors == Entity.PlayerColors.RED)
        {
            DiceBackgoundSwitcher.instance.ChangeBackgroundImage(2);
        }
        else if (playerList[activePlayer].playerColors == Entity.PlayerColors.GREEN)
        {
            DiceBackgoundSwitcher.instance.ChangeBackgroundImage(1);
        }
        else //if (playerList[activePlayer].playerColors == Entity.PlayerColors.RED)
        {
            DiceBackgoundSwitcher.instance.ChangeBackgroundImage(3);
        }
    }


    public void ReportTurnPossible(bool possible){
        turnPossible = possible;
    }

    public void ReportWinning(){
        //SHOW UI
        playerList[activePlayer].hasWon = true;

		//SAVE WINNERS
		for (int i = 0; i < SaveSettings.winners.Length; i++)
		{
            if(SaveSettings.winners[i] == "")
			{
                SaveSettings.winners[i] = playerList[activePlayer].playerName;
                break;
			}
		}
    }

//---------------------HUMAN INPUT ------------------------//

    void ActivateRollButton(bool buttonOn){

        rollButton.interactable = buttonOn;
    }
    void ActivatePowerButton(bool buttonOn)
    {

        powerButton.interactable = buttonOn;
    }

    public void DeactivateAllSelectors(){
        for(int i = 0; i<playerList.Count;i++){
            for(int j =0;j<playerList[i].allPawns.Length;j++){
                playerList[i].allPawns[j].SetSelector(false);
                
            }
        }
    }
    

    public void HumanRoll(){
        
        dice.Roll();
        ActivateRollButton(false);

        
    }

    //ON ROLL DICE BUTTON
    public void HumanRollDice(){
        

        //ROLL DICE
        //rolledHumanDice = Random.Range(1,7);
        //rolledHumanDice = 6;

        //MOVABLE PAWN LIST
        List <PawnManager> movablePawns = new List<PawnManager>();

        //START NODE FULL CHECK
        //is start node occupied
        bool startNodeFull = false;
        for(int i=0;i<playerList[activePlayer].allPawns.Length;i++){
            if(playerList[activePlayer].allPawns[i].currentNode == playerList[activePlayer].allPawns[i].startNode){
                startNodeFull = true;
                break; //we found a match
            }
        }

        //NUMBER <6
        if(rolledHumanDice < 6){

            movablePawns.AddRange(PossiblePawns());

        }

        //NUMBER ==6 && !startnode
        if(rolledHumanDice == 6 && !startNodeFull){
            //INSIDE BASE CHECK
            for(int i = 0; i < playerList[activePlayer].allPawns.Length; i++){
                if(!playerList[activePlayer].allPawns[i].ReturnIsOut()){
                    movablePawns.Add(playerList[activePlayer].allPawns[i]);
                }
            }
            //OUTSIDE CHECK
            movablePawns.AddRange(PossiblePawns());
        
        //NUMBER == 6 && startnode
        }else if(rolledHumanDice == 6 && startNodeFull){

            movablePawns.AddRange(PossiblePawns());

        }

        //ACTIVATE ALL POSSIBLE SELECTORS
        if(movablePawns.Count > 0){
            for(int i=0; i< movablePawns.Count; i++){
                movablePawns[i].SetSelector(true);
            }
        }else{
            state = States.SWITCH_PLAYER;
        }
        

        
    }

    List <PawnManager> PossiblePawns(){

        List<PawnManager> tempList = new List<PawnManager>();
        
        for(int i=0; i < playerList[activePlayer].allPawns.Length; i++){
                //MAKE SURE HE IS OUT ALREADY
                if(playerList[activePlayer].allPawns[i].ReturnIsOut()){
                    if(playerList[activePlayer].allPawns[i].CheckPossibleKick(playerList[activePlayer].allPawns[i].pawnId,rolledHumanDice)){
                        tempList.Add(playerList[activePlayer].allPawns[i]);
                        continue;
                    }
                    if(playerList[activePlayer].allPawns[i].CheckPossibleMove(rolledHumanDice)){
                        tempList.Add(playerList[activePlayer].allPawns[i]);
                    }
                }
            }

        return tempList;
    }

    public void EmptyAllTemplates()
    {
        EmptyTemplate(templateRed);
        EmptyTemplate(templateBlue);
        EmptyTemplate(templateGreen);
        EmptyTemplate(templateYellow);
    }

    public void EmptyTemplate(SelectionTemplate template)
    {
        template.leaderPawn = emptyPawn;
        template.secondPawn = emptyPawn;
        template.thirdPawn = emptyPawn;
        template.fourthPawn = emptyPawn;
    }

    public void SwitchScene(string sceneToSwitch)
    {
        SceneManager.LoadScene("LoadingScene");
        LevelLoaderManager.sceneToLoad = sceneToSwitch;
    }

}




