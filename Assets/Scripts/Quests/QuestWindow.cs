using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestWindow : MonoBehaviour
{
    //fields for all the elements
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private GameObject goalPrefab;
    [SerializeField] private Transform goalsContent;
    [SerializeField] private TMP_Text xpText;
    [SerializeField] private TMP_Text coinsText;

    public void Initialize(Quest quest)
    {
        titleText.text = quest.Information.Name;
        descriptionText.text = quest.Information.Description;

        foreach(var goal in quest.Goals)
        {
            GameObject goalObject = Instantiate(goalPrefab, goalsContent);
            goalObject.transform.Find("Text").GetComponent<TMP_Text>().text = goal.GetDescription();

            GameObject countObj = goalObject.transform.Find("Count").gameObject;
            GameObject sliderObj = goalObject.transform.Find("Slider").gameObject;

            GameObject skipObj = goalObject.transform.Find("Skip").gameObject;

            if (goal.Completed)
            {
                countObj.SetActive(false);
                skipObj.SetActive(false);
                sliderObj.SetActive(false);
                goalObject.transform.Find("Text").GetComponent<TMP_Text>().text = "Reward granted!";
                //goalObject.transform.transform.parent.GetComponent<Image>().sprite = GameAssets.instance.completedQuestBackground;
                //goalObject.transform.Find("Done").gameObject.SetActive(true);
            }
            else
            {
                countObj.GetComponent<TMP_Text>().text = goal.CurrentAmount + "/" + goal.RequiredAmount;
                sliderObj.GetComponent<Slider>().maxValue = goal.RequiredAmount;
                sliderObj.GetComponent<Slider>().value = goal.CurrentAmount;

                skipObj.GetComponent<Button>().onClick.AddListener(delegate
                {
                    goal.Skip();

                    countObj.SetActive(false);
                    skipObj.SetActive(false);
                    sliderObj.SetActive(false);

                    goalObject.transform.Find("Text").GetComponent<TMP_Text>().text = "Reward granted!";
                    //goalObject.transform.transform.parent.GetComponent<Image>().sprite = GameAssets.instance.completedQuestBackground;

                    //goalObject.transform.Find("Done").gameObject.SetActive(true);

                });
            }
        }
        //xpText.text = quest.Reward.XP.ToString();
        coinsText.text = quest.Reward.Currency.ToString();
    }

    public void CloseWindows()
    {
        gameObject.SetActive(false);

        for (int i = 0; i < goalsContent.childCount; i++)
        {
            Destroy(goalsContent.GetChild(i).gameObject);
        }
    }
}
