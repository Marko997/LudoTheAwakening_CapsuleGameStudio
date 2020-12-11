using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnManager : MonoBehaviour
{
    public int pawnId;
    [Header("ROUTES")]   
    public CommonRouteManager commonRoute;
    public CommonRouteManager finalRoute;

    public List<NodeManager> fullRoute = new List<NodeManager>();

    [Header("NODES")]
    public NodeManager startNode;
    public NodeManager baseNode; //pawn house
    public NodeManager currentNode;
    public NodeManager goalNode;

    int routePosition;
    int startNodeIndex;

    int steps;
    int doneSteps;

    [Header("BOOLS")]
    public bool isOut;
    bool isMoving;
    bool hasTurn; // human input

    [Header("SELECTOR")]
    public GameObject selector;

    //ARC MOVEMENT
    float amplitude = 0.5f;
    float timeForPointToPoint = 0f;

    public DiceButton button;

    private void Start() {
        startNodeIndex = commonRoute.RequestPosition(startNode.gameObject.transform);
        CreateFullRoute();

    }

    void CreateFullRoute(){
        for(int i =0;i<commonRoute.childNodesList.Count-1;i++){
            int tempPosition = startNodeIndex +i;
            tempPosition %= commonRoute.childNodesList.Count;

            fullRoute.Add(commonRoute.childNodesList[tempPosition].GetComponent<NodeManager>());
        }

        for(int i =0;i<finalRoute.childNodesList.Count;i++){
        
            fullRoute.Add(finalRoute.childNodesList[i].GetComponent<NodeManager>());
        }
    }

    // public int steps;

    // public bool isMoving;

    // public int diceValue;

    // public DiceSide[] diceSides;

    

    // public DiceController dice;

    // private void Start() {
    //     button = FindObjectOfType<DiceButton>();
    //     dice = FindObjectOfType<DiceController>();
    // }

    IEnumerator Move(int diceNumber){
        if(isMoving){
            yield break;
        }
        isMoving = true;

        while(steps>0){
            routePosition++;

            Vector3 nextPos = fullRoute[routePosition].gameObject.transform.position;
            
            Vector3 startPos = fullRoute[routePosition-1].gameObject.transform.position;

            // while(MoveToNextNode(nextPos,8f)){
            //     yield return null;
            // }
            while(MoveInArcToNextNode(startPos,nextPos,8f)){
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
            timeForPointToPoint = 0;
            steps--;
            doneSteps++;
            
        }
        goalNode = fullRoute[routePosition];
        //CHECK POSSIBLE KICK
        if(goalNode.isTaken){
            //KICK THE OTHER STONE
            goalNode.pawn.ReturnToBase();
        }

        currentNode.pawn = null;
        currentNode.isTaken = false;

        goalNode.pawn = this;
        goalNode.isTaken = true;

        currentNode = goalNode;
        goalNode = null;

        //REPORT TO GAMEMANAGER
        //SWITCH THE PLAYER
        if(diceNumber<6){
            GameManager.instance.state = GameManager.States.SWITCH_PLAYER;
        }else{
            GameManager.instance.state = GameManager.States.ROLL_DICE;

        }

        isMoving = false;

    }

    bool MoveToNextNode(Vector3 goal, float speed){
        return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, speed * Time.deltaTime));
    }

    bool MoveInArcToNextNode(Vector3 startPos, Vector3 goalPosition, float speed){
        
        timeForPointToPoint += speed * Time.deltaTime;
        Vector3 pawnPosition = Vector3.Lerp(startPos, goalPosition,timeForPointToPoint);
        pawnPosition.y += amplitude * Mathf.Sin(Mathf.Clamp01(timeForPointToPoint)*Mathf.PI);

        return goalPosition != (transform.position = Vector3.Lerp(transform.position, pawnPosition, timeForPointToPoint));

    }

    public bool ReturnIsOut(){
        return isOut;
    }

    public void LeaveBase(){

        steps = 1;
        isOut = true;
        routePosition = 0;
        StartCoroutine(MoveOutFromBase());

    }

    IEnumerator MoveOutFromBase(){
        if(isMoving){
            yield break;
        }
        isMoving = true;

        while(steps>0){

            Vector3 nextPos = fullRoute[routePosition].gameObject.transform.position;
            
            // while(MoveToNextNode(nextPos,8f)){
            //     yield return null;
            // }

            Vector3 startPos = baseNode.gameObject.transform.position;
            while(MoveInArcToNextNode(startPos,nextPos,5f)){
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
            timeForPointToPoint = 0;
            steps--;
            doneSteps++;
            
        }
        //update node
        goalNode = fullRoute[routePosition];
        //check for eating pawn
        if(goalNode.isTaken){
            //return to base node
           goalNode.pawn.ReturnToBase();
        }

        goalNode.pawn = this;
        goalNode.isTaken = true;
        
        currentNode = goalNode;
        goalNode = null;

        //report back to gamemanager
        GameManager.instance.state = GameManager.States.ROLL_DICE;

        isMoving = false;

    }

    public bool CheckPossibleMove(int diceNumber){

        int tempPosition = routePosition + diceNumber;
        if(tempPosition >= fullRoute.Count){

            return false;

        }
        return !fullRoute[tempPosition].isTaken;
    }

    public bool CheckPossibleKick(int pawnID, int diceNumber){

        int tempPosition = routePosition + diceNumber;
        if(tempPosition >= fullRoute.Count){

            return false;

        }
        if(fullRoute[tempPosition].isTaken){
            
            if(pawnID == fullRoute[tempPosition].pawn.pawnId){
                return false;
            }
            return true;
        }
        return false;

    }

    public void StartTheMove(int diceNumber){
        
        steps = diceNumber;
        StartCoroutine(Move(diceNumber));
    }

    public void ReturnToBase(){

        StartCoroutine(ReturnEatenPawn());
    }

    IEnumerator ReturnEatenPawn(){

        GameManager.instance.ReportTurnPossible(false);
        routePosition = 0;
        currentNode = null;
        goalNode = null;
        isOut = false;
        doneSteps = 0;

        Vector3 baseNodePos = baseNode.gameObject.transform.position;
        while(MoveToNextNode(baseNodePos,100f)){
            yield return null;
        }
        GameManager.instance.ReportTurnPossible(true);
    }
}
