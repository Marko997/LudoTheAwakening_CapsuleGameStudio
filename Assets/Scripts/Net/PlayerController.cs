using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : NetworkBehaviour
{
    private bool networkStarted = true;

    //Networked fields
    public NetworkVariable<CustomNetworkVariables.NetworkString> playerName = new NetworkVariable<CustomNetworkVariables.NetworkString>("PlayerName",NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    public NetworkVariable<Color> playerColor = new NetworkVariable<Color>(Color.black, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    //Fields
    private TextMeshProUGUI playerNameLabel;
    private GameObject myPLayerListItem;

    public List<string> deckStrings = new List<string>(4);
    public List<GameObject> pawnContainer = new List<GameObject>(4); //napraviti objekat koji ima sve pijune i singleton je tako da se ovde ubace samo oni koje player ima

    private void Start()
    { 
        deckStrings.Add("Spearman");
        deckStrings.Add("Spearman");
        deckStrings.Add("Spearman");
        deckStrings.Add("Spearman");

        
    }

    public void Update()
    {
        if (networkStarted)
        {
            RegisterEvents();

            myPLayerListItem = Instantiate(LobbyScene.Instance.playerListItemPrefab, Vector3.zero, Quaternion.identity);
            myPLayerListItem.transform.SetParent(LobbyScene.Instance.playerListContainer, false);

            playerNameLabel = myPLayerListItem.GetComponentInChildren<TextMeshProUGUI>();

            if (IsOwner)
            {
                if (PlayerPrefs.GetString("NAME") == "")
                {
                    playerName.Value = UnityEngine.Random.Range(1000, 9999).ToString();
                }
                else
                {
                    playerName.Value = PlayerPrefs.GetString("NAME");
                }
                
                playerColor.Value = new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
            }
            else
            {
                playerNameLabel.text = playerName.Value;
            }

            networkStarted = false;
        }
    }

    public override void OnDestroy()
    {
        Destroy(myPLayerListItem);
        UnregisterEvents();
    }

    public void ChangeName(string newName)
    {
        if (IsOwner)
        {
            playerName.Value = newName;
        }
    }

    public void RegisterEvents()
    {
        playerName.OnValueChanged += OnPlayerNameChange;
        //playerColor.OnValueChanged += OnColorChange;
    }
    private void UnregisterEvents()
    {
        playerName.OnValueChanged -= OnPlayerNameChange;
        //playerColor.OnValueChanged -= OnColorChange;
    }
    private void OnPlayerNameChange(CustomNetworkVariables.NetworkString previousValue, CustomNetworkVariables.NetworkString newValue)
    {
        playerNameLabel.text = playerName.Value;
    }
    //private void OnColorChange(Color oldValue, Color newValue) => _renderer.material.color = newValue;

}
