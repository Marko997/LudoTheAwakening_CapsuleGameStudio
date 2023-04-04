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
    //bool isTurningOffRunning = false;

    NetworkVariable<int> chatEmojiIndex = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    NetworkVariable<int> chatEmojiIndex1 = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    NetworkVariable<int> chatEmojiIndex2 = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    NetworkVariable<int> chatEmojiIndex3 = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [SerializeField] private Sprite[] emojis;
    [SerializeField] private Image[] emojisInChatMenu;

    private Coroutine timerCoroutine;

    void Start()
    {
        RegisterEvents();

        chatEmojiDisplay.SetActive(false);
        chatButton.onClick.AddListener(OnChatButtonPressed);

        for (int i = 0; i < 4; i++)
        {
            emojisInChatMenu[i].sprite = emojis[i];
        }
            switch (NetworkManager.Singleton.LocalClientId)
            {
                case 0:
                int childCount = chatEmojiDisplay.transform.childCount;

                //for (int i = 0; i < 4; i++) //UNITY BUG ALWAYS SENDS 4
                //{
                //    chatEmojiDisplay.transform.GetChild(i).gameObject.GetComponent<Button>().onClick.AddListener(() => ShowRedEmoji(i));

                //}
                    chatEmojiDisplay.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(() => ShowRedEmoji(0));
                    chatEmojiDisplay.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(() => ShowRedEmoji(1));
                    chatEmojiDisplay.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(() => ShowRedEmoji(2));
                    chatEmojiDisplay.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(() => ShowRedEmoji(3));
                    break;

                case 1:
                    chatEmojiDisplay.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(() => ShowBlueEmoji(0));
                    chatEmojiDisplay.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(() => ShowBlueEmoji(1));
                    chatEmojiDisplay.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(() => ShowBlueEmoji(2));
                    chatEmojiDisplay.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(() => ShowBlueEmoji(3));
                    break;
                case 2:
                    chatEmojiDisplay.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(() => ShowYellowEmoji(0));
                    chatEmojiDisplay.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(() => ShowYellowEmoji(1));
                    chatEmojiDisplay.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(() => ShowYellowEmoji(2));
                    chatEmojiDisplay.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(() => ShowYellowEmoji(3));
                    break;
                case 3:
                    chatEmojiDisplay.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(() => ShowGreenEmoji(0));
                    chatEmojiDisplay.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(() => ShowGreenEmoji(1));
                    chatEmojiDisplay.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(() => ShowGreenEmoji(2));
                    chatEmojiDisplay.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(() => ShowGreenEmoji(3));
                    break;
                default:
                    break;
            }
    }

    void RegisterEvents()
    {
        chatEmojiIndex.OnValueChanged += UpdateRedChatIconOnOthers;
        chatEmojiIndex1.OnValueChanged += UpdateBlueChatIconOnOthers;
        chatEmojiIndex2.OnValueChanged += UpdateYellowChatIconOnOthers;
        chatEmojiIndex3.OnValueChanged += UpdateGreenChatIconOnOthers;
    }

    void UnregisterEvents()
    {
        chatEmojiIndex.OnValueChanged -= UpdateRedChatIconOnOthers;
        chatEmojiIndex1.OnValueChanged -= UpdateBlueChatIconOnOthers;
        chatEmojiIndex2.OnValueChanged -= UpdateYellowChatIconOnOthers;
        chatEmojiIndex3.OnValueChanged -= UpdateGreenChatIconOnOthers;
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

    void ShowRedEmoji(int emojiIndex)
    {
        //Update emoji on self
        allChatDisplayIcons[0].gameObject.SetActive(true);
        allChatDisplayIcons[0].sprite = emojis[emojiIndex];

        //Update emoji on others
        UpdateEmojiInArrayServerRpc(0, emojiIndex);
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine); // Stop the old coroutine
        }
        timerCoroutine = StartCoroutine(TurnOffAfterDelay(allChatDisplayIcons[0].gameObject)); // Start the new coroutine
    }

    void ShowBlueEmoji(int emojiIndex)
    {
        allChatDisplayIcons[0].gameObject.SetActive(true);
        allChatDisplayIcons[0].sprite = emojis[emojiIndex];

        UpdateEmojiInArrayServerRpc(1, emojiIndex);

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine); // Stop the old coroutine
        }
        timerCoroutine = StartCoroutine(TurnOffAfterDelay(allChatDisplayIcons[0].gameObject)); // Start the new coroutine

        //StartCoroutine(TurnOffAfterDelay(allChatDisplayIcons[0].gameObject));
    }
    void ShowYellowEmoji(int emojiIndex)
    {
        allChatDisplayIcons[0].gameObject.SetActive(true);
        allChatDisplayIcons[0].sprite = emojis[emojiIndex];

        UpdateEmojiInArrayServerRpc(2, emojiIndex);

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine); // Stop the old coroutine
        }
        timerCoroutine = StartCoroutine(TurnOffAfterDelay(allChatDisplayIcons[0].gameObject)); // Start the new coroutine

        //StartCoroutine(TurnOffAfterDelay(allChatDisplayIcons[0].gameObject));
    }
    void ShowGreenEmoji(int emojiIndex)
    {
        allChatDisplayIcons[0].gameObject.SetActive(true);
        allChatDisplayIcons[0].sprite = emojis[emojiIndex];

        UpdateEmojiInArrayServerRpc(3, emojiIndex);

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine); // Stop the old coroutine
        }
        timerCoroutine = StartCoroutine(TurnOffAfterDelay(allChatDisplayIcons[0].gameObject)); // Start the new coroutine

        //StartCoroutine(TurnOffAfterDelay(allChatDisplayIcons[0].gameObject));
    }

    [ServerRpc(RequireOwnership =false)]
    void UpdateEmojiInArrayServerRpc(int playerIndex, int emojiIndex)
    {
        switch (playerIndex)
        {
            case 0:
                chatEmojiIndex.Value = emojiIndex;
                break;
            case 1:
                chatEmojiIndex1.Value = emojiIndex;
                break;
            case 2:
                chatEmojiIndex2.Value = emojiIndex;
                break;
            case 3:
                chatEmojiIndex3.Value = emojiIndex;
                break;
            default:
                break;
        }
    }

    void UpdateRedChatIconOnOthers(int prev, int newValue)
    {
        if(NetworkManager.LocalClientId == 0) { return; }

        allChatDisplayIcons[NetworkManager.Singleton.LocalClientId].gameObject.SetActive(true);
        allChatDisplayIcons[NetworkManager.Singleton.LocalClientId].sprite = emojis[newValue];

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine); // Stop the old coroutine
        }
        timerCoroutine = StartCoroutine(TurnOffAfterDelay(allChatDisplayIcons[NetworkManager.Singleton.LocalClientId].gameObject)); // Start the new coroutine

        //StartCoroutine(TurnOffAfterDelay(allChatDisplayIcons[NetworkManager.Singleton.LocalClientId].gameObject));
    }

    IEnumerator TurnOffAfterDelay(GameObject icon)
    {
        yield return new WaitForSeconds(5); // Wait for the specified time
        icon.SetActive(false);
    }

    void UpdateBlueChatIconOnOthers(int prev, int newValue)
    {
        if (NetworkManager.LocalClientId == 1) { return; }

        allChatDisplayIcons[1].gameObject.SetActive(true);
        allChatDisplayIcons[1].sprite = emojis[newValue];

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine); // Stop the old coroutine
        }
        timerCoroutine = StartCoroutine(TurnOffAfterDelay(allChatDisplayIcons[1].gameObject)); // Start the new coroutine
    }
    void UpdateYellowChatIconOnOthers(int prev, int newValue)
    {
        if (NetworkManager.LocalClientId == 2) { return; }

        allChatDisplayIcons[2].gameObject.SetActive(true);
        allChatDisplayIcons[2].sprite = emojis[newValue];

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine); // Stop the old coroutine
        }
        timerCoroutine = StartCoroutine(TurnOffAfterDelay(allChatDisplayIcons[2].gameObject)); // Start the new coroutine
    }
    void UpdateGreenChatIconOnOthers(int prev, int newValue)
    {
        if (NetworkManager.LocalClientId == 3) { return; }

        allChatDisplayIcons[3].gameObject.SetActive(true);
        allChatDisplayIcons[3].sprite = emojis[newValue];

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine); // Stop the old coroutine
        }
        timerCoroutine = StartCoroutine(TurnOffAfterDelay(allChatDisplayIcons[3].gameObject)); // Start the new coroutine
    }

    private new void OnDestroy()
    {
        UnregisterEvents();
    }
}
