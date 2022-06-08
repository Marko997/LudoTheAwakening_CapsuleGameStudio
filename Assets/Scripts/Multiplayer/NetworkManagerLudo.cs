using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class NetworkManagerLudo : NetworkManager
{
    [Header("Player Managers")]
    public GameObject playerRoomPrefab;
    public GameObject playerGamePrefab;
    public int playerCounter = 0;

    [SerializeField] private GameObject playerSpawnSystem = null;

    private MpGameManager _mpGameManager;
    private MpGameManager mpGameManager
    {
        get
        {
            if (_mpGameManager == null) _mpGameManager = GameObject.Find("#GAME_MANAGER").GetComponent<MpGameManager>();

            return _mpGameManager;
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        GameObject playerNewObject;

        switch (SceneManager.GetActiveScene().name)
        {
            case "MainScene":
                playerNewObject = Instantiate(playerRoomPrefab);
                break;
            case "MPgameScene":
                playerNewObject = Instantiate(playerGamePrefab);
                break;
            default:
                return;
        }

        GameObject playerTmp = conn.identity.gameObject;
        NetworkServer.ReplacePlayerForConnection(conn, playerNewObject);
        NetworkServer.Destroy(playerTmp);

        if(SceneManager.GetActiveScene().name == "MPgameScene")
        {
            mpGameManager.InsertPlayerData(playerNewObject.GetComponent<MP_playerEntity>());

            mpGameManager.SpawnSelectorRPC();
            playerCounter++;
            if(playerCounter == 1)
            {
                mpGameManager.OnStartGame();
            }
        }
    }
    

}