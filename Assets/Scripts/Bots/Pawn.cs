using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public enum PawnAnimationState
{
    Idle,
    Walk,
    Attack,
    Death,
    Jump
}

public class Pawn : MonoBehaviour
{
    public int pawnId;
    [Header("Routes")]
    public Route commonRoute;
    public Route finalRoute;

    public List<Node> fullRoute = new List<Node>();

    [Header("Nodes")]
    public Node startNode;
    public Node baseNode;
    public Node currentNode;
    public Node goalNode;

    public int routePosition;
    public int eatPower;
    int startNodeIndex;

    int steps;
    int doneSteps;
    public int lookTileInt = 0;

    [Header("Selector")]
    public GameObject selector;
    public Spell spell;

    [Header("Bools")]
    public bool isOut;
    bool isMoving;
    public bool isSelected;

    public bool hasTurn; //for human input

    PawnAnimationState animState;
    Animator animator;

    [Header("ARC MOVEMENT")]
//    float amplitude = 0.5f;
    float timeForPointToPoint = 0f;

    public bool ChangeIsSelected(bool state)
    {
        return isSelected = state;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        spell = gameObject.GetComponent<Spell>();
        startNodeIndex = commonRoute.RequestPosition(startNode.gameObject.transform);
        CreateFullRoute();
        selector = Instantiate(selector, this.transform);
        SetSelector(false);
    }

    private void Update()
    {
        ClientVisuals();
    }

    protected void CreateFullRoute()
    {
        for (int i = 0; i < commonRoute.childNodesList.Count; i++) //count-1
        {
            int tempPosition = startNodeIndex + i;
            tempPosition %= commonRoute.childNodesList.Count;

            fullRoute.Add(commonRoute.childNodesList[tempPosition].GetComponent<Node>());
        }

        for (int i = 0; i < finalRoute.childNodesList.Count; i++)
        {

            fullRoute.Add(finalRoute.childNodesList[i].GetComponent<Node>());
        }
    }

    IEnumerator Move(int diceNumber)
    {
        if (isMoving)
        {
            yield break;
        }
        isMoving = true;

        if (steps > 0)
        {
            ChangeAnimationState(PawnAnimationState.Walk);
        }

        if (routePosition == 50)//its not 50 check which number it should be
        {
            lookTileInt = 1;
        }
        else
        {
            lookTileInt = 0;
        }

        while (steps > 0)
        {
            routePosition++;

            Vector3 nextPos = fullRoute[routePosition].gameObject.transform.position;
            Vector3 startPos = fullRoute[routePosition - 1].gameObject.transform.position;
            Vector3 lookAtTile = fullRoute[routePosition + lookTileInt].transform.position;

            //while (MoveToNextNode(nextPos, 8f))
            //{
            //    yield return null;
            //}
            while (MoveInArcToNextNode(startPos, nextPos, 8f, lookAtTile))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
            //timeForPointToPoint = 0;
            steps--;
            doneSteps++;


        }

        goalNode = fullRoute[routePosition];
        //CHECK POSSIBLE KICK
        if (goalNode.isTaken)
        {
            //KICK THE OTHER STONE
            goalNode.pawn.ReturnToBase();
            BotGameManager.Instance.canRollAgain = true;
        }

        currentNode.pawn = null;
        currentNode.isTaken = false;

        goalNode.pawn = this;
        goalNode.isTaken = true;

        currentNode = goalNode;
        goalNode = null;

        //REPORT TO GAMEMANAGER

        //WIN CONDITION CHECK
        if (WinCondition())
        {
            BotGameManager.Instance.ReportWinning();
        }

        //SWITCH THE PLAYER
        if (diceNumber < 6)
        {
            if (BotGameManager.Instance.playerList[BotGameManager.Instance.activePlayer].playerType == BotPlayerTypes.HUMAN)
            {
                if((routePosition > 0 && routePosition < 49 - eatPower) && fullRoute[routePosition + eatPower].isTaken && pawnId != fullRoute[routePosition+eatPower].pawn.pawnId) //CHECK FOR ATTACK
                {
                    BotGameManager.Instance.state = States.ATTACK;
                }
                else if(!BotGameManager.Instance.canRollAgain) //CHECK FOR SWITCHING PLAYER
                {
                    BotGameManager.Instance.state = States.SWITCH_PLAYER;
                }
                else //ROLL DICE AGAIN
                {
                    BotGameManager.Instance.state = States.ROLL_DICE; 
                }
            }
            else //THIS IS FOR BOTS -- NEED TO UPDATE TO USE SPELL AND ROLL AGAIN!!!
            {
                BotGameManager.Instance.state = States.SWITCH_PLAYER;
            }
        }
        else
        {
            BotGameManager.Instance.state = States.ROLL_DICE;

        }
        ChangeAnimationState(PawnAnimationState.Idle);
        isMoving = false;
    }

