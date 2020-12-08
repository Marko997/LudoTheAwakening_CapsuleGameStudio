using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Awake() {
        instance = this;
    }

    void Update() {

        if(playerList[activePlayer].playerTypes == Entity.PlayerTypes.BOT){
            switch(state){
            case States.ROLL_DICE:
                StartCoroutine(RollDiceDelay());
                state = States.WAITING;
            break;
            case States.WAITING:

            break;
            case States.SWITCH_PLAYER:

            break;
            } 
        }

        
    }

    void RollDice(){
        int diceNumber = Random.Range(1,7);

        if(diceNumber == 6){
            //check start node

        }
        if(diceNumber <6){
            //check for kick
        }
        Debug.Log(diceNumber+"sadsadsadsa");
    }

    IEnumerator RollDiceDelay(){
        yield return new WaitForSeconds(2);
        RollDice();
    }
    

}
