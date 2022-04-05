using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerLobbyManager : NetworkBehaviour
{
    [SyncVar(hook = nameof(HandlePlayerIsReadyChanged))]
    public bool playerIsReady = false;

    private LobbyManager _lobbyManager;
    private LobbyManager lobbyManager
    {
        get {
            if (_lobbyManager == null) _lobbyManager = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();

            return _lobbyManager;
        }
    }

    public void HandlePlayerIsReadyChanged(bool oldValue, bool newValue)
    {
        lobbyManager.UpdatePlayerStatus();
    }

    [Command]
    public void CmdTogglePlayerIsReady()
    {
        playerIsReady = !playerIsReady;
        lobbyManager.StartGameIfAllPlayersAreReady();
    }


    public override void OnStartClient()
    {
        lobbyManager.AddPlayerToLobby(this);
    }

    public override void OnStartServer()
    {
        lobbyManager.AddPlayerToLobby(this);
    }


    public override void OnStopClient()
    {
        lobbyManager.RemovePlayerFromLobby(this);
    }
}
