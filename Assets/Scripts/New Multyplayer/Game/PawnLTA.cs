using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PawnLTA : NetworkBehaviour
{
    [Header("PAWN INFO")]
    [SerializeField] private int pawnId;
    [SerializeField] private string pawnName;

    [SerializeField] public enum SpellType
    {
        SPEARMAN,
        ARCHER,
        MACEBEARER,
        SWORDGIRL,
        SLINGSHOOTMAN,
        WIZARD
    }

    public SpellType spellType;

    private Quaternion baseRotation;

    [Header("ROUTES")]
    public CommonRouteLTA commonRoute;
    public CommonRouteLTA finalRoute;
    public List<NodeLTA> fullRoute = new List<NodeLTA>();

    [Header("NODES")]
    public NodeLTA startNode;
    public NodeLTA baseNode;
    public NodeLTA currentNode;
    public NodeLTA goalNode;
    public NodeLTA eatNode;

    public int routePosition;
    protected int startNodeIndex;

    [Header("RUNTIME")]
    int steps;
    int doneSteps;
    public int eatPower;

    [Header("BOOLS")]
    public bool isOut;
    bool isMoving;
    protected bool hasTurn; //for human
    public bool isSelected = false;

    [Header("ARC MOVEMENT")]
    float amplitude = 0.5f;
    float timeForPointToPoint = 0f;

    [Header("VFX")]
    public GameObject selector;
    public GameObject glowShader;

    public int PawnId { get => pawnId; set => pawnId = value; }
    public string PawnName { get => PawnName; set => PawnName = value; }
    public Quaternion BaseRotation { get => baseRotation; set => baseRotation = value; }

    public void Init()
    {
        startNodeIndex = commonRoute.RequestPosition(startNode.gameObject.transform);
        CreateFullRoute();
    }

    protected void CreateFullRoute()
    {
        for(int i = 0; i < commonRoute.childNodesList.Count - 1; i++)
        {
            int tempPosition = startNodeIndex + i;
            tempPosition %= commonRoute.childNodesList.Count;

            fullRoute.Add(commonRoute.childNodesList[tempPosition].GetComponent<NodeLTA>());
        }

        for(int i = 0; i < finalRoute.childNodesList.Count; i++)
        {
            fullRoute.Add(finalRoute.childNodesList[i].GetComponent<NodeLTA>());
        }
    }

    IEnumerator Move(int diceNumber)
    {
        if (isMoving)
        {
            yield break;
        }
        isMoving = true;

        while(steps > 0)
        {
            routePosition++;

            Vector3 nextPos = fullRoute[routePosition].gameObject.transform.position;
            Vector3 startPos = fullRoute[routePosition - 1].gameObject.transform.position;

            //TO DO PAWN ROTATION
            //RotatePawnOnBoard();

            while (MoveInArcToNextNode(startPos, nextPos, 8f))
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.1f);
            timeForPointToPoint = 0;
            steps--;
            doneSteps++;
        }

        goalNode = fullRoute[routePosition];
        //Check for possible kick
        if (goalNode.isTaken)
        {
            goalNode.pawn.ReturnToBase();
        }

        currentNode.pawn = null;
        currentNode.isTaken = false;

        goalNode.pawn = this;
        goalNode.isTaken = true;

        currentNode = goalNode;
        goalNode = null;

        //Check for win
        if (WinCondition())
        {
            NetworkGameManagerLTA.instance.ReportWinning();
        }

        //Switch player
        if(diceNumber < 6)
        {
            if (NetworkGameManagerLTA.instance.playerList[NetworkGameManagerLTA.instance.activePlayer].playerTypes == PlayerEntityLTA.PlayerTypes.HUMAN)
            {
                NetworkGameManagerLTA.instance.state = NetworkGameManagerLTA.States.ATTACK;
            }
            else
            {
                NetworkGameManagerLTA.instance.state = NetworkGameManagerLTA.States.SWITCH_PLAYER;
            }
        }
        else
        {
            NetworkGameManagerLTA.instance.state = NetworkGameManagerLTA.States.ROLL_DICE;
        }

        isMoving = false;


    }

    //Pawn rotation method TO DO
    //RotatePawnOnBoard(){}

    bool MoveToNextNode(Vector3 goal, float speed)
    {
        return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, speed * Time.deltaTime));
    }

    bool MoveInArcToNextNode(Vector3 startPos, Vector3 goalPosition, float speed)
    {

        timeForPointToPoint += speed * Time.deltaTime;
        Vector3 pawnPosition = Vector3.Lerp(startPos, goalPosition, timeForPointToPoint);
        pawnPosition.y += amplitude * Mathf.Sin(Mathf.Clamp01(timeForPointToPoint) * Mathf.PI);

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

        while(steps > 0)
        {
            Vector3 nextPos = fullRoute[routePosition].gameObject.transform.position;
            Vector3 startPos = baseNode.gameObject.transform.position;
            while (MoveInArcToNextNode(startPos, nextPos, 5f))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
            timeForPointToPoint = 0;
            steps--;
            doneSteps++;
        }
        //Update node
        goalNode = fullRoute[routePosition];
        //Check for kick pawn
        if (goalNode.isTaken)
        {
            goalNode.pawn.ReturnToBase();
        }

        goalNode.pawn = this;
        goalNode.isTaken = true;

        currentNode = goalNode;
        goalNode = null;

        NetworkGameManagerLTA.instance.state = NetworkGameManagerLTA.States.ROLL_DICE;

        isMoving = false;
    }

    public bool CheckPossibleMove(int diceNumber)
    {
        int tempPosition = routePosition + diceNumber;
        if(tempPosition >= fullRoute.Count)
        {
            return false;
        }
        return !fullRoute[tempPosition].isTaken;
    }

    public bool CheckPossibleKick(int pawnID, int diceNumber)
    {
        int tempPosistion = routePosition + diceNumber;
        if(tempPosistion >= fullRoute.Count)
        {
            return false;
        }
        if (fullRoute[tempPosistion].isTaken)
        {
            if(pawnID == fullRoute[tempPosistion].pawn.pawnId)
            {
                return false;
            }
            return true;
        }
        return true;
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
        NetworkGameManagerLTA.instance.ReportTurnPossible(false);
        routePosition = 0;
        currentNode = null;
        goalNode = null;
        isOut = false;
        doneSteps = 0;
        transform.rotation = baseRotation;

        Vector3 baseNodePos = baseNode.gameObject.transform.position;
        while (MoveToNextNode(baseNodePos, 100f))
        {
            yield return null;
        }
        NetworkGameManagerLTA.instance.ReportTurnPossible(true);
    }

    bool WinCondition()
    {
        for (int i = 0; i < finalRoute.childNodesList.Count; i++)
        {
            if (!finalRoute.childNodesList[i].GetComponent<NodeLTA>().isTaken)
            {
                return false;
            }
        }
        return true;
    }

    //------------------HUMAN INPUT--------------------//

    public void SetSelector(bool selectorOn)
    {
        selector.SetActive(selectorOn);
        hasTurn = selectorOn;
    }

    private void OnMouseDown()
    {
        if (hasTurn)
        {
            if (!isOut)
            {
                LeaveBase();
            }
            else
            {
                StartTheMove(NetworkGameManagerLTA.instance.rolledHumanDice);
                isSelected = true;
            }
            NetworkGameManagerLTA.instance.DeactivateAllSelectors();
        }
    }
}
