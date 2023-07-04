using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class BotChat : MonoBehaviour
{
    [SerializeField] private Button chatButton;
    [SerializeField] private GameObject chatEmojiDisplay;

    [SerializeField] private Image[] allChatDisplayIcons;

    bool isChatOppened;

    [SerializeField] private Sprite[] emojis;
    [SerializeField] private Image[] emojisInChatMenu;

    private Coroutine timerCoroutine;

    public float minDelay = 1.0f; // Minimum delay before showing emoji (in seconds)
    public float maxDelay = 3.0f; // Maximum delay before showing emoji (in seconds)


    void Start()
    {
        chatEmojiDisplay.SetActive(false);
        chatButton.onClick.AddListener(OnChatButtonPressed);

        for (int i = 0; i < 4; i++)
        {
            emojisInChatMenu[i].sprite = emojis[i];
        }

        StartCoroutine(RandomlyShowEmojis());
    }

    void OnChatButtonPressed()
    {
        SoundManager.PlayOneSound(SoundManager.Sound.ButtonClick);
        if (isChatOppened)
        {
            chatEmojiDisplay.SetActive(false);
            isChatOppened = false;
            return;
        }

        chatEmojiDisplay.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(() => ShowEmoji(0));
        chatEmojiDisplay.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(() => ShowEmoji(1));
        chatEmojiDisplay.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(() => ShowEmoji(2));
        chatEmojiDisplay.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(() => ShowEmoji(3));

        chatEmojiDisplay.SetActive(true);
        isChatOppened = true;
    }

    void ShowEmoji(int emojiIndex)
    {
        SoundManager.PlayOneSound(SoundManager.Sound.ButtonClick);
        //Update emoji on self
        allChatDisplayIcons[0].gameObject.SetActive(true);
        allChatDisplayIcons[0].sprite = emojis[emojiIndex];

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine); // Stop the old coroutine
        }
        timerCoroutine = StartCoroutine(TurnOffAfterDelay(allChatDisplayIcons[0].gameObject)); // Start the new coroutine

        if (isChatOppened)
        {
            chatEmojiDisplay.SetActive(false);
            isChatOppened = false;
            return;
        }
    }

    IEnumerator TurnOffAfterDelay(GameObject icon)
    {
        yield return new WaitForSeconds(5); // Wait for the specified time
        icon.SetActive(false);
    }

    public void ShowBotEmoji(int emojiIndex)
    {
        //SoundManager.PlayOneSound(SoundManager.Sound.ButtonClick);

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine); // Stop the old coroutine
        }

        int botNumber = Random.Range(1, 3);

        // Start the new coroutine with a random delay
        float randomDelay = Random.Range(minDelay, maxDelay);
        timerCoroutine = StartCoroutine(ShowBotEmojiAfterDelay(emojiIndex, randomDelay, botNumber));
    }

    private IEnumerator ShowBotEmojiAfterDelay(int emojiIndex, float delay, int botNumber)
    {
        yield return new WaitForSeconds(delay);

        // Update emoji on self
        allChatDisplayIcons[botNumber].gameObject.SetActive(true);
        allChatDisplayIcons[botNumber].sprite = emojis[emojiIndex];

        // Turn off the emoji after a fixed duration
        StartCoroutine(TurnOffAfterDelay(allChatDisplayIcons[botNumber].gameObject));
    }

    private IEnumerator RandomlyShowEmojis()
    {
        while (true)
        {
            // Generate a random delay before showing the next emoji
            float randomDelay = Random.Range(minDelay, 10f);
            yield return new WaitForSeconds(randomDelay);

            // Generate a random emoji index
            int randomEmojiIndex = Random.Range(0, emojis.Length);

            // Show the emoji with the random index
            ShowBotEmoji(randomEmojiIndex);
        }
    }
}
