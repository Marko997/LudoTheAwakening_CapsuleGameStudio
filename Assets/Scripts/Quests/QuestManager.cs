using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private GameObject questPrefab;
    [SerializeField] private Transform questContent;
    [SerializeField] private GameObject questHolder;

    public List<Quest> CurrentQuests;

    private void Awake()
    {
        foreach (var quest in CurrentQuests)
        {
            quest.Initialize();
            quest.QuestCompleted.AddListener(OnQuestCompleted);

            GameObject questObject = Instantiate(questPrefab, questContent);
            questObject.transform.Find("Icon").GetComponent<Image>().sprite = quest.Information.Icon;

            questObject.GetComponent<Button>().onClick.AddListener(delegate
            {
                questHolder.GetComponent<QuestWindow>().Initialize(quest);
                questHolder.SetActive(true);
	        });
        }
    }

    public void Play(string playName)
    {
        EventManager.Instance.QueueEvent(new PlayGameEvent(playName));
    }

    private void OnQuestCompleted(Quest quest)
    {
        questContent.GetChild(CurrentQuests.IndexOf(quest)).Find("Checkmark").gameObject.SetActive(true);
    }
}
