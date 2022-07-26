using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ConnectToLobbyLTA : MonoBehaviour
{
    [SerializeField] private NetworkManagerLTA networkManager = null;
    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel = null;
    [SerializeField] private Button joinButton;

    private void OnEnable()
    {
        NetworkManagerLTA.OnClientConnected += HandleClientConnected;
        NetworkManagerLTA.OnClientDisconnected += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        NetworkManagerLTA.OnClientConnected -= HandleClientConnected;
        NetworkManagerLTA.OnClientDisconnected -= HandleClientDisconnected;
    }

    public void ConnectToLobby()
    {
        //string ipAddress = "to be added";
        //networkManager.networkAddress = ipAddress;

        networkManager.StartClient();

        joinButton.interactable = false;
    }

    private void HandleClientConnected()
    {
        joinButton.interactable = true;

        gameObject.SetActive(false);
        landingPagePanel.SetActive(false);
    }

    private void HandleClientDisconnected()
    {
        joinButton.interactable = true;
    }

}
