using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.Services.Lobbies;

public class LobbyScene : MonoSingleton<LobbyScene>
{
    //[SerializeField] private Animator animator;
    public static bool isGameLeft = false;
    //Lobby UI
    public Transform playerListContainer;
    public GameObject playerListItemPrefab;
    public TMP_InputField playerNameInput;

    public GameObject startButton;
    public GameObject connectButton;

    public TextMeshProUGUI joinCode;
    public TMP_InputField codeInput;

    public GameObject slider;
    public GameObject wrongCodeText;

    private bool bots;

    public PlayerController bot1;

    public static List<PlayerController> botsList = new List<PlayerController>();

    private async void Start()
    {
        SoundManager.Initialize();
        SoundManager.PlayOneSound(SoundManager.Sound.MainMenuBackground);

        bots = false;
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            //If not already logged, log the user in
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    private void Update()
    {
        if (!NetworkManager.Singleton.IsHost)
        {
            startButton.SetActive(false);
        }
        else
        {
            startButton.SetActive(true);
        }
        if(codeInput.text != null && codeInput.text.Length == 6)
        {
            connectButton.GetComponent<Button>().interactable = true;
        }

    }

    void Cleanup()
    {
        if (NetworkManager.Singleton != null)
        {
            Destroy(NetworkManager.Singleton.gameObject);
            isGameLeft = false;
        }
    }

    #region Buttons
    //Main
    public async void OnMainHostButton()
    {
        slider.SetActive(true);
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(4, "europe-central2"); //this is for 4 players host + 3

            string _joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log(_joinCode);
            joinCode.text = _joinCode;

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        }
        catch (RelayServiceException ex)
        {
            Debug.Log(ex);
        }

        NetworkManager.Singleton.StartHost();

        

        if (NetworkManager.Singleton.IsListening)
        {
            if (bots)
            {
                var bot = Instantiate(bot1);
                bot.GetComponent<NetworkObject>().Spawn();
                botsList.Add(bot);
            }
            slider.SetActive(false);
        }
    }

    public void OnConnectButtonPressed()
    {
        OnMainConnectButton(codeInput.text);
    }

    public async void OnMainConnectButton(string joinCode)
    {
        wrongCodeText.SetActive(false);
        slider.SetActive(true);
        codeInput.text = "";
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            
        }
        catch (RelayServiceException ex)
        {
            Debug.Log("Join code wrong");
            slider.SetActive(false);
            wrongCodeText.SetActive(true);

            throw;
        }

        NetworkManager.Singleton.StartClient();

        NetworkManager.Singleton.OnClientConnectedCallback += ClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += ClientDisConnected;
            
    }


    private void ClientConnected(ulong clientId)
    {
        slider.SetActive(false);
        joinCode.gameObject.SetActive(false);
    }
    private void ClientDisConnected(ulong clientId)
    {
        NetworkManager.Singleton.Shutdown();
    }

    //Lobby

    public void OnLobbyBackButton()
    {
        NetworkManager.Singleton.Shutdown();
    }


    [ServerRpc]
    public void DisconnectClientFromServerServerRpc(ulong id)
    {
        NetworkManager.Singleton.DisconnectClient(id);
    }

    public void OnLobbyStartButton()
    {
        SoundManager.PlayOneSound(SoundManager.Sound.ButtonClick);
        if (!bots)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("Game", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

        NetworkManager.Singleton.SceneManager.LoadScene("BotsGame", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
    public void OnLobbySubmitNameChange()
    {
        string newName = playerNameInput.text;
        if(NetworkManager.Singleton.ConnectedClients.TryGetValue(NetworkManager.Singleton.LocalClientId, out var networkedClient))
        {
            var player = networkedClient.PlayerObject.GetComponent<PlayerController>();
            if (player)
            {
                player.ChangeName(newName);
            }
        }
    }

    #endregion
}
