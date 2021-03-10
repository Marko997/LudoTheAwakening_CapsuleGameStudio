using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<Entity> playerList = new List<Entity>();

    public enum States{
        WAITING,
        ROLL_DICE,
        SWITCH_PLAYER
    }

    public States state;

    public int activePlayer;

    bool switchingPlayer;
    bool turnPossible = true;

    public GameObject rollButton;
    //public GameObject powerButton;

    [HideInInspector]public int rolledHumanDice;

    public Roller dice;
    public PawnManager pawn;
    public CommonRouteManager commonRoute;

    void Awake() {
        instance = this;

		//ADDING PAWNS TO ENTITIY
		//for (int i = 0; i < playerList.Count; i++)
		//{
		//	playerList[i].allPawns[0] = Instantiate<RedPawn>;
		//}

		//INSERT DATA FROM STARTGAME SCENE
		for (int i = 0; i < playerList.Count; i++)
		{
            if(SaveSettings.players[i] == "HUMAN")
			{
                playerList[i].playerTypes = Entity.PlayerTypes.HUMAN;
			}
            if (SaveSettings.players[i] == "BOT")
			{
				playerList[i].playerTypes = Entity.PlayerTypes.BOT;
            }
            if (playerList[i].playerName == "Red") {

                playerList[i].playerName = SaveSettings.playerNames[0];

            }
            if (playerList[i].playerName == "Green")
            {

                playerList[i].playerName = SaveSettings.playerNames[1];

            }
            if (playerList[i].playerName == "Blue")
            {

                playerList[i].playerName = SaveSettings.playerNames[2];

            }
            if (playerList[i].playerName == "Yellow" && SaveSettings.yellowPawns[0] == "Pawn")
            {
                //PawnManager yellowPawn = pawn;
                                
                //playerList[i].allPawns[0] = Instantiate(yellowPawn, transform.position, transform.rotation);
                //playerList[i].allPawns[1] = Instantiate(yellowPawn, transform.position, transform.rotation);
                //playerList[i].allPawns[2] = Instantiate(yellowPawn, transform.position, transform.rotation);
                //playerList[i].allPawns[3] = Instantiate(yellowPawn, transform.position, transform.rotation);
                
                //playerList[i].allPawns[0].pawnId = 4;
                //playerList[i].allPawns[0].commonRoute = Instantiate(commonRoute,transform.position,transform.rotation);
               
                
                
                playerList[i].playerName = SaveSettings.playerNames[3];
                
            }
		}
    }

    void Start() {
        ActivateButton(false);

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
            case States.SWITCH_PLAYER:
                if(turnPossible){
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
            case States.SWITCH_PLAYER:
                if(turnPossible){
                    //DEACTIVATE BUTTON
                    
                    StartCoroutine(SwitchPlayer());
                    state = States.WAITING;
                }
                break;
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
            state = States.WAITING;
            return;
        }
        //NONE IS POSSIBLE
        //SWITCH PLAYER
        state = States.SWITCH_PLAYER;
        //Debug.Log("Should switch player!");
    }

    IEnumerator SwitchPlayer(){
        if(switchingPlayer){
            yield break;
        }
        switchingPlayer = true;

        yield return new WaitForSeconds(2);

        /*for(int i=0;i<playerList[activePlayer].allPawns.Length;i++){
                            
            playerList[activePlayer].allPawns[i].powerButton.SetActive(false);

        }*/

        //DEACTIVATE POWER BUTTONS LOGIC FOR LATER
        // for(int i=0;i<playerList[activePlayer].allPawns.Length;i++){
        //     if(playerList[activePlayer].allPawns[i].pawnName =="Spearman"){
                            
        //         playerList[activePlayer].allPawns[i].spearmanPowerButton.gameObject.SetActive(false);

        //         }else if(playerList[activePlayer].allPawns[i].pawnName =="Archer"){
        //             playerList[activePlayer].allPawns[i].archerPowerButton.SetActive(false);
                            
        //         }else if(playerList[activePlayer].allPawns[i].pawnName =="Swordgirl"){
        //             playerList[activePlayer].allPawns[i].swordgirlPowerButton.SetActive(false);
                            
        //         }else if(playerList[activePlayer].allPawns[i].pawnName =="Macebearer"){
        //             playerList[activePlayer].allPawns[i].macebearerPowerButton.SetActive(false);
                            
        //         }
        //     }
    

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




