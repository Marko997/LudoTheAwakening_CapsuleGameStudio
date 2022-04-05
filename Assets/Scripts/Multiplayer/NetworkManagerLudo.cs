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
    }

}