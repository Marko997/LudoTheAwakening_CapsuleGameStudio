using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<Entity> playerList = new List<Entity>();

    public AllPawnsTemplates templates;

    public SelectionTemplate templateYellow;
    public SelectionTemplate templateBlue;
    public SelectionTemplate templateGreen;
    public SelectionTemplate templateRed;

    public enum States{
        WAITING,
        ROLL_DICE,
        ATTACK,
        SWITCH_PLAYER
    }

    public States state;

    public int activePlayer;

    bool switchingPlayer;
    bool turnPossible = true;

    public GameObject rollButton;
    public GameObject powerButton;

    [HideInInspector]public int rolledHumanDice;

    public bool displaySpellButton;

    public Roller dice;
    public CommonRouteManager commonRoute;

    void Awake()
	{
		instance = this;

		commonRoute = Instantiate(templates.commonRoute, Vector3.zero, Quaternion.identity).GetComponent<CommonRouteManager>();

		//INSERT DATA FROM STARTGAME SCENE
		InsertPlayerData();

	}

	private void InsertPlayerData()
	{
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
			if (playerList[i].playerName == "Red")
			{

				var finalRoute = Instantiate(templates.redRoute, Vector3.zero, Quaternion.Euler(0, 0, 0)).GetComponent<CommonRouteManager>();
				var redBase = Instantiate(templates.redBase, Vector3.zero, Quaternion.identity);
                //var redPawn = SaveSettings.pawn.GetComponent<PawnManager>();
                var redPawn = templateRed.leaderPawn.GetComponent<PawnManager>();
                var redPawn2 = templateRed.secondPawn.GetComponent<PawnManager>();
                var redPawn3 = templateRed.thirdPawn.GetComponent<PawnManager>();
                var redPawn4 = templateRed.fourthPawn.GetComponent<PawnManager>();

                Quaternion baseRotation = Quaternion.Euler(0, 90, 0);

                CreatePawns(i, finalRoute, redBase, 40, redPawn, redPawn2, redPawn3, redPawn4, 90,baseRotation);


                //CreatePawns(i, finalRoute, redBase, 40, redPawn,90,0);

                //playerList[i].playerName = SaveSettings.playerNames[0];

			}
			if (playerList[i].playerName == "Green")
			{
				var finalRoute = Instantiate(templates.greenRoute, Vector3.zero, Quaternion.Euler(0, 270, 0)).GetComponent<CommonRouteManager>();
				var greenBase = Instantiate(templates.greenBase, Vector3.zero, Quaternion.identity);
				var greenPawn = templateGreen.leaderPawn.GetComponent<PawnManager>();
                var greenPawn2 = templateGreen.secondPawn.GetComponent<PawnManager>();
                var greenPawn3 = templateGreen.thirdPawn.GetComponent<PawnManager>();
                var greenPawn4 = templateGreen.fourthPawn.GetComponent<PawnManager>();

                Quaternion baseRotation = Quaternion.Euler(0, 0, 0);

                CreatePawns(i, finalRoute, greenBase, 27, greenPawn, greenPawn2, greenPawn3, greenPawn4, 0,baseRotation);

                //CreatePawns(i, finalRoute, greenBase, 27, greenPawn,0,1);

                //playerList[i].playerName = SaveSettings.playerNames[1];

			}
			if (playerList[i].playerName == "Blue")
			{
				var finalRoute = Instantiate(templates.blueRoute, Vector3.zero, Quaternion.Euler(0, 180, 0)).GetComponent<CommonRouteManager>();
				var blueBase = Instantiate(templates.blueBase, Vector3.zero, Quaternion.identity);
				var bluePawn = templateBlue.leaderPawn.GetComponent<PawnManager>();
                var bluePawn2 = templateBlue.secondPawn.GetComponent<PawnManager>();
                var bluePawn3 = templateBlue.thirdPawn.GetComponent<PawnManager>();
                var bluePawn4 = templateBlue.fourthPawn.GetComponent<PawnManager>();

                Quaternion baseRotation = Quaternion.Euler(0, 270, 0);

                CreatePawns(i, finalRoute, blueBase, 14, bluePawn2, bluePawn3, bluePawn4, bluePawn, 270,baseRotation);

                //CreatePawns(i, finalRoute, blueBase, 14, bluePawn,270,2);



                //playerList[i].playerName = SaveSettings.playerNames[2];

			}
			if (playerList[i].playerName == "Yellow")
			{
				var finalRoute = Instantiate(templates.yellowRoute, Vector3.zero, Quaternion.Euler(0, 90, 0)).GetComponent<CommonRouteManager>();
				var yellowBase = Instantiate(templates.yellowBase, Vector3.zero, Quaternion.identity);
				var yellowPawn = templateYellow.leaderPawn.GetComponent<PawnManager>();
                var yellowPawn2 = templateYellow.secondPawn.GetComponent<PawnManager>();
                var yellowPawn3 = templateYellow.thirdPawn.GetComponent<PawnManager>();
                var yellowPawn4 = templateYellow.fourthPawn.GetComponent<PawnManager>();

                Quaternion baseRotation = Quaternion.Euler(0, 180, 0);

                CreatePawns(i, finalRoute, yellowBase, 1, yellowPawn, yellowPawn2, yellowPawn3, yellowPawn4,180,baseRotation);


                //playerList[i].playerName = SaveSettings.playerNames[3];

            }
        }
	}

	private void CreatePawns(int i, CommonRouteManager finalRoute, GameObject newBase, int startNode, PawnManager pawn,PawnManager pawn2, PawnManager pawn3, PawnManager pawn4, int pawnRotation, Quaternion baseRotation)
	{
		for (int j = 0; j < playerList[i].allPawns.Length-3; j++)
		{
			var newPawn = Instantiate(pawn, new Vector3(newBase.transform.GetChild(0).transform.position.x, newBase.transform.GetChild(0).transform.position.y, newBase.transform.GetChild(0).transform.position.z), baseRotation).GetComponent<PawnManager>();
			var newPawn2 = Instantiate(pawn2, new Vector3(newBase.transform.GetChild(1).transform.position.x, 0, newBase.transform.GetChild(1).transform.position.z), baseRotation).GetComponent<PawnManager>();
			var newPawn3 = Instantiate(pawn3, new Vector3(newBase.transform.GetChild(2).transform.position.x, 0, newBase.transform.GetChild(2).transform.position.z), baseRotation).GetComponent<PawnManager>();
			var newPawn4 = Instantiate(pawn4, new Vector3(newBase.transform.GetChild(3).transform.position.x, 0, newBase.transform.GetChild(3).transform.position.z), baseRotation).GetComponent<PawnManager>();

            //newPawn.baseRotation = Quaternion.Euler(0, pawnRotation, 0);

			CreatePawn(finalRoute, newBase, startNode, 0, j, newPawn,baseRotation);
            CreatePawn(finalRoute, newBase, startNode, 1, j, newPawn2,baseRotation);
            CreatePawn(finalRoute, newBase, startNode, 2, j, newPawn3, baseRotation);
            CreatePawn(finalRoute, newBase, startNode, 3, j, newPawn4, baseRotation);

            playerList[i].allPawns[0] = newPawn;
			playerList[i].allPawns[1] = newPawn2;
			playerList[i].allPawns[2] = newPawn3;
			playerList[i].allPawns[3] = newPawn4;
		}
	}

	private void CreatePawn(CommonRouteManager finalRoute, GameObject newBase, int startNode, int spellType, int j, PawnManager newPawn, Quaternion baseRotation)
	{
		newPawn.commonRoute = commonRoute;
		newPawn.finalRoute = finalRoute;
		newPawn.baseNode = newBase.transform.GetChild(j).GetComponent<NodeManager>();
		newPawn.startNode = commonRoute.transform.GetChild(startNode).GetComponent<NodeManager>();
		newPawn.selector = Instantiate(templates.yellowSelector, new Vector3(newPawn.transform.position.x, 0.1f, newPawn.transform.position.z), Quaternion.identity);
		newPawn.selector.transform.parent = newPawn.transform;
        newPawn.baseRotation = baseRotation;

		//newPawn.spellType = (PawnManager.SpellType)spellType;

		newPawn.Init();
	}

	void Start() {
        ActivateButton(false);
        powerButton.SetActive(false);

        int randomPlayer = Random.Range(0, playerList.Count);
        activePlayer = randomPlayer;
        InfoText.instance.ShowMessage(playerList[activePlayer].playerName + " starts first!");
    }

    void Update() {
        //CPU
        if(playerList[activePlayer].playerTypes == Entity.PlayerTypes.BOT){
            switch(state){
            case States.ROLL_DICE:
                if(turnPossible){
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
                if(turnPossible){
                    powerButton.SetActive(false);
                        StartCoroutine(SwitchPlayer());
                    state = States.WAITING;
                }
                break;
            } 
        }
        //HUMAN
        if(playerList[activePlayer].playerTypes == Entity.PlayerTypes.HUMAN){
            switch(state){
            case States.ROLL_DICE:
                if(turnPossible){
                    //DEACTIVATE HIGHLIGHTS
                    ActivateButton(true);
                    state = States.WAITING;
                    
                }
            break;
            case States.WAITING:
                //IDLE
            break;
				case States.ATTACK:
					if (turnPossible)
					{
						powerButton.SetActive(true);
						StartCoroutine(WaitForAttack());
                        
						state = States.WAITING;
					}
					break;
				case States.SWITCH_PLAYER:
                if(turnPossible){
                    powerButton.SetActive(false);
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

        
    }

    IEnumerator WaitForAttack()
	{
        yield return new WaitForSeconds(5);
        state = States.SWITCH_PLAYER;
    }
    public void PlaySpell()
	{

        displaySpellButton = false;
        powerButton.SetActive(false);
        for (int i = 0; i < playerList[activePlayer].allPawns.Length; i++)
		{
            var activePawn = playerList[activePlayer].allPawns[i];
            //var eatNode = activePawn.fullRoute[activePawn.routePosition + activePawn.eatPower];
            

            //---------SPEARMAN----------//
            if (activePawn.isSelected && activePawn.spellType == PawnManager.SpellType.SPEARMAN) { }
			{
                activePawn.eatPower = 1;
                activePawn.eatNode = activePawn.fullRoute[activePawn.routePosition + activePawn.eatPower];
                if (activePawn.eatNode.isTaken)
                {
                    //KICK THE OTHER STONE
                    activePawn.eatNode.pawn.ReturnToBase();
                    //activePawn.goalNode = eatNode;
                    activePawn.isSelected = false;
                    powerButton.SetActive(false);
                    activePawn.eatNode = null;
                }

            }
            //-------------ARHCER----------//
            if (activePawn.isSelected && activePawn.spellType == PawnManager.SpellType.ARCHER) { }
            {
                activePawn.eatPower = 3;
                activePawn.eatNode = activePawn.fullRoute[activePawn.routePosition + activePawn.eatPower];
                if (activePawn.goalNode.isTaken)
                {
                    //KICK THE OTHER STONE
                    activePawn.eatNode.pawn.ReturnToBase();
                    //activePawn.goalNode = eatNode;
                    activePawn.isSelected = false;
                    powerButton.SetActive(false);
                    activePawn.eatNode = null;
                }

            }
            //-----------MACEBARER----------//
            if (activePawn.isSelected && activePawn.spellType == PawnManager.SpellType.MACEBEARER) { }
            {
                activePawn.eatPower = -1;
                activePawn.eatNode = activePawn.fullRoute[activePawn.routePosition + activePawn.eatPower];
                if (activePawn.goalNode.isTaken)
                {
                    //KICK THE OTHER STONE
                    activePawn.eatNode.pawn.ReturnToBase();
                    //activePawn.goalNode = eatNode;
                    activePawn.isSelected = false;
                    powerButton.SetActive(false);
                    activePawn.eatNode = null;
                }

            }
            //------------SWORDGIRL----------//
            if (activePawn.isSelected && activePawn.spellType == PawnManager.SpellType.SWORDGIRL) { }
            {
                powerButton.SetActive(true);
                displaySpellButton = true;
                activePawn.currentNode = activePawn.fullRoute[activePawn.routePosition];
                if (activePawn.currentNode.isTaken)
                {
                    //KICK THE OTHER STONE
                    activePawn.eatNode.pawn.ReturnToBase();
                    //activePawn.goalNode = eatNode;
                    activePawn.isSelected = false;
                    powerButton.SetActive(false);
                    activePawn.eatNode = null;
                }

            }
            //--------------SLINGSHOTMAN------------//
            if (activePawn.isSelected && activePawn.spellType == PawnManager.SpellType.SLINGSHOOTMAN) { }
            {
                activePawn.eatPower = 2;
                activePawn.goalNode = activePawn.fullRoute[activePawn.routePosition + activePawn.eatPower];
                if (activePawn.goalNode.isTaken)
                {
                    //KICK THE OTHER STONE
                    activePawn.goalNode.pawn.ReturnToBase();
                    //activePawn.goalNode = eatNode;
                    activePawn.isSelected = false;
                    powerButton.SetActive(false);
                    activePawn.eatNode = null;
                }

            }
            //-----------WIZARD-----------------//
            if (activePawn.isSelected && activePawn.spellType == PawnManager.SpellType.WIZARD) { }
            {
                powerButton.SetActive(true);
                displaySpellButton = true;
                activePawn.eatPower = 6;
                activePawn.currentNode = activePawn.fullRoute[activePawn.routePosition + activePawn.eatPower];
                

            }
            
        }
	}


    void CPUDice(){
        dice.Roll1();
    }

    public void RollDice(int _diceNumber){
        int diceNumber = _diceNumber;//Random.Range(1,7);

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

        InfoText.instance.ShowMessage(playerList[activePlayer].playerName + " has rolled "+ _diceNumber);
        //Debug.Log(diceNumber+"sadsadsadsa");
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
            state = States.ATTACK;
            //state = States.WAITING;
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

        yield return new WaitForSeconds(2);
    

        //SET NEXT PLAYER
        SetNextActivePlayer();

        switchingPlayer = false;
    }

    void SetNextActivePlayer(){
        activePlayer++;
        activePlayer %= playerList.Count;

        int available = 0;
        for(int i=0;i<playerList.Count;i++){
            if(!playerList[i].hasWon){
                available++;
            }
        }

        if(playerList[activePlayer].hasWon && available>1){
            SetNextActivePlayer();
            return;
        }
        else if(available<2){
            //GAME OVER SCREEN
            SceneManager.LoadScene("EndScene");
            state = States.WAITING;
            return;
        }

        InfoText.instance.ShowMessage(playerList[activePlayer].playerName+ " has turn!");
        state = States.ROLL_DICE;
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

    void ActivateButton(bool buttonOn){
        rollButton.SetActive(buttonOn);
    }

    public void DeactivateAllSelectors(){
        for(int i = 0; i<playerList.Count;i++){
            for(int j =0;j<playerList[i].allPawns.Length;j++){
                playerList[i].allPawns[j].SetSelector(false);
                
            }
        }
    }
    

    public void HumanRoll(){
        
        dice.Roll1();
        ActivateButton(false);

        
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




}




