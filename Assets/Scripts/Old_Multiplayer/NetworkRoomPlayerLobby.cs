using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkRoomPlayerLobby : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject lobbyUI = null;
    [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];
    [SerializeField] private TextMeshProUGUI[] playerReadyTexts;
    [SerializeField] private Button startGameButton = null;

    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string displayName = "Loading";
    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool IsReady = false;

    private bool isLeader;

    public bool IsLeader
    {
        set {
            isLeader = value;
            //startGameButton.gameObject.SetActive(value);
        }
    }

    private MirrorServer room;

    private MirrorServer Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as MirrorServer;
        }

    }

    public override void OnStartAuthority()
    {
        //CmdSetDisplayName(PlayerNameInput.DisplayName);
        lobbyUI.SetActive(true);
    }

    public override void OnStartClient()
    {
        Debug.Log("OnStartClient");
        Room.RoomPlayers.Add(this);
        UpdateDisplay();
    }

    public override void OnStopClient()
    {
        Room.RoomPlayers.Remove(this);
        UpdateDisplay();
    }

    public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();
    public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();

    public void UpdateDisplay()
    {
        if (!isLocalPlayer)
        {
            foreach(var player in Room.RoomPlayers)
            {
                if (player.hasAuthority)
                {
                    player.UpdateDisplay();
                    break;
                }
            }
            return;
        }
        //for (int i = 0; i < playerNameTexts.Length; i++)
        //{
        //    playerNameTexts[i].text = "Waiting for player...";
        //    playerReadyTexts[i].text = string.Empty;
        //}
        for (int i = 0; i < Room.RoomPlayers.Count; i++)
        {
            //playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
            playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady ?
                "<color=green>Ready</color>" :
                "<color=red>Not ready</color>"; 
        }
    }

    public void HandleReadyToStart(bool readyToStart)
    {
        if (!isLeader) { return; }
        Debug.Log("show button "+isLeader);
        startGameButton.interactable = readyToStart;
        Debug.Log(readyToStart);
        startGameButton.gameObject.SetActive(readyToStart);
    }

    [Command]
    private void CmdSetDisplayName(string _displayName)
    {
        displayName = _displayName;
    }

    [Command]
    public void CmdReadyUp()
    {
        IsReady = !IsReady;
        Room.NotifyPlayersOfReadyState(); //TODO: Radi samo kada je Host pokrenut, ne server only ... Da li je player host ili se hostuje na nekom serveru
    }

    [Command]
    public void CmdStartGame()
    {
        if(Room.RoomPlayers[0].connectionToClient != connectionToClient) { return; }

        Room.StartGame();
    }
}
