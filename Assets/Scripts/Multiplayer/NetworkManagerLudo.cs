using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class NetworkManagerLudo : NetworkManager
{
    [Header("Player Managers")]
    public GameObject playerGamePrefab;
    public GameObject playerLobbyPrefab;
    

    public void ChangeSceneToMultiPlayer(List<PlayerLobbyManager> playerLobbyManagers)
    {
        foreach (var player in playerLobbyManagers)
        {
            var conn = player.connectionToClient;

            GameObject gamePlayerInstance = Instantiate(playerGamePrefab);

            NetworkServer.Destroy(conn.identity.gameObject);
            NetworkServer.ReplacePlayerForConnection(conn, gamePlayerInstance);

            base.ServerChangeScene("MPgameScene");
        }
    }
}