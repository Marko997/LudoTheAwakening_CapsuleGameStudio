using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnManager : MonoBehaviour
{

    public CommonRouteManager currentRoute;

    int routePosition;

    public int steps;

    bool isMoving;

    public int diceValue;

    public DiceSide[] diceSides;

    public DiceButton button;

    public DiceController dice;

    private void Start() {
        button = FindObjectOfType<DiceButton>();
        dice = FindObjectOfType<DiceController>();
    }
 

    public void Update() {
        
        if(button.pressed && !isMoving){

            dice.RollDice();
            steps = SideValueCheck();
            
            Debug.Log(steps);

            if(dice.rb.IsSleeping() && !dice.hasLanded && dice.thrown){
                dice.hasLanded = true;
                dice.rb.useGravity = false;
                dice.SideValueCheck();
  

            }else if(dice.rb.IsSleeping() && dice.hasLanded && dice.diceValue ==0){
                dice.RollAgain();
            }

            if(routePosition+steps < currentRoute.childNodesList.Count){
                StartCoroutine(Move());
            }
            else{
                Debug.Log("Roled number is to high!");
            }
        }

        
        
    }

    IEnumerator Move(){
        if(isMoving){
            yield break;
        }
        isMoving = true;

        while(steps>0){
            Vector3 nextPos = currentRoute.childNodesList[routePosition+1].position;
            while(MoveToNextNode(nextPos)){
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
            steps--;
            routePosition++;
        }

        isMoving = false;

    }

    bool MoveToNextNode(Vector3 goal){
        return goal != (transform.position = Vector3.MoveTowards(transform.position,goal,2f*Time.deltaTime));
    }

    public int SideValueCheck(){
        diceValue = 0;
        foreach(DiceSide side in diceSides){
            if(side.OnGround()){
                diceValue = side.sideValue;
                
            }
        }
        return diceValue;
    }
}
