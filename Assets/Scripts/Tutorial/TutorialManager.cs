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
    public GameObject tutorialBackground;
    public GameObject pawnHolder;
    public GameObject popUp;
    public GameObject buttonToPress;

    public GameObject finalBackgroud;

    public Tutorial currentTutorial;
    private Transform previousButtonParent;
    private Transform previousExpTextParent;

    public GameObject deckCloseButton;

    public bool isAllTutorialsCompleted = false;

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
        //PlayerPrefs.DeleteKey("TUTORIAL");
        //turn on when tutorial creating is finished
        if (PlayerPrefs.GetString("TUTORIAL") == "FINISHED")
        {
            expText.gameObject.SetActive(false);
            tutorialBackground.SetActive(false);
            pawnHolder.SetActive(true);
            return;
        }
        else
        {
            expText.gameObject.SetActive(true);
            tutorialBackground.SetActive(true);
            pawnHolder.SetActive(false);
        }
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

        if(currentTutorial.Order == 4 || currentTutorial.Order == 5) //turn off deck close button, so player don't break the tutorial
        {
            deckCloseButton.SetActive(false);
        }
        else
        {
            deckCloseButton.SetActive(true);
        }

        expText.text = currentTutorial.Explanation;
        previousExpTextParent = expText.transform.parent;

        popUp = currentTutorial.popUp;
        popUp.SetActive(true);
        expText.transform.SetParent(popUp.transform.GetChild(0), false);
        expText.transform.localPosition = new Vector3(0f,0f,0f);

        //buttonToPress = null;
        buttonToPress = currentTutorial.buttonToPress;
        previousButtonParent = buttonToPress.transform.parent;

        if(currentTutorial.Order == 4){ return;} //We dont need to change button parent for this tutorial
        if (currentTutorial.Order == 5) { return; } //We dont need to change button parent for this tutorial
        buttonToPress.transform.SetParent(popUp.transform,true);
    }

    public void CompletedTutorial()
    {
        //ADD check for order if that tutId needs hero panel turn panel on or off
        if(currentTutorial.Order < 4 || currentTutorial.Order > 5) //Since we didn't move a button to new parent no need to return it back
        {
            buttonToPress.transform.SetParent(previousButtonParent, true); 
        }
        buttonToPress.GetComponent<CustomTutorialButton>().isClicked = false;
        expText.transform.SetParent(previousExpTextParent, true);
        popUp.SetActive(false);
        Debug.Log(currentTutorial.Order + 1);
        SetNextTutorial(currentTutorial.Order+1);
        
    }

    public void CompletedAllTutorials()
    {
        PlayerPrefs.SetString("TUTORIAL", "FINISHED");
        StartCoroutine(TurnOffTutorial());
    }

    IEnumerator TurnOffTutorial()
    {
        finalBackgroud.SetActive(true);
        expText.transform.SetParent(finalBackgroud.transform, false);
        expText.transform.localPosition = new Vector3(0f, 0f, 0f);

        expText.text = "You have completed all the tutorials, hoerah!!";
        pawnHolder.SetActive(true);
        isAllTutorialsCompleted = true;
        yield return new WaitForSeconds(2f);

        finalBackgroud.SetActive(false);
    }
}
