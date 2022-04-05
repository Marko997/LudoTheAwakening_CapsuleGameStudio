using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class LobbyManager : NetworkBehaviour
{
    [SyncVar]
    public int numberOfMaxPlayersInLobby;

    public TextMeshProUGUI[] playerListReadyTexts;

    private List<PlayerLobbyManager> playerList = new List<PlayerLobbyManager>();

    public void AddPlayerToLobby(PlayerLobbyManager player)
    {
        //TODO: Dodati logiku da se mogu samo 4 igraca konektovati, to treba server da regulise...

        playerList.Add(player);
        UpdatePlayerStatus();
    }


    public void RemovePlayerFromLobby(PlayerLobbyManager player)
    {
        playerList.Remove(player);
        UpdatePlayerStatus();
    }


    public void UpdatePlayerStatus()
    {

        if (isServer) return;

        for (int i = 0; i < numberOfMaxPlayersInLobby; i++)
        {

            if(playerList.Count > i)
            {

                playerListReadyTexts[i].text = playerList[i].playerIsReady ?
                "<color=green>Ready</color>" :
                "<color=red>Not ready</color>";
                
            }
            else
            {

                playerListReadyTexts[i].text = "Connecting...";

            }

        }

    }


    public void StartGameIfAllPlayersAreReady()
    {
        if (playerList.Count != numberOfMaxPlayersInLobby) return;

        foreach (var player in playerList)
        {
            if (!player.playerIsReady) return;
        }
        NetworkManagerLudo networkManagerLudo = GameObject.Find("NetworkManagerLudo").GetComponent<NetworkManagerLudo>();
        networkManagerLudo.ServerChangeScene("MPgameScene");


    }


    public void ReadyButtonClicked()
    {

        PlayerLobbyManager playerLobbyManager = NetworkClient.localPlayer.gameObject.GetComponent<PlayerLobbyManager>();
        playerLobbyManager.CmdTogglePlayerIsReady();

    }
}
