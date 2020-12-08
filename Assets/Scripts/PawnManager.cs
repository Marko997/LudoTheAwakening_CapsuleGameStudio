using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnManager : MonoBehaviour
{
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
 

    public void Update() {
        
        if(button.pressed && !isMoving){

            steps = Random.Range(1,7);

            if(doneSteps + steps < fullRoute.Count){
                StartCoroutine(Move());
            }else{
                Debug.Log("Number is to high!");
            }
        }
    }


    IEnumerator Move(){
        if(isMoving){
            yield break;
        }
        isMoving = true;

        while(steps>0){
            routePosition++;

            Vector3 nextPos = fullRoute[routePosition].gameObject.transform.position;
            
            while(MoveToNextNode(nextPos,8f)){
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
            steps--;
            doneSteps++;
            
        }

        isMoving = false;

    }

    bool MoveToNextNode(Vector3 goal, float speed){
        return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, speed * Time.deltaTime));
    }

    // public int SideValueCheck(){
    //     diceValue = 0;
    //     foreach(DiceSide side in diceSides){
    //         if(side.OnGround()){
    //             diceValue = side.sideValue;
                
    //         }
    //     }
    //     return diceValue;
    // }
}
