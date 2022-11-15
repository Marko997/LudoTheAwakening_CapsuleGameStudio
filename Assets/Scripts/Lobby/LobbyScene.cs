using UnityEngine;
using Unity.Netcode;
using TMPro;

public class LobbyScene : MonoSingleton<LobbyScene>
{
    //[SerializeField] private Animator animator;

    //Lobby UI
    public Transform playerListContainer;
    public GameObject playerListItemPrefab;
    public TMP_InputField playerNameInput;

    #region Buttons
    //Main
    public void OnMainHostButton()
    {
        NetworkManager.Singleton.StartHost();
        //animator.SetTrigger("Lobby");
    }
    public void OnMainConnectButton()
    {
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
