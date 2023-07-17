using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public List<Tutorial> Tutorials = new List<Tutorial>();

    //UI
    public TMP_Text expText;
    public GameObject popUp;
    public Button buttonToPress;

    private Tutorial currentTutorial;
    private Transform previousButtonParent;

    private static TutorialManager instance;
    public static TutorialManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<TutorialManager>();
            }
            if(instance == null)
            {
                Debug.Log("There is no tutorial manager!");
            }
            return instance;
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        //turn on when tutorial creating is finished
        //if(PlayerPrefs.GetString("TUTORIAL") == "FINISHED")
        //{
        //    expText.gameObject.SetActive(false);
        //    return;
        //}
        SetNextTutorial(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTutorial)
        {
            currentTutorial.CheckIfHappening();
        }
    }

    public Tutorial GetTutorialByOrder(int order)
    {
        for (int i = 0; i < Tutorials.Count; i++)
        {
            if (Tutorials[i].Order == order)
            {
                return Tutorials[i];
            }
        }
        return null;
    }

    public void SetNextTutorial(int currentOrder)
    {
        currentTutorial = GetTutorialByOrder(currentOrder);

        if (!currentTutorial)
        {
            CompletedAllTutorials();
            return;
        }
        
        expText.text = currentTutorial.Explanation;
        popUp = currentTutorial.popUp;
        popUp.SetActive(true);
        buttonToPress = currentTutorial.buttonToPress;
        previousButtonParent = buttonToPress.transform.parent;
        buttonToPress.transform.SetParent(popUp.transform,true);
    }

    public void CompletedTutorial()
    {
        buttonToPress.transform.SetParent(previousButtonParent, true);
        popUp.SetActive(false);
        SetNextTutorial(currentTutorial.Order++);
    }

    public void CompletedAllTutorials()
    {
        PlayerPrefs.SetString("TUTORIAL", "FINISHED");
        expText.text = "You have completed all the tutorials, hoerah!!";
    }
}
