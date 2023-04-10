using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Globalization;
using System;
//using UnityEditor.PackageManager;
using Unity.Collections.LowLevel.Unsafe;

public class GameManager : NetworkBehaviour
{
    private bool playersSpawned = true;

    NetworkVariable<ulong> currentTurn = new NetworkVariable<ulong>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    NetworkVariable<int> currentDiceRoll = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    NetworkVariable<bool> moveCompleted = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    //[SerializeField]NetworkVariable<bool> canAttack = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [SerializeField] private GameObject piecePrefab;
    [SerializeField] private Transform pathContainer;
    [SerializeField] private Transform startPositionContainer;

    // Server-side values for moving / spawning pieces
    private Dictionary<ulong, bool> playerReady = new Dictionary<ulong, bool>();
    private Dictionary<ulong, Piece[]> playerPieces = new Dictionary<ulong, Piece[]>();
    private Dictionary<ulong, bool> playerCompleted = new Dictionary<ulong, bool>();
    private Dictionary<ulong, CustomNetworkVariables.NetworkString> playerNames = new Dictionary<ulong, CustomNetworkVariables.NetworkString>();
    private Dictionary<ulong, int> allPlayerImages = new Dictionary<ulong, int>();
    private Dictionary<int, int[]> paths;
    private Dictionary<int, Vector3[]> startPosition;
    private LudoTile[] board;
    private int[] finalTiles = new int[4];
    private int rollCountThisTurn;
    private bool canRollAgain;
    [SerializeField] private bool canAttack;

    // Client-side values
    private Piece[] localPieces = new Piece[4];
    // UI References
    [SerializeField] private Image currentTurnColor;
    [SerializeField] private TextMeshProUGUI diceRollText;
    [SerializeField] private Image playerColor;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private Transform winnerContainer;
    [SerializeField] private GameObject winnerPrefab;
    [SerializeField] private GameObject winnerPanel;
    [SerializeField] private Button attackButton;
    [SerializeField] private Sprite[] diceSides;
    [SerializeField] private TextMeshProUGUI currentTurnText;

    //Players info in UI
    [SerializeField] private Image[] playerImages;
    [SerializeField] private TextMeshProUGUI[] playerNamesText;
    [SerializeField] private Color playerColors;
    [SerializeField] private Image[] otherPlayerDices;
    public Sprite[] playerImageSprites;

    public TextMeshProUGUI winnerText;

    // CONST
    private const int TEAM_COUNT = 4;
    private const int TILE_COUNT = 57;
    private const int TILE_IN_PATH_PRIOR_TO_GOAL = 52;
    private const int TILE_OFFSET_IN_BETWEEN_TEAM = 13;

    //Timer
    public int duration;
    private int remainingDuration;

    // Callbacks
    private void Awake()
    {
        #region Set path values for every team 
        paths = new Dictionary<int, int[]>(TEAM_COUNT);
        for (int teamId = 0; teamId < TILE_OFFSET_IN_BETWEEN_TEAM; teamId++)
        {
            int[] teamPath = new int[TILE_COUNT];
            for (int i = 0; i < TILE_COUNT; i++)
            {
                int offset = (TILE_OFFSET_IN_BETWEEN_TEAM * teamId);
                if (i + ((offset == 0) ? 1 : offset) < TILE_IN_PATH_PRIOR_TO_GOAL)
                    teamPath[i] = i + offset;
                else if (i < TILE_IN_PATH_PRIOR_TO_GOAL - 1)
                    teamPath[i] = i - TILE_IN_PATH_PRIOR_TO_GOAL + offset;
                else
                    teamPath[i] = i + 1 + (teamId * 6);
            }

            paths.Add(teamId, teamPath);
        }
        var tempPath = paths[2];
        paths[2] = paths[1];
        paths[1] = tempPath;
        #endregion

        #region Initiate the board []
        board = new LudoTile[pathContainer.childCount];
        for (int i = 0; i < board.Length; i++)
        {
            board[i] = new LudoTile();
            board[i].tileTransform = pathContainer.GetChild(i); // for position ref
            board[i].pieces = new Piece[4]; // maximum of 4 on a single tile
        }
        #endregion

        #region Set the start position
        startPosition = new Dictionary<int, Vector3[]>(4);
        for (int i = 0; i < 4; i++)
        {
            Transform c = startPositionContainer.GetChild(i);
            Vector3[] positions = new Vector3[4];
            for (int j = 0; j < 4; j++)
                positions[j] = c.GetChild(j).position;

            startPosition.Add(i, positions);
        }
        #endregion

        #region Final Tiles for victory test
        finalTiles[0] = paths[0][TILE_COUNT - 1];
        finalTiles[1] = paths[1][TILE_COUNT - 1];
        finalTiles[2] = paths[2][TILE_COUNT - 1];
        finalTiles[3] = paths[3][TILE_COUNT - 1];
        #endregion
    }
    private void Start()
    {
        RegisterEvents();
        RepositionCamera(NetworkManager.Singleton.LocalClientId);
        OnPlayerReadyServerRpc(NetworkManager.Singleton.LocalClientId);

        UpdateCurrentTurnColorAndText();
        
    }

