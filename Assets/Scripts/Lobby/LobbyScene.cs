using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;

public class LobbyScene : MonoSingleton<LobbyScene>
{
    //[SerializeField] private Animator animator;
    public static bool isGameLeft = false;
    //Lobby UI
    public Transform playerListContainer;
    public GameObject playerListItemPrefab;
    public TMP_InputField playerNameInput;

    public GameObject startButton;

    public TextMeshProUGUI joinCode;
    public TMP_InputField codeInput;

    private async void Start()
    {
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
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3); //this is for 4 players host + 3

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
        //animator.SetTrigger("Lobby");
    }

    public void OnConnectButtonPressed()
    {
        OnMainConnectButton(codeInput.text);
    }

    public async void OnMainConnectButton(string joinCode)
    {
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        }
        catch (RelayServiceException ex)
        {
            Debug.Log(ex);
        }

        NetworkManager.Singleton.StartClient();
        //animator.SetTrigger("Lobby");
    }

    //Lobby

    public void OnLobbyBackButton()
    {
        //animator.SetTrigger("Main");
        NetworkManager.Singleton.Shutdown();
    }
    public void OnLobbyStartButton()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("Game",UnityEngine.SceneManagement.LoadSceneMode.Single);
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
