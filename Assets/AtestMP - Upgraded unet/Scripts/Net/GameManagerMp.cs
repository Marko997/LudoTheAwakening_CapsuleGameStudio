using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerMp : NetworkBehaviour
{
    [SyncVar]private int currentTurn;
    [SyncVar]private int currentDiceRoll;
    [SyncVar]private bool moveCompleted;

    [SerializeField] private GameObject piecePrefab;
    [SerializeField] private Transform pathContainer;
    [SerializeField] private Transform startPositionContainer;

    //Server side values
    private Dictionary<int, bool> playerReady = new Dictionary<int, bool>();
    private Dictionary<int, Piece[]> playerPieces = new Dictionary<int, Piece[]>();
    private Dictionary<int, bool> playerCompleted = new Dictionary<int, bool>();
    private Dictionary<int, int[]> paths;
    private Dictionary<int, Vector3[]> startPosition;
    private LudoTile[] board;
    private int[] finalTiles = new int[4];
    private int rollCountThisTurn;
    private bool canRollAgain;


    //Client side values
    private Piece[] localPieces = new Piece[4];

    [SerializeField] private Image currentTurnColor;
    [SerializeField] private TextMeshProUGUI diceRollText;
    [SerializeField] private Image playerColor;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private Transform winnerContainer;
    [SerializeField] private GameObject winnerPrefab;
    [SerializeField] private GameObject winnerPanel;

    //Const
    private const int TEAM_COUNT = 4;
    private const int TILE_COUNT = 57;
    private const int TILE_IN_PATH_PRIOR_TO_GOAL = 52;
    private const int TILE_OFFSET_IN_BETWEEN_TEAM = 13;

    //Callbacks
    private void Awake()
    {
        #region Set Path Values For Every Team
        paths = new Dictionary<int, int[]>(TEAM_COUNT);
        for (int teamId = 0; teamId < TILE_OFFSET_IN_BETWEEN_TEAM; teamId++)
        {
            int[] teamPath = new int[TILE_COUNT];
            for (int i = 0; i < TILE_COUNT; i++)
            {
                int offset = (TILE_OFFSET_IN_BETWEEN_TEAM * teamId);
                if(i+((offset == 0) ? 1: offset) < TILE_IN_PATH_PRIOR_TO_GOAL)
                {
                    teamPath[i] = i + offset;
                }else if(i< TILE_IN_PATH_PRIOR_TO_GOAL - 1)
                {
                    teamPath[i] = i - TILE_IN_PATH_PRIOR_TO_GOAL + offset;
                }
                else
                {
                    teamPath[i] = i + 1 + (teamId * 6);
                }
            }
            paths.Add(teamId, teamPath);
        }


        #endregion

        #region Set the start position
        startPosition = new Dictionary<int, Vector3[]>(4);
        for (int i = 0; i < 4; i++)
        {
            Transform c = startPositionContainer.GetChild(i);
            Vector3[] positions = new Vector3[4];
            for (int j = 0; j < 4; j++)
            {
                positions[j] = c.GetChild(j).position;
            }
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
        OnPlayerReadyClientRpc(NetworkClient.connection.connectionId);
    }

    public override void OnStartServer()
    {
        if (hasAuthority)
        {
            moveCompleted = true;
            //moveCompleted.setDitry(true);
        }

        playerReady = new Dictionary<int, bool>();
        
        //foreach (var nc in NetworkManager.singleton.connectedClients)
        foreach(var nc in NetworkServer.connections)
        {
            Debug.Log(nc + " sadasdsadasd");
            Debug.Log(nc.Value.identity + " sadasdsadasd");
            playerReady.Add(nc.Key, false);
            var pc = nc.Value.identity.GetComponent<PlayerController>();

            //var pc = nc.Value.PlayerObject.GetComponent<PlayerController>();
            Debug.Log(pc + " pccccc");
            if (pc.hasAuthority)
            {
                playerColor.color = Utility.TeamToColor(((Team)Utility.RetrieveTeamId(pc.connectionToClient.connectionId)));
                //playerName.text = pc.playerName;
            }
        }
    }

    public void OnDestroy()
    {
        UnregisterEvents();
    }

    public void Update()
    {
        if(currentTurn == NetworkClient.connection.connectionId)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, 50.0f, LayerMask.GetMask("ActivePiece")))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    MovePiece(hit.collider.GetComponent<Piece>());
                }
            }
        }
    }

    private void MovePiece(Piece piece)
    {
        MovePieceClientRpc(Utility.GetPieceIndex(localPieces, piece));
    }

    private bool CanMovePiece(Piece piece, int diceRoll)
    {
        if (piece.currentTile == -1 && diceRoll != 6)
        {
            return false;
        }
        else
        {
            if (board[paths[(int)piece.currentTeam][0]].PieceCount() > 1)
            {
                if (board[paths[(int)piece.currentTeam][0]].GetFirstPiece().currentTeam != piece.currentTeam)
                {
                    // A blocker is formed by the enemy team, can't spawn
                    return false;
                }
            }
        }
        int matchingIndex = FindIndexInPath(piece);

        if(matchingIndex + diceRoll >= paths[(int)piece.currentTeam].Length)
        {
            return false;
        }

        for (int i = matchingIndex; i <= matchingIndex + diceRoll; i++)
        {
            int targetTile = paths[(int)piece.currentTeam][i];
            int pieceCount = board[targetTile].PieceCount();

            if (i <= TILE_IN_PATH_PRIOR_TO_GOAL)
            {
                if (pieceCount < 2)
                {
                    continue;
                }
                if (board[targetTile].GetFirstPiece().currentTeam == piece.currentTeam)
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            else if (i < 50)
            {
                if (pieceCount > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

        }
        return true;
    }

    private int FindIndexInPath(Piece piece)
    {
        int matchingIndex = 0;
        for(int j =0; j< paths[(int)piece.currentTeam].Length; j++)
        {
            if(paths[(int)piece.currentTeam][j] == piece.currentTile)
            {
                matchingIndex = j;
                break;
            }
        }
        return matchingIndex;
    }

    // Server side methods
    private void NextTurn()
    {
        // 0. Save the previous turn value, so we can check which one is next
        var previousTurn = currentTurn;

        // 1. Reset the roll count, used in the 3x 6 in a row roll turn skip
        rollCountThisTurn = 0;

        // 2. Create an array with the client IDs
        //int[] turnIds = NetworkManager.singleton.ConnectedClients.Keys.ToArray();
        int[] turnIds = NetworkServer.connections.Keys.ToArray();

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
                    int prevLead = turnIds[0];
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
                currentTurn = turnIds[i];
                break;
            }
        }
    }
    private void SpawnAllPlayers()
    {
        // Assign the values (owner side only)
        playerPieces = new Dictionary<int, Piece[]>(4);
        playerCompleted = new Dictionary<int, bool>(4);

        int playerIndex = 0;
        foreach (KeyValuePair<int, NetworkConnectionToClient> nc in NetworkServer.connections)
        {
            Piece[] pieces = new Piece[4];

            // Spawn 4 Networked pieces for every active player
            PlayerController pc = nc.Value.identity.GetComponent<PlayerController>();
            for (int i = 0; i < 4; i++)
            {
                GameObject go = GameObject.Instantiate(piecePrefab);
                go.transform.position = startPosition[playerIndex][i];
                NetworkServer.Spawn(go.gameObject, nc.Value.identity.connectionToClient);
                //go.GetComponent<NetworkIdentity>().SpawnWithOwnership(nc.Key);
                pieces[i] = go.GetComponent<Piece>();
            }

            playerPieces.Add(nc.Key, pieces);
            playerCompleted.Add(nc.Key, false);

            playerIndex++;
        }
    }

    // Client side methods
    private Piece[] FindLocalPieces()
    {
        // This is for players to find which pieces are theirs, and to assign localPiece
        var r = new Piece[4];
        var pieces = FindObjectsOfType<Piece>();
        int pi = 0;
        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i].hasAuthority) // valjda
            {
                r[pi] = pieces[i];
                pi++;
            }
        }

        // Using FindObjectOfType pulls the object from top to bottom
        System.Array.Reverse(r);

        return r;
    }

    // Buttons
    public void DiceRollButton()
    {
        //DiceRollServerRpc(NetworkManager.singleton.LocalClientId);
        DiceRollServerRpc(NetworkClient.connection.connectionId,-1);
    }
    public void DiceRollButton(int forceDice)
    {
        DiceRollServerRpc(NetworkClient.localPlayer.connectionToClient.connectionId, forceDice);
    }
    public void EndButton()
    {
        NetworkManager.Shutdown();
        SceneManager.LoadScene("Lobby");
    }

    // Events
    private void RegisterEvents()
    {
        //currentTurn.OnValueChanged += UpdateCurrentTurn;
        //currentDiceRoll.OnValueChanged += UpdateDiceUI;
        //moveCompleted.OnValueChanged += UpdateMoveCompleted;
        //UpdateCurrentTurn();
        //UpdateDiceUI();
        //UpdateMoveCompleted();
    }
    private void UnregisterEvents()
    {
        //currentTurn.OnValueChanged -= UpdateCurrentTurn;
        //currentDiceRoll.OnValueChanged -= UpdateDiceUI;
        //moveCompleted.OnValueChanged -= UpdateMoveCompleted;
    }

    // Called by OnValueChanged for currentTurn
    private void UpdateCurrentTurn(ulong prev, int newValue)
    {
        int teamId = Utility.RetrieveTeamId(newValue);
        currentTurnColor.color = Utility.TeamToColor((Team)teamId);
    }
    // Called by OnValueChanged for currentDiceRoll
    private void UpdateDiceUI(int prev, int newValue)
    {
        diceRollText.text = newValue.ToString();
    }
    // Called by OnValueChanged for moveCompleted
    private void UpdateMoveCompleted(bool prev, bool newValue)
    {
        if (newValue)
            diceRollText.text = "Roll!";
    }

    // RPCs
    [ClientRpc]
    private void EnterPieceServerRpc(int index, int clientIndex)
    {
        int teamId = Utility.RetrieveTeamId(clientIndex);
        Piece piece = playerPieces[clientIndex][index];
        int startPosition = paths[teamId][0];

        // 1. Move the piece @ the start
        board[startPosition].AddPiece(piece);
        piece.currentTile = startPosition;

        // 2. Are we killing any piece?
        Piece p = board[startPosition].GetFirstPiece();
        if (p != null && p.currentTeam != piece.currentTeam)
        {
            board[startPosition].RemovePiece(p);
            p.currentTile = -1;
            p.PositionClientRpc(-Vector3.one); // start position is set localy
        }

        moveCompleted = true;
        //moveCompleted.SetDirty(true);

        //ClientRpcParams clientRpcParams = new ClientRpcParams
        //{
        //    Send = new ClientRpcSendParams
        //    {
        //        TargetClientIds = new ulong[] { clientIndex }
        //    }
        //};
        DisableInteractionClientRpc();//clientRpcParams);
    }
    [ClientRpc]
    private void MovePieceClientRpc(int indexPos) //ClientRpcParams serverRpcParams = default)
    {
        int clientId = 1;// serverRpcParams.Receive.SenderClientId; Izvuci nekako clientId
        Piece piece = playerPieces[clientId][indexPos];

        // 0. Are we in play?
        if (piece.currentTile == -1)
        {
            EnterPieceServerRpc(indexPos, clientId);
            return;
        }

        // 1. Find the index in the current team's path
        int matchingIndex = FindIndexInPath(piece);
        int targetTile = paths[(int)piece.currentTeam][matchingIndex + currentDiceRoll];
        int previousPosition = piece.currentTile;

        // 2. Are we killing any piece?
        Piece p = board[targetTile].GetFirstPiece();
        if (p != null && p.currentTeam != piece.currentTeam)
        {
            board[targetTile].RemovePiece(p);
            p.currentTile = -1;
            p.PositionClientRpc(-Vector3.one); // start position is set localy
        }

        // 3. Move the piece there
        piece.currentTile = targetTile;
        board[targetTile].AddPiece(piece);
        board[previousPosition].RemovePiece(piece);

        moveCompleted = true;
        //moveCompleted.SetDirty(true);
        //ClientRpcParams clientRpcParams = new ClientRpcParams
        //{
        //    Send = new ClientRpcSendParams
        //    {
        //        TargetClientIds = new ulong[] { clientId }
        //    }
        //};
        DisableInteractionClientRpc();//clientRpcParams);

        // 4. Are you winning?
        if (finalTiles.Contains(targetTile))
        {
            if (board[targetTile].PieceCount() == 4)
            {
                //NetworkServer.connections.TryGetValue()
                //NetworkManager.singleton.ConnectedClients.TryGetValue(serverRpcParams.Receive.SenderClientId, out var networkedClient);
                string playerName = NetworkClient.connection.identity.GetComponent<PlayerController>().playerName;
                AddWinnerToTheListClientRpc(playerName);
                playerCompleted[NetworkClient.connection.connectionId] = true;

                int i = 0;
                foreach (KeyValuePair<int, bool> pd in playerCompleted)
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

        if (!canRollAgain)
            NextTurn();
    }
    [ClientRpc]
    private void OnPlayerReadyClientRpc(int clientId)
    {
        // Called when the player is done loading his scene, and his network is initialized
        playerReady[clientId] = true;
        // If all players are ready, spawn all the networked pieces
        if (!playerReady.ContainsValue(false))
            SpawnAllPlayers();
    }
    [ClientRpc]
    public void DiceRollServerRpc(int clientId, int forceDice)
    {
        forceDice = -1;
        // If we're either on the first turn, or we rolled a 6 
        if ((rollCountThisTurn == 0 || canRollAgain) && clientId == currentTurn && moveCompleted)
        {
            // Actually roll
            int rv = (forceDice == -1) ? Random.Range(1, 7) : forceDice;
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
            currentDiceRoll = rv;
            //currentDiceRoll.SetDirty(true);

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
                moveCompleted = false;
                //moveCompleted.SetDirty(true);
                //ClientRpcParams clientRpcParams = new ClientRpcParams
                //{
                //    Send = new ClientRpcSendParams
                //    {
                //        TargetClientIds = new ulong[] { clientId }
                //    }
                //};
                EnableInteractionClientRpc(pieceYouCanMove); //clientRpcParams);
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
    public void EnableInteractionClientRpc(bool[] pieceYouCanMove)
    {
        // Sent to current turn client, and activate the pieces that they can move
        if (localPieces[0] == null)
            localPieces = FindLocalPieces();

        for (int i = 0; i < pieceYouCanMove.Length; i++)
            if (localPieces[i] != null && pieceYouCanMove[i])
                localPieces[i].EnableInteraction();
    }
    [ClientRpc]
    public void DisableInteractionClientRpc()
    {
        // Sent to current turn client, turn off all pieces
        for (int i = 0; i < localPieces.Length; i++)
            localPieces[i].DisableInteraction();
    }

}