    bool MoveToNextNode(Vector3 goal, float speed)
    {
        return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, speed * Time.deltaTime));
    }

    bool MoveInArcToNextNode(Vector3 startPos, Vector3 goalPosition, float speed, Vector3 lookAtTile)
    {

        timeForPointToPoint += 5f * Time.deltaTime;
        Vector3 pawnPosition = Vector3.Lerp(startPos, goalPosition, timeForPointToPoint);

        RotatePawn(lookAtTile);

        pawnPosition.y += 0.5f * Mathf.Sin(Mathf.Clamp01(timeForPointToPoint) * Mathf.PI);

        return goalPosition != (transform.position = Vector3.Lerp(transform.position, pawnPosition, timeForPointToPoint));

    }

    public bool ReturnIsOut()
    {
        return isOut;
    }

    public void LeaveBase()
    {
        steps = 1;
        isOut = true;
        routePosition = 0;
        StartCoroutine(MoveOutFromBase());
    }

    IEnumerator MoveOutFromBase()
    {
        if (isMoving)
        {
            yield break;
        }
        isMoving = true;

        while (steps > 0)
        {

            Vector3 nextPos = fullRoute[routePosition].gameObject.transform.position;
            Vector3 startPos = baseNode.gameObject.transform.position;
            Vector3 lookAtTile = fullRoute[routePosition + 1].transform.position;

            while (MoveInArcToNextNode(startPos, nextPos, 5f, lookAtTile))
            {
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
        if (goalNode.isTaken)
        {
            //return to base node
            goalNode.pawn.ReturnToBase();
        }

        goalNode.pawn = this;
        goalNode.isTaken = true;

        currentNode = goalNode;
        goalNode = null;

        //report back to gamemanager
        BotGameManager.Instance.state = States.ROLL_DICE;

        ChangeAnimationState(PawnAnimationState.Idle);

        isMoving = false;


    }

    public void RotatePawn(Vector3 lookAtTile)
    {
        float directionToFace = Vector3.Dot(transform.up, lookAtTile - transform.position);
        Vector3 point = lookAtTile - (transform.up * directionToFace);
        transform.LookAt(point, transform.up);
    }

    public bool CheckPossibleMove(int diceNumber)
    {
        int tempPosition = routePosition + diceNumber;
        if (tempPosition >= fullRoute.Count)
        {
            return false;
        }
        return !fullRoute[tempPosition].isTaken;
    }

    public bool CheckPossibleKick(int pawnID, int diceNumber)
    {
        int tempPosition = routePosition + diceNumber;
        if (tempPosition >= fullRoute.Count)
        {

            return false;

        }
        if (fullRoute[tempPosition].isTaken)
        {

            if (pawnID == fullRoute[tempPosition].pawn.pawnId)
            {
                return false;
            }
            return true;
        }
        return false;
    }

    public void StartTheMove(int diceNumber)
    {

        steps = diceNumber;
        StartCoroutine(Move(diceNumber));
    }

    public void ReturnToBase()
    {

        StartCoroutine(ReturnEatenPawn());
    }

    IEnumerator ReturnEatenPawn()
    {

        BotGameManager.Instance.ReportTurnPossible(false);
        routePosition = 0;
        currentNode = null;
        goalNode = null;
        isOut = false;
        doneSteps = 0;
        //transform.rotation = baseRotation; DODAJ KASNIJE

        Vector3 baseNodePos = baseNode.gameObject.transform.position;
        while (MoveToNextNode(baseNodePos, 100f))
        {
            yield return null;
        }
        BotGameManager.Instance.ReportTurnPossible(true);
    }

    bool WinCondition()
    {
        for (int i = 0; i < finalRoute.childNodesList.Count; i++)
        {
            if (!finalRoute.childNodesList[i].GetComponent<Node>().isTaken)
            {
                return false;
            }
        }
        return true;
    }

    //-------------------------HUMAN INPUT--------------------//

    public void SetSelector(bool selectorState)
    {
        if (selectorState)
        {
            gameObject.layer = LayerMask.NameToLayer("ActivePiece");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Piece");
        }
        
        selector.SetActive(selectorState);
        //THIS IS FOR HAVING CLICK ABILITY
        hasTurn = selectorState;
    }

    private void ClientVisuals()
    {
        switch (animState)
        {
            case PawnAnimationState.Idle:
                animator.SetBool("isWalking", false);
                animator.SetBool("isDead", false);
                animator.ResetTrigger("attack");
                animator.ResetTrigger("jump");
                break;
            case PawnAnimationState.Walk:
                animator.SetBool("isWalking", true);
                break;
            case PawnAnimationState.Attack:
                //animator.SetBool("isAttacking", true);
                animator.SetTrigger("attack");
                break;
            case PawnAnimationState.Death:
                animator.SetBool("isDead", true);
                break;
            case PawnAnimationState.Jump:
                animator.SetTrigger("jump");
                break;
            default:
                break;
        }
    }

    public void ChangeAnimationState(PawnAnimationState state)
    {
        animState = state;
    }
}
