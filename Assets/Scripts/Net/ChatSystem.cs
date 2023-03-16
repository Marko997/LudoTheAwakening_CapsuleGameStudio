using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class ChatSystem : NetworkBehaviour
{
    [SerializeField] private Button chatButton;
    [SerializeField] private GameObject chatEmojiDisplay;

    [SerializeField] private Image[] allChatDisplayIcons;


    bool isChatOppened;

    NetworkVariable<int> chatEmojiIndex = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);



    // Start is called before the first frame update
    void Start()
    {
        RegisterEvents();

        //rotiranje red playera sa playerom ciji je screen
        if(NetworkManager.Singleton.LocalClientId > 0)
        {
            var temp = allChatDisplayIcons[0];
            allChatDisplayIcons[0] = allChatDisplayIcons[NetworkManager.Singleton.LocalClientId];
            allChatDisplayIcons[NetworkManager.Singleton.LocalClientId] = temp;
        }

        chatEmojiDisplay.SetActive(false);
        chatButton.onClick.AddListener(OnChatButtonPressed);

        foreach (Transform item in chatEmojiDisplay.transform)
        {
            item.gameObject.GetComponentInChildren<Button>().onClick.AddListener(ShowEmojiToOtherPlayersServerRpc);
        }

        //chatEmojiDisplay.GetComponentInChildren<Button>().onClick.AddListener(ShowEmojiToOtherPlayers);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RegisterEvents()
    {
        chatEmojiIndex.OnValueChanged += UpdateChatIcon;
    }

    void UnregisterEvents()
    {
        chatEmojiIndex.OnValueChanged -= UpdateChatIcon;
    }

    void OnChatButtonPressed()
    {
        if (isChatOppened)
        {
            chatEmojiDisplay.SetActive(false);
            isChatOppened = false;
            return;
        }

        chatEmojiDisplay.SetActive(true);
        isChatOppened = true;
    }

    [ServerRpc(RequireOwnership = false)]
    void ShowEmojiToOtherPlayersServerRpc()
    {
        chatEmojiIndex.Value = Random.Range(1,5);
    }

    void UpdateChatIcon(int prev, int newValue)
    {
        //Azuriraj tom klijentu prvi element niza


        //Azuriraj ostalim klijentima ono gde se on nalazi
        for (int i = 0; i < 4; i++)
        {
            //allChatDisplayIcons[NetworkManager.Singleton.LocalClientId].color = Color.red;
            //update player 0 (RED) chat icon to other clients
            if ((i > 0 && i < 4) && (int)NetworkManager.Singleton.LocalClientId == i)
            {
                Debug.Log(i);
                allChatDisplayIcons[1].color = Color.red;
            }

            //update other clients chat icon on player 0 (RED)
            //if ((int)NetworkManager.Singleton.LocalClientId == 0 && i < 4)
            //{
            //allChatDisplayIcons[(int)currentTurn.Value].color = Color.red;
            //}
        }
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }
}
