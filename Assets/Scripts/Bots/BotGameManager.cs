using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework.Interfaces;
using UnityEngine.SceneManagement;

public enum States
{
    WAITING,
    ROLL_DICE,
    SWITCH_PLAYER,
    ATTACK
}

public class BotGameManager : MonoBehaviour
{
    public static BotGameManager Instance;

    public List<Player> playerList = new List<Player>();

    public int activePlayer;
    bool switchingPLayer;
    bool turnPossible = true;
    public bool hasBeenClicked = false;

    public bool canRollAgain;
    public int sixRollCountPerTurn;
    int numberOfRollsWithoutSix;

    //HUMAN INPUTS
    //roll dice button
    //attack button
    public GameObject rollButton;
    public GameObject attackButton;
    public int rolledHumanDice;

    [SerializeField] private Sprite[] diceSides;

    //STATEMACHINE
    public States state;

    private Camera mainCamera;
    public Image[] otherPlayerDices;

    public GameObject gameOverScreen;

    public TMP_Text turnText;
    public TMP_Text winningPlayerText;

    public string[] firstNameList = { "Ludo", "Rolling", "Lucky", "Hero", "Royal", "Knight", "Master", "Champ", "Star", "Legend" };
    public string[] lastNameList = { "Swift", "Blaze", "Ace", "Gamer", "Pro", "Victor", "King", "Queen", "Supreme", "Elite" };

    public TMP_Text[] allPlayerNamesTexts;

    public Sprite[] allPlayerImagesPrefabs;
    public Image[] allPlayerImages;

    public GameObject diceShine;
    public GameObject endScreen;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //prebaciti u playera
        playerList[0].playerName = PlayerPrefs.GetString("NAME");
        allPlayerNamesTexts[0].text = playerList[0].playerName;
        allPlayerImages[0].sprite = allPlayerImagesPrefabs[PlayerPrefs.GetInt("IMAGE")];

        for (int i = 1; i < playerList.Count; i++)
        {
            playerList[i].playerName = GetRandomName(firstNameList) + GetRandomName(lastNameList);
            allPlayerNamesTexts[i].text = playerList[i].playerName;

            allPlayerImages[i].sprite = allPlayerImagesPrefabs[Random.Range(0,allPlayerImagesPrefabs.Length)];
        }

        ActivateRollButton(false);
        mainCamera = Camera.main;
        rollButton.GetComponent<Button>().onClick.AddListener(() => HumanRollDice());
        attackButton.transform.GetChild(1).GetComponentInChildren<Button>().onClick.AddListener(() => AttackSpellCast());

        int randomPlayer = Random.Range(0, playerList.Count);
        activePlayer = randomPlayer;

