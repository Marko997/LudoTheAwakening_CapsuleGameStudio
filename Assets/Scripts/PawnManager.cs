using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnManager : MonoBehaviour
{

    public CommonRouteManager currentRoute;

    int routePosition;

    public int steps;

    bool isMoving;
 

    void Update() {
        if(Input.GetKeyDown(KeyCode.Space) && !isMoving){
            steps = DiceNumberText.diceNumber;
            Debug.Log(steps);

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
}
