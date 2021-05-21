using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PawnCardManager : MonoBehaviour
{
    public int currentIndex;

	public PawnTemplate[] pawnCards;

    public PawnRotatorY displayedPawn;

    [Header("Cards UI")]
    public Text cardNameText;
    public Image cardSpriteImage;
    public Text cardHisText;
    public Text cardDesText;
    public Text cardLevelText;

    [SerializeField] private int[] levelNeededExp = new int[] { 10, 30, 60 };

    [Header("CharacetrInfo Slots")]
    public GameObject[] slots;

    public Image SelectButton;
    public Image historyImage;

    public Text numberOfHeroes;
    
    //More to add later


	void Start()
    {
        currentIndex = 0;
        //historyImage.gameObject.SetActive(false);

        //DisplayInfo();
        DisplayCharacters();
        DisplayCharacter();
    }

    
    void Update()
    {
        if(displayedPawn.pawnHolder == null)
        {
            DisplayCharacter();
        }else if(displayedPawn.pawnHolder != null)
        {
            if (displayedPawn.pawnHolder.name != displayedPawn.loadOutTemplate.leaderPawn.name)
            {
                Destroy(displayedPawn.pawnHolder.gameObject);

                DisplayCharacter();

            }
        }
        
    }

    public void NextButton()
	{
        currentIndex++;
        if(currentIndex >= pawnCards.Length)
		{
            currentIndex = 0;
		}
        //UPDATES
        DisplayInfo();
        
	}
    public void BackButton()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = pawnCards.Length - 1;
        }
        //UPDATES
        DisplayInfo();
    }

    public void DisplayCharacter()
    {

        displayedPawn.pawnHolder = Instantiate(displayedPawn.loadOutTemplate.leaderPawn.transform);
        displayedPawn.pawnHolder.name = displayedPawn.loadOutTemplate.leaderPawn.name;
        displayedPawn.pawnHolder.transform.parent = displayedPawn.transform;
        displayedPawn.pawnHolder.localScale = new Vector3(500, 500, 500);
        displayedPawn.pawnHolder.transform.localPosition = new Vector3(-7.5f, -1058, -166);
        Destroy(displayedPawn.pawnHolder.GetComponent<PawnManager>());
        //SelectButton.color = new Color(1,1,1,1);
        //cardSpriteImage.sprite = pawnCards[currentIndex].cardSprite;
        //cardSpriteImage.GetComponent<Image>().color = new Color(1f,1f,1f,1f);

        //cardNameText.text = pawnCards[currentIndex].cardName;
        //cardHisText.text = pawnCards[currentIndex].cardHistory;
        //cardDesText.text = pawnCards[currentIndex].cardDescription;

    }

    //FOR INFO IN CHARACTER SCREEN FOR LATER USE
    public void DisplayInfo()
	{
        if(pawnCards[currentIndex].locked == false)
		{
            //SelectButton.color = new Color(1,1,1,1);
            //cardSpriteImage.sprite = pawnCards[currentIndex].cardSprite;
            //cardSpriteImage.GetComponent<Image>().color = new Color(1f,1f,1f,1f);

            //cardNameText.text = pawnCards[currentIndex].cardName;
            //cardHisText.text = pawnCards[currentIndex].cardHistory;
            //cardDesText.text = pawnCards[currentIndex].cardDescription;

		}
		else
		{
            //SelectButton.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
            //cardSpriteImage.sprite = pawnCards[currentIndex].cardSprite;
            //cardSpriteImage.GetComponent<Image>().color = new Color(0.15f, 0.15f, 0.15f, 1f);

            //cardNameText.text = "???";
            //cardHisText.text = "???";
            //cardDesText.text = "???";
        }
	}

    public void DisplayLevelBar()
	{
        //TO BE ADDED LATER
	}

    //FOR CHARACTER INFO IN SELECTION SCREEN
    public void DisplayCharacters()
	{
		for (int i = 0; i < slots.Length; i++)
		{
            slots[i].transform.GetChild(0).GetComponent<Image>().sprite = pawnCards[i].cardSprite;
            slots[i].transform.GetChild(1).GetComponent<Text>().text = pawnCards[i].cardName;

			if (!pawnCards[i].locked)
			{
                slots[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1,1);
			}
			else
			{
                slots[i].transform.GetChild(0).GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 1);
            }
		}
	}

    public void ShowPageUI()
	{
        //TO BE ADDED
	}
    public void UpdateLevel()
    {
        //TO BE ADDED
    }

}