        //Turn text color and value
        turnText.color = Utility.TeamToColor((Team)Utility.RetrieveTeamId((ulong)activePlayer));
        turnText.text = playerList[activePlayer].playerName + " has turn!";

        
    }

    private string GetRandomName(string[] nameList)
    {
        int randomIndex = Random.Range(0, nameList.Length);
        return nameList[randomIndex];
    }

    private void Update()
    {
        if (activePlayer == 0)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 10500.0f, LayerMask.GetMask("ActivePiece")))
            {
                if (Input.GetMouseButtonDown(0)) // Left click on a piece
                {
                    var selectedPiece = hit.collider.GetComponent<Pawn>();

                    if (selectedPiece.hasTurn)
                    {
                        if (!selectedPiece.isOut)
                        {
                            selectedPiece.LeaveBase();
                        }
                        else
                        { 
                            selectedPiece.StartTheMove(rolledHumanDice);
                            selectedPiece.ChangeIsSelected(true);
                        }
                        DeactivateAllSelectors();
                    }
                }
            }
        }

        if (playerList[activePlayer].playerType == BotPlayerTypes.CPU)
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

                    break;
                case States.SWITCH_PLAYER:
                    if (turnPossible)
                    {
                        StartCoroutine(SwitchPlayer());
                        state = States.WAITING;
                    }
                    
                    break;
                case States.ATTACK:

                    break;
            }
        }
        if (playerList[activePlayer].playerType == BotPlayerTypes.HUMAN)
        {
            switch (state)
            {
                case States.ROLL_DICE:
                    if (turnPossible)
                    {
                        ActivateRollButton(true);
                        state = States.WAITING;
                    }
                    break;
                case States.WAITING:

                    break;
                case States.SWITCH_PLAYER:
                    if (turnPossible)
                    {
                        //turn off isSelected for selected piece
                        for (int i = 0; i < playerList[activePlayer].allPawns.Length; i++)
                        {
                            if (playerList[activePlayer].allPawns[i].isSelected)
                            {
                                playerList[activePlayer].allPawns[i].ChangeIsSelected(false);
                            }
                        }

                        ActivateRollButton(false);
                        ActivateAttackButton(false);
                        StartCoroutine(SwitchPlayer());
                        state = States.WAITING;
                    }

                    break;
                case States.ATTACK:
                    ActivateAttackButton(true);
                    break;
            }
        }
    }

    public void AttackSpellCast()
    {
        for (int i = 0; i < playerList[activePlayer].allPawns.Length; i++)
        {
            Pawn selectedPiece = playerList[activePlayer].allPawns[i];

            if (selectedPiece.isSelected)
            {
                //selectedPiece.UpdateAnimationStateServerRpc(AnimationState.Attack); //play animation
                selectedPiece.ChangeAnimationState(PawnAnimationState.Attack);

                selectedPiece.spell.CastBotSpell(); //cast spell
            }
        }
        canRollAgain = true;

        ActivateAttackButton(false);

        if (canRollAgain)
        {
            state = States.ROLL_DICE;
        }
        else //THIS SHOULDN'T HAPPEND
        {
            state = States.SWITCH_PLAYER;
        }
    }

    void RollDice()
    {
        int diceNumber = Random.Range(1, 7);

        switch (activePlayer)
        {
            case 1:
                otherPlayerDices[0].sprite = diceSides[diceNumber - 1];
                break;
            case 2:
                otherPlayerDices[1].sprite = diceSides[diceNumber - 1];
                break;
            case 3:
                otherPlayerDices[2].sprite = diceSides[diceNumber - 1];
                break;
            default:
                break;
        }

        if(diceNumber == 6)
        {
            if(activePlayer > 1)
            {
                otherPlayerDices[activePlayer - 1].GetComponent<Image>().sprite = diceSides[6];
            }
            CheckStartNode(diceNumber);
        }

        if(diceNumber < 6)
        {
            MoveAStone(diceNumber);
        }
    }

    IEnumerator RollDiceDelay()
    {
        yield return new WaitForSeconds(1);
        RollDice();
    }

    void CheckStartNode(int diceNumber)
    {
        //is start node occupied
        bool startNodeFull = false;
        for (int i = 0; i < playerList[activePlayer].allPawns.Length; i++)
        {
            if (playerList[activePlayer].allPawns[i].currentNode == playerList[activePlayer].allPawns[i].startNode)
            {
                startNodeFull = true;
                break; //we found a match
            }
        }
        if (startNodeFull)
        {//moving
            MoveAStone(diceNumber);
            Debug.Log("The start node is full!");
        }
        else
        { //Leave base
            for (int i = 0; i < playerList[activePlayer].allPawns.Length; i++)
            {
                if (!playerList[activePlayer].allPawns[i].ReturnIsOut())
                {

                    playerList[activePlayer].allPawns[i].LeaveBase();
                    state = States.WAITING;
                    return;
                }

            }
            MoveAStone(diceNumber);
        }
    }

    void MoveAStone(int diceNumber)
    {
        List<Pawn> movablePawns = new List<Pawn>();
        List<Pawn> moveKickPawns = new List<Pawn>();

        //FILL THE LISTS
        for (int i = 0; i < playerList[activePlayer].allPawns.Length; i++)
        {
            if (playerList[activePlayer].allPawns[i].ReturnIsOut())
            {
                //CHECK FOR POSSIBLE KICK
                if (playerList[activePlayer].allPawns[i].CheckPossibleKick(playerList[activePlayer].allPawns[i].pawnId, diceNumber))
                {
                    moveKickPawns.Add(playerList[activePlayer].allPawns[i]);
                    continue;
                }
                //CHECK FOR POSSIBLE MOVE
                if (playerList[activePlayer].allPawns[i].CheckPossibleMove(diceNumber))
                {
                    movablePawns.Add(playerList[activePlayer].allPawns[i]);

                }
            }
        }
        //PERFORM KICK IF POSSIBLE
        if (moveKickPawns.Count > 0)
        {
            int num = Random.Range(0, moveKickPawns.Count);
            moveKickPawns[num].StartTheMove(diceNumber);
            state = States.WAITING;
            return;
        }
        //PERFORM MOVE IF POSSIBLE
        if (movablePawns.Count > 0)
        {
            int num = Random.Range(0, movablePawns.Count);
            movablePawns[num].StartTheMove(diceNumber);

            //if (playerList[activePlayer].playerType == BotPlayerTypes.HUMAN)
            //{
            //    //state = States.ATTACK;
            //}
            //else
            //{
            //    state = States.WAITING;
            //}
            return;
        }
        //NONE IS POSSIBLE
        //SWITCH PLAYER

        state = States.SWITCH_PLAYER;
    }

    IEnumerator SwitchPlayer()
    {
        if (switchingPLayer)
        {
            yield break;
        }
        switchingPLayer = true;

        yield return new WaitForSeconds(1);


        //SET NEXT PLAYER
        SetNextActivePlayer();

        switchingPLayer = false;
    }

    void SetNextActivePlayer()
    {
        activePlayer++;
        activePlayer %= playerList.Count;

        hasBeenClicked = false;

        //Reset roll count
        sixRollCountPerTurn = 0;

        int available = 0;
        for (int i = 0; i < playerList.Count; i++)
        {
            if (!playerList[i].hasWon)
            {
                available++;
            }
        }

        if (playerList[activePlayer].hasWon)
        {
            gameOverScreen.SetActive(true);
            winningPlayerText.text = playerList[activePlayer].playerName;
            state = States.WAITING;
            return;
        }

        //if (playerList[activePlayer].hasWon && available > 1)
        //{
        //    SetNextActivePlayer();
        //    return;
        //}
        //else if (available < 2)
        //{
        //    //GAME OVER SCREEN
        //    gameOverScreen.SetActive(true);

        //    state = States.WAITING;
        //    return;
        //}

        //Show text whose turn is
        turnText.color = Utility.TeamToColor((Team)Utility.RetrieveTeamId((ulong)activePlayer));
        turnText.text = playerList[activePlayer].playerName+ " has turn!";

        rollButton.GetComponent<Image>().sprite = diceSides[6];

        if (activePlayer > 1) //reset dice sprite when player finish rolling
        {
            otherPlayerDices[activePlayer - 2].sprite = diceSides[6];
        }
        else// case when player 0 has turn, reset player 3 dice
        {
            otherPlayerDices[2].sprite = diceSides[6];
        }

        state = States.ROLL_DICE;
    }
    public void ReportTurnPossible(bool possible)
    {
        turnPossible = possible;
    }

    public void ReportWinning()
    {
        //SHOW UI
        playerList[activePlayer].hasWon = true;

        //SAVE WINNERS
        //for (int i = 0; i < SaveSettings.winners.Length; i++)
        //{
        //    if (SaveSettings.winners[i] == "")
        //    {
        //        SaveSettings.winners[i] = playerList[activePlayer].playerName;
        //        break;
        //    }
        //}
    }

    //---------------------HUMAN INPUT ------------------------//
    void ActivateRollButton(bool buttonState)
    {
        rollButton.GetComponent<Button>().interactable = buttonState;
        diceShine.SetActive(buttonState);
        //rollButton.GetComponent<Button>().onClick.AddListener(()=>HumanRollDice());
    }
    void ActivateAttackButton(bool buttonState)
    {
        attackButton.transform.GetChild(1).GetComponent<Button>().interactable = buttonState;
    }

    public void DeactivateAllSelectors()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            for (int j = 0; j < playerList[i].allPawns.Length; j++)
            {
               playerList[i].allPawns[j].SetSelector(false);
            }
        }
    }

    public void HumanRoll()
    {
        ActivateRollButton(false);
    }
    public void HumanRoll(int forceDice)
    {
        ActivateRollButton(false);
    }

    bool IsAnyPawnOut()
    {
        for (int i = 0; i < playerList[activePlayer].allPawns.Length; i++)
        {
            if (playerList[activePlayer].allPawns[i].ReturnIsOut())
            {
                return false;
            }

        }return true;
    }

    //ON ROLL DICE BUTTON
    public void HumanRollDice(int forceDice = -1)
    {
        if (hasBeenClicked) { return; }
        if (sixRollCountPerTurn == 0 || canRollAgain)
        {
            hasBeenClicked = true;
            sixRollCountPerTurn++;
            canRollAgain = false;
            numberOfRollsWithoutSix++;

            //rollButton.GetComponent<Button>().onClick.RemoveAllListeners();

            if (numberOfRollsWithoutSix == 3 && IsAnyPawnOut())
            {
                rolledHumanDice = 6;
                numberOfRollsWithoutSix = 0;
            }
            else
            {
                rolledHumanDice = (forceDice == -1) ? Random.Range(1, 7) : forceDice;
            }

            Debug.Log(rolledHumanDice);
            rollButton.GetComponent<Image>().sprite = diceSides[rolledHumanDice - 1];

            //MOVABLE PAWN LIST
            List<Pawn> movablePawns = new List<Pawn>();

            if(rolledHumanDice == 6)
            {
                if(sixRollCountPerTurn == 3)
                {
                    state = States.SWITCH_PLAYER;
                    return;
                }
                else
                {
                    canRollAgain = true;
                }
            }
            //ActivateRollButton(false);
            //START NODE FULL CHECK
            //is start node occupied
            bool startNodeFull = false;
            for (int i = 0; i < playerList[activePlayer].allPawns.Length; i++)
            {
                if (playerList[activePlayer].allPawns[i].currentNode == playerList[activePlayer].allPawns[i].startNode)
                {
                    startNodeFull = true;
                    break; //we found a match
                }
            }

            //NUMBER <6
            if (rolledHumanDice < 6)
            {
                movablePawns.AddRange(PossiblePawns());
            }

            //NUMBER ==6 && !startnode
            if (rolledHumanDice == 6 && !startNodeFull)
            {
                //INSIDE BASE CHECK
                for (int i = 0; i < playerList[activePlayer].allPawns.Length; i++)
                {
                    if (!playerList[activePlayer].allPawns[i].ReturnIsOut())
                    {
                        movablePawns.Add(playerList[activePlayer].allPawns[i]);
                    }
                }
                //OUTSIDE CHECK
                movablePawns.AddRange(PossiblePawns());

                //NUMBER == 6 && startnode
            }
            else if (rolledHumanDice == 6 && startNodeFull)
            {
                movablePawns.AddRange(PossiblePawns());
            }

            //ACTIVATE ALL POSSIBLE SELECTORS
            if (movablePawns.Count > 0)
            {
                for (int i = 0; i < movablePawns.Count; i++)
                {
                    movablePawns[i].SetSelector(true);
                }
            }
            else
            {
                state = States.SWITCH_PLAYER;
            }
        }
    }

    List<Pawn> PossiblePawns()
    {
        List<Pawn> tempList = new List<Pawn>();

        for (int i = 0; i < playerList[activePlayer].allPawns.Length; i++)
        {
            //MAKE SURE HE IS OUT ALREADY
            if (playerList[activePlayer].allPawns[i].ReturnIsOut())
            {
                if (playerList[activePlayer].allPawns[i].CheckPossibleKick(playerList[activePlayer].allPawns[i].pawnId, rolledHumanDice))
                {
                    tempList.Add(playerList[activePlayer].allPawns[i]);
                    continue;
                }
                if (playerList[activePlayer].allPawns[i].CheckPossibleMove(rolledHumanDice))
                {
                    tempList.Add(playerList[activePlayer].allPawns[i]);
                }
            }
        }

        return tempList;
    }


    public void SwitchScene(string sceneToSwitch)
    {
        SceneManager.LoadScene(sceneToSwitch);
        //LevelLoaderManager.sceneToLoad = sceneToSwitch;
    }
}
