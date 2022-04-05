using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkGamePlayerLobby : NetworkBehaviour
{


    [SyncVar]
    private string displayName = "Loading";

    private MirrorServer room;

    private MirrorServer Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as MirrorServer;
        }

    }


    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);
        Room.GamePlayers.Add(this);
    }

    public override void OnStopClient()
    {
        Room.GamePlayers.Remove(this);
    }
    [Server]
   public void SetDisplayName(string displayName)
    {
        this.displayName = displayName;
    }
}
