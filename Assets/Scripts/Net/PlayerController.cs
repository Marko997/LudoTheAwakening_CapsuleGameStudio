using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;

public enum PlayerTypes
{
    CPU,
    HUMAN
}

public class PlayerController : NetworkBehaviour
{
    private bool networkStarted = true;

    //Networked fields
    public NetworkVariable<CustomNetworkVariables.NetworkString> playerName = new NetworkVariable<CustomNetworkVariables.NetworkString>("PlayerName",NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    public NetworkVariable<Color> playerColor = new NetworkVariable<Color>(Color.black, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> playerImageIndex = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    //Fields
    private TextMeshProUGUI playerNameLabel;
    private GameObject myPLayerListItem;

    public Image playerImage;

    public List<string> deckStrings = new List<string>(4);
    public List<GameObject> pawnContainer = new List<GameObject>(4); //napraviti objekat koji ima sve pijune i singleton je tako da se ovde ubace samo oni koje player ima
    public List<Sprite> allPlayerImages = new List<Sprite>();

    public PlayerTypes PlayerType;

    private void Start()
    {
        if (PlayerType == PlayerTypes.HUMAN)
        {
            if (PlayerPrefs.HasKey("Deck"))
            {
                string[] cardIDs = PlayerPrefs.GetString("Deck").Split(',');

                foreach (var cardID in cardIDs.Select((value, i) => new { i, value }))
                {
                    deckStrings.Add(cardID.value);
                }
            }
        }
    }

    public void Update()
    {
        //if (PlayerType == PlayerTypes.HUMAN)
        //{
            if (networkStarted)
            {
                RegisterEvents();

                myPLayerListItem = Instantiate(LobbyScene.Instance.playerListItemPrefab, Vector3.zero, Quaternion.identity);
                myPLayerListItem.transform.SetParent(LobbyScene.Instance.playerListContainer, false);

                playerNameLabel = myPLayerListItem.GetComponentInChildren<TextMeshProUGUI>();
                playerImage = myPLayerListItem.GetComponent<Image>();

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
                    if (PlayerPrefs.GetInt("IMAGE") == 0)
                    {
                        playerImageIndex.Value = UnityEngine.Random.Range(1, 4);
                    }
                    else
                    {
                        playerImageIndex.Value = PlayerPrefs.GetInt("IMAGE");
                    }
                }
                else
                {
                    playerNameLabel.text = playerName.Value;
                    playerImage.sprite = allPlayerImages[playerImageIndex.Value];
                }

            if(PlayerType == PlayerTypes.CPU)
            {
                playerName.Value = UnityEngine.Random.Range(1000, 9999).ToString();
                playerImageIndex.Value = UnityEngine.Random.Range(1, 4);
            }

                networkStarted = false;
            }
        //}
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
        playerImageIndex.OnValueChanged += OnPlayerImageChange;
    }
    private void UnregisterEvents()
    {
        playerName.OnValueChanged -= OnPlayerNameChange;
        playerImageIndex.OnValueChanged -= OnPlayerImageChange;
    }
    private void OnPlayerNameChange(CustomNetworkVariables.NetworkString previousValue, CustomNetworkVariables.NetworkString newValue)
    {
        playerNameLabel.text = playerName.Value;
    }

    private void OnPlayerImageChange(int previousValue, int newValue)
    {
        playerImage.sprite = Instantiate(allPlayerImages[playerImageIndex.Value]);
    }
}
