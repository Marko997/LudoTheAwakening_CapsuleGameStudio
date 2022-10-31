using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class LobbyScene : MonoSingleton<LobbyScene>
{
    [SerializeField] public GameObject playerListItemPrefab;
    [SerializeField] public Transform playerListContainer;
    [SerializeField] public TMP_InputField playerNameInput;

    public void OnMainHostButton()
    {
        NetworkManager.singleton.StartHost();
    }

    public void OnMainConnectButton()
    {
        NetworkManager.singleton.StartClient();
    }

    public void OnLobbyBackButton()
    {
        NetworkManager.Shutdown();
    }

    public void OnLobbyStartButton()
    {
        //Change scene
    }


}