    public void UpdateCurrentTurnColorAndText()
    {
        if (!IsServer) { return; }
        string name;
        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            if (client.Key == currentTurn.Value)
            {
                name = client.Value.PlayerObject.GetComponent<PlayerController>().playerName.Value;
                UpdateCurrentTurnTextClientRpc(name, currentTurn.Value);
            }
        }
    }

    [ClientRpc]
    public void UpdateCurrentTurnTextClientRpc(string name, ulong currentTurnId)
    {
        //currentTurnText.text = name;
        currentTurnText.color = Utility.TeamToColor((Team)Utility.RetrieveTeamId(currentTurnId));
        //Debug.Log(currentTurnId);
        currentTurnText.text = $"{name} has turn!";
    }

    public override void OnDestroy()
    {
        UnregisterEvents();
    }

    public void Update()
    {
        if (playersSpawned && IsServer)
        {
            // Is this is the owner, set the movecompleted value so we can begin our first turn
            if (IsOwner)
            {
                moveCompleted.Value = true;
                moveCompleted.SetDirty(true);
            }

            // Instantiate logic values
            playerReady = new Dictionary<ulong, bool>();
            foreach (KeyValuePair<ulong, NetworkClient> nc in NetworkManager.Singleton.ConnectedClients)
            {
                playerReady.Add(nc.Key, false);
            }

            playersSpawned = false;
        }
        // If its not our turn, don't update the raycasts
        if (currentTurn.Value == NetworkManager.Singleton.LocalClientId)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 10500.0f, LayerMask.GetMask("ActivePiece")))
            {
                if (Input.GetMouseButtonDown(0)) // Left click on a piece
                {
                    var selectedPiece = hit.collider.GetComponent<Piece>();
                    //selectedPiece.isSelected.Value = true;
                    if (selectedPiece.GetComponent<NetworkObject>().IsOwner)
                    {
                        selectedPiece.UpdateIsSelectedStateServerRpc(true);
                    }
                    MovePiece(selectedPiece);
                }
            }
        }
    }
    //public void UpdatePieceIsSelected(Piece selectedPiece, bool value)
    //{
    //    selectedPiece.isSelected.Value = value;
    //}

    [ServerRpc(RequireOwnership = false)]
    public void UpdateIsSelectedServerRpc(int indexPos, bool newValue, ServerRpcParams serverRpcParams = default)
    {
        ulong clientId = serverRpcParams.Receive.SenderClientId;
        Piece selectedPiece = playerPieces[clientId][indexPos];

        selectedPiece.isSelected.Value = newValue;
        Debug.Log(selectedPiece.isSelected.Value);
        //UpdateIsSelectedClientRpc(selectedPieceValue,newValue);
    }


    //Camera
    private void RepositionCamera(ulong clientId)
    {
        switch (clientId)
        {
            case 0:
                Camera.main.transform.position = new Vector3(-50f,90f,50f);
                Camera.main.transform.rotation = Quaternion.Euler(50f,135f,0f); 
                break;
            case 1:
                Camera.main.transform.position = new Vector3(50f, 90f, -50f);
                Camera.main.transform.rotation = Quaternion.Euler(50f, -45f, 0f);
                break;
            case 2:
                Camera.main.transform.position = new Vector3(50f, 90f, 50f);
                Camera.main.transform.rotation = Quaternion.Euler(50f, -135f, 0f);
                break;
            case 3:
                Camera.main.transform.position = new Vector3(-50f, 90f, -50f);
                Camera.main.transform.rotation = Quaternion.Euler(50f, 45f, 0f);
                break;
            default:
                break;
        }
    }

    #region Piece logic
    private void MovePiece(Piece piece)
    {
        MovePieceServerRpc(Utility.GetPieceIndex(localPieces, piece));
    }
    // Ask the board if its even possible to move this piece based off our diceroll
    private bool CanMovePiece(Piece piece, int diceRoll)
    {
        // 0. Are we in play? if not, did we roll a 6
        if (piece.currentTile == -1 && diceRoll != 6)
        {
            return false;
        }
        else
        {
            // If theres two or more here
            if (board[paths[(int)piece.currentTeam][0]].PieceCount() > 1)
            {
                // If its not our team
                if (board[paths[(int)piece.currentTeam][0]].GetFirstPiece().currentTeam != piece.currentTeam)
                {
                    // A blocker is formed by the enemy team, can't spawn
                    return false;
                }
            }
        }

        // 1. Find the index in the current team's path
        int matchingIndex = FindIndexInPath(piece);

        // 2. Are we @ the limit
        if (matchingIndex + diceRoll >= paths[(int)piece.currentTeam].Length)
        {
            return false;
        }

        // 3. Check for obstructions
        for (int i = matchingIndex + 1; i <= matchingIndex + diceRoll; i++)
        {
            int targetTile = paths[(int)piece.currentTeam][i];
            int pieceCount = board[targetTile].PieceCount();

            // Initial path, prior to the safe zone
            if (i <= TILE_IN_PATH_PRIOR_TO_GOAL)
            {
                // If no one is here, or a single piece, continue
                if (pieceCount < 2)
                    continue;

                // If theres more than one piece, and its our team
                if (board[targetTile].GetFirstPiece().currentTeam == piece.currentTeam)
                    continue;
                else
                    return false;
            }
            // Pieces of the same team can't stack in the house
            else if (i < 56)
            {
                if (pieceCount > 0)
                    return false;
                else
                    return true;
            }
        }

        // Made it that far? You can do the move!
        return true;
    }
    // Path logic
    private int FindIndexInPath(Piece piece)
    {
        // Find the index of the current location, based off each team's individual path[]
        int matchingIndex = 0;
        for (int j = 0; j < paths[(int)piece.currentTeam].Length; j++)
        {
            //Debug.Log(paths[(int)piece.currentTeam].Length);
            if (paths[(int)piece.currentTeam][j] == piece.currentTile)
            {
                matchingIndex = j;
                break;
            }
        }

        return matchingIndex;
    }
    #endregion
    #region Server side methods
    //Started working on timer, problem with losing track of turns, couldnt roll dice on any client for some reason
    //private void BeginTimer(int second)
    //{
    //    remainingDuration = second;
    //    if (switcherCoroutine != null)
    //    {
    //        StopCoroutine(switcherCoroutine);
    //    }
    //    switcherCoroutine = StartCoroutine(UpdateTimer());
    //}

    //private IEnumerator UpdateTimer()
    //{
    //    while (remainingDuration >= 0)
    //    {
    //        //update UI
    //        remainingDuration--;
    //        Debug.Log(remainingDuration);
    //        yield return new WaitForSeconds(1f);
    //    }
    //    //OnEnd();
    //    Debug.Log("Should switch turn");
    //    NextTurn();
    //}
    //private Coroutine switcherCoroutine;
    private void NextTurn()
    {
        // 0. Save the previous turn value, so we can check which one is next
        var previousTurn = currentTurn.Value;

        // 1. Reset the roll count, used in the 3x 6 in a row roll turn skip
        rollCountThisTurn = 0;
        //duration = 10;
        
        //BeginTimer(duration);
        // 2. Create an array with the client IDs
        ulong[] turnIds = NetworkManager.Singleton.ConnectedClients.Keys.ToArray();

        // 3. Ensure the current player is at the begening of the array, this will swap positions until it is true
        // ex : Blue's turn : [0,2,3,4] -> [4,0,2,3]
        for (int i = 0; i < turnIds.Length; i++)
        {
            // Is it already in order
            if (previousTurn == 0)
                break;

            // Shift all values by one, and sets the last value as the previous first
            if (turnIds[i] == previousTurn)
            {
                while (turnIds[0] != previousTurn)
                {
                    ulong prevLead = turnIds[0];
                    for (int j = 1; j < turnIds.Length; j++)
                        turnIds[j - 1] = turnIds[j];
                    turnIds[turnIds.Length - 1] = prevLead;
                }

                break;
            }
        }

        // If the next player's is done with the game, skip him
        for (int i = 1; i < turnIds.Length; i++)
        {
            if (!playerCompleted[turnIds[i]])
            {
                currentTurn.Value = turnIds[i];
                break;
            }
        }
    }

    private GameObject Swap(string pawnName, List<GameObject> pawns)
    {
        switch (pawnName)
        {
            case "Spearman":
                piecePrefab = pawns[0]; //item iz liste game objekata u kojoj se cuvaju heroji
                break;
            case "Archer":
                piecePrefab = pawns[1]; //item iz liste game objekata u kojoj se cuvaju heroji
                break;
            case "Macebearer":
                piecePrefab = pawns[2]; //item iz liste game objekata u kojoj se cuvaju heroji
                break;
            case "Slinger":
                piecePrefab = pawns[3]; //item iz liste game objekata u kojoj se cuvaju heroji
                break;
            default:
                break;
        }
        return piecePrefab;

    }

    [ClientRpc]
    private void UpdatePlayerNamesClientRpc(ulong key, string name, int playerImageIndex)
    {
        playerNames.Add(key, name);
        allPlayerImages.Add(key, playerImageIndex);
    }

    private void SpawnAllPlayers()
    {
        // Assign the values (owner side only)
        playerPieces = new Dictionary<ulong, Piece[]>(4);
        playerCompleted = new Dictionary<ulong, bool>(4);
        playerNames = new Dictionary<ulong, CustomNetworkVariables.NetworkString>(4);

        foreach (KeyValuePair<ulong, NetworkClient> nc in NetworkManager.Singleton.ConnectedClients)
        {
            PlayerController pc = nc.Value.PlayerObject.GetComponent<PlayerController>();
            for (int i = 0; i < NetworkManager.Singleton.ConnectedClients.Count; i++)
            {
                if (!playerNames.ContainsKey(nc.Key))
                {
                    UpdatePlayerNamesClientRpc(nc.Key,pc.playerName.Value, pc.playerImageIndex.Value);
                }
            }
        }

        int playerIndex = 0;
        foreach (KeyValuePair<ulong, NetworkClient> nc in NetworkManager.Singleton.ConnectedClients)
        {
            Piece[] pieces = new Piece[4];

            // Spawn 4 Networked pieces for every active player
            PlayerController pc = nc.Value.PlayerObject.GetComponent<PlayerController>();
            for (int i = 0; i < 4; i++)
            {
                //GameObject go = GameObject.Instantiate(piecePrefab);

                GameObject go = Instantiate(Swap(pc.deckStrings[i], pc.pawnContainer));

                go.transform.position = startPosition[playerIndex][i];
                go.GetComponent<NetworkObject>().SpawnWithOwnership(nc.Key);
                pieces[i] = go.GetComponent<Piece>();

                int[] value;
                if(paths.TryGetValue(((int)pieces[i].currentTeam), out value))
                {
                    for (int j = 0; j < value.Length; j++)
                    {
                        pieces[i].head.GetComponent<Renderer>().material = pieces[i].allPawnColorMaterials[((int)pieces[i].currentTeam)];
                        pieces[i].body.GetComponent<Renderer>().material = pieces[i].allPawnColorMaterials[((int)pieces[i].currentTeam)];
                        pieces[i].board.Add(board[paths[((int)pieces[i].currentTeam)][j]]);
                    }
                }
                //Sets starting rotation
                pieces[i].transform.rotation = Utility.TeamToRotataion(pieces[i].currentTeam);
            }

            ClientRpcParams clientRpcParams = new ClientRpcParams()
            {
                Send = new ClientRpcSendParams()
                {
                    TargetClientIds = new ulong[] { nc.Key }
                }
            };
            
            ChangeColorAndNameClientRpc(nc.Key, pc.playerName.Value,clientRpcParams);

            playerPieces.Add(nc.Key, pieces);
            playerCompleted.Add(nc.Key, false);
           
            playerIndex++;
        }
    }
    #endregion
    #region Client side methods
    private Piece[] FindLocalPieces()
    {
        // This is for players to find which pieces are theirs, and to assign localPiece
        var r = new Piece[4];
        var pieces = FindObjectsOfType<Piece>();
        int pi = 0;
        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i].IsOwner)
            {
                r[pi] = pieces[i];
                pi++;
            }
        }

        // Using FindObjectOfType pulls the object from top to bottom
        System.Array.Reverse(r);

        return r;
    }
    #endregion  
    // Buttons
    public void DiceRollButton()
    {
        //currentDiceRoll.OnValueChanged += UpdateDiceUI;
        DiceRollServerRpc(NetworkManager.Singleton.LocalClientId);
    }
    public void DiceRollButton(int forceDice)
    {
        DiceRollServerRpc(NetworkManager.Singleton.LocalClientId, forceDice);
    }
    public void AttackButton()
    {
        AttackServerRpc(NetworkManager.Singleton.LocalClientId);
    }
    public void EndButton()
    {
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("Lobby");
    }

    // Events
    private void RegisterEvents()
    {
        currentTurn.OnValueChanged += UpdateCurrentTurn;
        currentDiceRoll.OnValueChanged += UpdateDiceUI;
        moveCompleted.OnValueChanged += UpdateMoveCompleted;
        
    }
    private void UnregisterEvents()
    {
        currentTurn.OnValueChanged -= UpdateCurrentTurn;
        currentDiceRoll.OnValueChanged -= UpdateDiceUI;
        moveCompleted.OnValueChanged -= UpdateMoveCompleted;
    }
    // Called by OnValueChanged for moveCompleted
    private void UpdateMoveCompleted(bool prev, bool newValue)
    {
        //if (newValue)
        //    diceRollText.text = "Roll!";
    }
    // Called by OnValueChanged for currentTurn
    private void UpdateCurrentTurn(ulong prev, ulong newValue)
    {
        string name;
        if (!IsServer) { return; }
        
        foreach(var client in NetworkManager.Singleton.ConnectedClients)
        {
            if(client.Key == newValue)
            {
                name = client.Value.PlayerObject.GetComponent<PlayerController>().playerName.Value;
                //currentTurnText.text = $"{name} has turn!";
                UpdateCurrentTurnTextClientRpc(name,newValue);
            }
        }   
        //int teamId = Utility.RetrieveTeamId(newValue);
        //currentTurnColor.color = Utility.TeamToColor((Team)teamId);
    }
    // Called by OnValueChanged for currentDiceRoll
    private void UpdateDiceUI(int prev, int newValue)
    {
        //diceRollText.text = newValue.ToString();
        StartCoroutine(DiceAnimation(newValue));
    }

    IEnumerator DiceAnimation(int diceValue)
    {
        // Pick up random value from 0 to 5 (All inclusive)
        int randomDiceSide = 0;

        //Roll BIG (main) dice for client device
        if (currentTurn.Value == NetworkManager.Singleton.LocalClientId)
        {
            //vrti samo veliku playerovu kockicu
            for (int i = 0; i <= 10; i++)
            {
                randomDiceSide = UnityEngine.Random.Range(0, 5);
                // Set sprite to upper face of dice from array according to random value
                otherPlayerDices[0].sprite = diceSides[randomDiceSide];

                // Pause before next itteration
                yield return new WaitForSeconds(0.05f);
            }
            otherPlayerDices[0].sprite = diceSides[diceValue - 1];            
        }
        else
        {
            int turn = 0;
            //Show rolling dice from main client to other clients
            for (int i = 0; i <= 10; i++)
            {
                // Set sprite to upper face of dice from array according to random value

                randomDiceSide = UnityEngine.Random.Range(0, 5);

                //update player 0 (RED) dice animation to other clients
                if ((i > 0 && i < 4) && (int)NetworkManager.Singleton.LocalClientId == i)
                {
                    otherPlayerDices[i].sprite = diceSides[randomDiceSide];
                }

                //update other clients dice animaiton on player 0 (RED)
                if ((int)NetworkManager.Singleton.LocalClientId == 0 && i < 4)
                {
                    //otherPlayerDices[(int)currentTurn.Value].sprite = diceSides[randomDiceSide];
                    otherPlayerDices[(int)currentTurn.Value].sprite = diceSides[randomDiceSide];
                    turn = (int)currentTurn.Value;
                    continue;
                }

                // Pause before next itteration
                yield return new WaitForSeconds(0.05f);
            }

            for (int j = 1; j < otherPlayerDices.Length; j++)
            {
                //update player 0 (RED) dice VALUE to other clients
                if ((j > 0 && j < 4) && (int)NetworkManager.Singleton.LocalClientId == j)
                {
                    otherPlayerDices[j].sprite = diceSides[diceValue - 1];
                }

                //update other clients dice VALUE on player 0 (RED)
                if ((int)NetworkManager.Singleton.LocalClientId == 0 && j < 4)
                {
                    otherPlayerDices[turn].sprite = diceSides[diceValue - 1];
                }
            }
        } 
    }

    // RPCs



    [ServerRpc(RequireOwnership = false)]
    private void AttackServerRpc(ulong clientId)
    {
        for (int i = 0; i < playerPieces[clientId].Length; i++)
        {
            Piece selectedPiece = playerPieces[clientId][i];

            if (selectedPiece.isSelected.Value)
            {
                selectedPiece.UpdateAnimationStateServerRpc(AnimationState.Attack);

                selectedPiece.spell.CastSpell(selectedPiece.currentTile + selectedPiece.eatPower, board);

                selectedPiece.UpdateIsSelectedStateServerRpc(false);
                //UpdatePieceIsSelected(selectedPiece,false);
            }
        }
        ClientRpcParams clientRpcParams = new ClientRpcParams()
        {
            Send = new ClientRpcSendParams()
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };
        //EnableAttackClientRpc(false, clientRpcParams);
        EnableAttackServerRpc(false, clientId);

        NextTurn();
    }
    [ClientRpc]
    public void ChangeColorAndNameClientRpc(ulong clientId, string updatedPlayerName, ClientRpcParams clientRpcParams)
    {
        if (NetworkManager.Singleton.LocalClientId == clientId)
        {
            var temp = playerImages[0];
            playerImages[0] = playerImages[clientId];
            playerImages[clientId] = temp;
        }
        //Lists for player backround color also player images should be here
        List<ulong> tempList = new List<ulong> { 0, 1, 2, 3 };

        var listColor = new List<Image>(playerImages);

        //Lists for showing player names
        var listNames = new List<TextMeshProUGUI>(playerNamesText);

        var temp2 = playerNames[clientId];
        playerNames[clientId] = playerNames[0];
        playerNames[0] = temp2;

        //List for dice colors for every player
        List<ulong> tempList2 = new List<ulong> { 0, 1, 2, 3 };
        var temp3 = tempList2[(int)clientId];
        tempList2[(int)clientId] = tempList2[0];
        tempList2[0] = temp3;

        //setting background and dice color
        for (int i = 0; i < listColor.Count; i++)
        {
            //listColor[i].color = Utility.TeamToColor(((Team)Utility.RetrieveTeamId(tempList[i])));
            if (allPlayerImages.ContainsKey((ulong)i))
            {
                listColor[i].sprite = playerImageSprites[allPlayerImages[(ulong)i]];
            }
            
            otherPlayerDices[i].color = Utility.TeamToColor(((Team)Utility.RetrieveTeamId(tempList2[i])));
        }

        //setting names
        for (int i = 0; i < playerNames.Count; i++)
        {
            listNames[i].text = playerNames[(ulong)i];
        }

        //setting color of attack button
        attackButton.gameObject.GetComponent<Image>().color = Utility.TeamToColor(((Team)Utility.RetrieveTeamId(clientId)));
        attackButton.interactable = false;
    }

    [ServerRpc(RequireOwnership = false)]
    private void EnterPieceServerRpc(int index, ulong clientIndex)
    {
        int teamId = Utility.RetrieveTeamId(clientIndex);
        Piece piece = playerPieces[clientIndex][index];
        int startPosition = paths[teamId][0];

        // 1. Move the piece @ the start
        //piece.steps = 0;
        piece.UpdateStepsValueClientRpc(0);

        //piece.transform.position = board[startPosition].tileTransform.position;

        board[startPosition].AddPiece(piece);
        piece.currentTile = startPosition;

        //piece.transform.position = board[startPosition].tileTransform.position;
        //board[startPosition].RepositionPieces();

        //1.a Rotate piece
        piece.RotatePawn(board[startPosition + 1].tileTransform.position);

        EatEnemyPawn(piece, startPosition);

        moveCompleted.Value = true;
        moveCompleted.SetDirty(true);

        //UpdatePieceIsSelected(piece, false);
        piece.UpdateIsSelectedStateServerRpc(false);

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientIndex }
            }
        };
        DisableInteractionClientRpc(clientRpcParams);
    }

    private void EatEnemyPawn(Piece piece, int startPosition)
    {
        Piece p = board[startPosition].GetEnemyPiece(piece);

        if (p != null && p.currentTeam != piece.currentTeam)
        {
            p.UpdateAnimationStateServerRpc(AnimationState.Death);
            board[startPosition].RemovePiece(p);
            p.currentTile = -1;
            p.isOut = false;
            p.routePosition = 0;
            p.PositionClientRpc(-Vector3.one); // start position is set localy
            p.transform.rotation = Utility.TeamToRotataion(p.currentTeam);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void MovePieceServerRpc(int indexPos, ServerRpcParams serverRpcParams = default)
    {
        ulong clientId = serverRpcParams.Receive.SenderClientId;
        Piece piece = playerPieces[clientId][indexPos];

        // 0. Are we in play?
        if (piece.currentTile == -1)
        {
            EnterPieceServerRpc(indexPos, clientId);
            return;
        }

        // 1. Find the index in the current team's path
        int matchingIndex = FindIndexInPath(piece);
        int targetTile = paths[(int)piece.currentTeam][matchingIndex + currentDiceRoll.Value];
        int previousPosition = piece.currentTile;

        // 2. Are we killing any piece?
        //EatEnemyPawn(piece,targetTile);

        // 3. Move the piece there
        //piece.steps = currentDiceRoll.Value; //adds steps to pawn
        piece.UpdateStepsValueClientRpc(currentDiceRoll.Value);

        board[targetTile].AddPiece(piece);
        piece.currentTile = targetTile;

        board[previousPosition].RemovePiece(piece);

        moveCompleted.Value = true;
        moveCompleted.SetDirty(true);
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };
        DisableInteractionClientRpc(clientRpcParams);

        // 4. Are you winning?
        if (finalTiles.Contains(targetTile))
        {
            if (board[targetTile].PieceCount() == 4)
            {
                NetworkManager.Singleton.ConnectedClients.TryGetValue(serverRpcParams.Receive.SenderClientId, out var networkedClient);
                string playerName = networkedClient.PlayerObject.GetComponent<PlayerController>().playerName.Value;
                AddWinnerToTheListClientRpc(playerName);
                playerCompleted[serverRpcParams.Receive.SenderClientId] = true;

                //winnerText.text = playerName;
                UpdateWinnerNameClientRpc(playerName);

                int i = 0;
                foreach (KeyValuePair<ulong, bool> pd in playerCompleted)
                    if (!pd.Value)
                        i++;

                if (i == 1) // If theres only one player left
                {
                    EndGameClientRpc();
                    return;
                }

                NextTurn();
                return;
            }
        }
        //Wait for piece to finish moving and activate button
        piece.t.Finished += delegate (bool manual)
        {
            if (!manual)
            {
                piece.UpdateAnimationStateServerRpc(AnimationState.Idle);

                EatEnemyPawn(piece, targetTile); //moved here so enemy pawn is eaten when peace reach that tile

                if ((piece.currentTile > 0 && piece.currentTile < 50 - piece.eatPower) &&
                    (board[piece.currentTile + piece.eatPower].GetFirstPiece() != null) && (board[piece.currentTile + piece.eatPower].GetFirstPiece().currentTeam != piece.currentTeam))
                {
                    //EnableAttackClientRpc(true, clientRpcParams);
                    EnableAttackServerRpc(true, clientId);
                }
                else
                {
                    //UpdatePieceIsSelected(piece,false);
                    piece.UpdateIsSelectedStateServerRpc(false);
                }
                
                if (piece.currentTile == 57)
                {
                    piece.gameObject.SetActive(false);
                }
                if (!canRollAgain && !canAttack)
                    NextTurn();
            }
        };
    }
    [ServerRpc(RequireOwnership = false)]
    private void EnableAttackServerRpc(bool state, ulong clientId)
    {
        canAttack = state;

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };

        EnableAttackClientRpc(state, clientRpcParams);
    }
    [ClientRpc]
    private void UpdateWinnerNameClientRpc(string name)
    {
        winnerText.text = name;
    }

    [ClientRpc]
    private void EnableAttackClientRpc(bool state, ClientRpcParams clientRpcParams)
    {
        canAttack = state;
        //Debug.Log(canAttack);
        attackButton.interactable = canAttack;
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnPlayerReadyServerRpc(ulong clientId)
    {
        // Called when the player is done loading his scene, and his network is initialized
        playerReady[clientId] = true;
        // If all players are ready, spawn all the networked pieces
        if (!playerReady.ContainsValue(false))
            SpawnAllPlayers();
    } 



    [ServerRpc(RequireOwnership = false)]
    public void DiceRollServerRpc(ulong clientId, int forceDice = -1)
    {
        // If we're either on the first turn, or we rolled a 6 
        if ((rollCountThisTurn == 0 || canRollAgain) && clientId == currentTurn.Value && moveCompleted.Value)
        {
            // Actually roll
            int rv = (forceDice == -1) ? UnityEngine.Random.Range(1, 7) : forceDice;

            rollCountThisTurn++;
            canRollAgain = false;
            if (rv == 6)
            {
                if (rollCountThisTurn == 3)
                {
                    // If we roll 3x 6 in a row, return
                    NextTurn();
                    return;
                }
                else
                {
                    canRollAgain = true;
                }
            }

            if(currentDiceRoll.Value == rv) // check if new roll is same as old one and do roll animation again
            {
                StartCoroutine(DiceAnimation(rv));
            }

            currentDiceRoll.Value = rv;
            currentDiceRoll.SetDirty(true);

            // Can we move any?
            bool[] pieceYouCanMove = new bool[4];
            bool canMove = false;
            for (int i = 0; i < playerPieces[clientId].Length; i++)
            {
                // For all of your pieces, check if we're allowed to move that piece with the current diceroll
                pieceYouCanMove[i] = CanMovePiece(playerPieces[clientId][i], rv);
                if (pieceYouCanMove[i])
                    canMove = true;
            }

            // If i can move atleast one piece, i gotta do it
            if (canMove)
            {
                moveCompleted.Value = false;
                moveCompleted.SetDirty(true);
                ClientRpcParams clientRpcParams = new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new ulong[] { clientId }
                    }
                };
                EnableInteractionClientRpc(pieceYouCanMove, clientRpcParams);
                //EnableAttackClientRpc(false,clientRpcParams);
                EnableAttackServerRpc(false, clientId);
            }
            else
            {
                NextTurn();
            }
        }
    }

    [ClientRpc]
    public void AddWinnerToTheListClientRpc(string playerName)
    {
        Debug.Log(playerName);
        // Sent to all clients when a player finishes
        string preString = "";
        int cc = winnerContainer.childCount;
        if (cc == 0)
            preString = "1st";
        else if (cc == 1)
            preString = "2nd";
        else if (cc == 2)
            preString = "3rd";
        else
            preString = "4th";

        // Create the piece of UI and fill the name of the winner
        GameObject go = Instantiate(winnerPrefab, winnerContainer);
        var t = go.GetComponent<TextMeshProUGUI>();
        t.text = $"{preString} : {playerName}";
    }
    [ClientRpc]
    public void EndGameClientRpc()
    {
        // Sent to all clients when theres a single player left
        winnerPanel.SetActive(true);
    }
    [ClientRpc]
    public void EnableInteractionClientRpc(bool[] pieceYouCanMove, ClientRpcParams clientRpcParams)
    {
        // Sent to current turn client, and activate the pieces that they can move
        if (localPieces[0] == null)
            localPieces = FindLocalPieces();

        for (int i = 0; i < pieceYouCanMove.Length; i++)
            if (localPieces[i] != null && pieceYouCanMove[i])
                localPieces[i].EnableInteraction();
    }
    [ClientRpc]
    public void DisableInteractionClientRpc(ClientRpcParams clientRpcParams)
    {
        // Sent to current turn client, turn off all pieces
        for (int i = 0; i < localPieces.Length; i++)
            localPieces[i].DisableInteraction();
    }

    public void Disconnect()
    {
        //LobbyScene.isGameLeft = true;
        NetworkManager.Singleton.Shutdown();
        Destroy(NetworkManager.Singleton.gameObject);
        //NetworkManager.Singleton.SceneManager.LoadScene("MainScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
        SceneManager.LoadScene("MainScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
        
    }
}
