using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PawnSelectionUIManager : MonoBehaviour
{
    public GameObject greenPlayerUI;
    public GameObject yellowPlayerUI;

    public bool twoPlayers;

    public PawnCardManager cardManager;

    public Image[] playerImage;
    public Image[] playerImage1;
    public Image[] playerImage2;
    public Image[] playerImage3;

	public SelectionTemplate templateBlue;
	public SelectionTemplate templateRed;
	public SelectionTemplate templateGreen;
	public SelectionTemplate templateYellow;

	public LeaderPawn[] leaderPawn;
	public LeaderPawn[] secondPawn;
	public LeaderPawn[] thirdPawn;
	public LeaderPawn[] fourthPawn;


	private void Start()
	{
		cardManager = FindObjectOfType<PawnCardManager>();
        
    }
    private void Update()
    {
        if (twoPlayers)
        {
            greenPlayerUI.SetActive(false);
            yellowPlayerUI.SetActive(false);
        }
        else
        {
            greenPlayerUI.SetActive(true);
            yellowPlayerUI.SetActive(true);
        }

    }


    public void UpdatePlayerImage(Image pawnImage)
	{
		pawnImage.sprite = cardManager.pawnCards[cardManager.currentIndex].cardSprite;
	}
	public void UpdatePawn(SelectionTemplate pawnTemplate, PawnCardManager cardManager, LeaderPawn pawn)
	{
		pawn.template = pawnTemplate;
		pawn.pawn = cardManager.pawnCards[cardManager.currentIndex].pawnObject.gameObject;
	}

	public void SelectCharacter(int _id)
	{
		cardManager.currentIndex = _id;
		cardManager.DisplayInfo();
	}

    public void PressPawnIconToSelectPawn()
    {
        for (int i = 0; i < leaderPawn.Length; i++)
        {
            if ((leaderPawn[i].isBlue && leaderPawn[i].isOppened) || (secondPawn[i].isBlue && secondPawn[i].isOppened) || (thirdPawn[i].isBlue && thirdPawn[i].isOppened) || (fourthPawn[i].isBlue && fourthPawn[i].isOppened))
            {
                AddPawnsToTemplate(templateBlue);
                leaderPawn[i].isBlue = false;
                secondPawn[i].isBlue = false;
                thirdPawn[i].isBlue = false;
                fourthPawn[i].isBlue = false;
            }
            if ((leaderPawn[i].isRed && leaderPawn[i].isOppened) || (secondPawn[i].isRed && secondPawn[i].isOppened) || (thirdPawn[i].isRed && thirdPawn[i].isOppened) || (fourthPawn[i].isRed && fourthPawn[i].isOppened))
            {
                AddPawnsToTemplate(templateRed);
                leaderPawn[i].isRed = false;
                secondPawn[i].isRed = false;
                thirdPawn[i].isRed = false;
                fourthPawn[i].isRed = false;
            }
            if ((leaderPawn[i].isGreen && leaderPawn[i].isOppened) || (secondPawn[i].isGreen && secondPawn[i].isOppened) || (thirdPawn[i].isGreen && thirdPawn[i].isOppened) || (fourthPawn[i].isGreen && fourthPawn[i].isOppened))
            {
                AddPawnsToTemplate(templateGreen);
                leaderPawn[i].isGreen = false;
                secondPawn[i].isGreen = false;
                thirdPawn[i].isGreen = false;
                fourthPawn[i].isGreen = false;
            }
            if ((leaderPawn[i].isYellow && leaderPawn[i].isOppened) || (secondPawn[i].isYellow && secondPawn[i].isOppened) || (thirdPawn[i].isYellow && thirdPawn[i].isOppened) || (fourthPawn[i].isYellow && fourthPawn[i].isOppened))
            {
                AddPawnsToTemplate(templateYellow);
                leaderPawn[i].isYellow = false;
                secondPawn[i].isYellow = false;
                thirdPawn[i].isYellow = false;
                fourthPawn[i].isYellow = false;
            }
        }

    }

    private void AddPawnsToTemplate(SelectionTemplate template)
    {
        if (cardManager.pawnCards[cardManager.currentIndex].locked == false)
        {
            for (int i = 0; i < leaderPawn.Length; i++)
            {
                if (leaderPawn[i].isOppened)
                {
                    AddPawnToUiHolder(template, playerImage[i], leaderPawn[i]);
                    template.leaderPawn = leaderPawn[i].pawn;
                }
                if (secondPawn[i].isOppened)
                {
                    AddPawnToUiHolder(template, playerImage1[i], secondPawn[i]);
                    template.secondPawn = secondPawn[i].pawn;
                }
                if (thirdPawn[i].isOppened)
                {
                    AddPawnToUiHolder(template, playerImage2[i], thirdPawn[i]);
                    template.thirdPawn = thirdPawn[i].pawn;
                }
                if (fourthPawn[i].isOppened)
                {
                    AddPawnToUiHolder(template, playerImage3[i], fourthPawn[i]);
                    template.fourthPawn = fourthPawn[i].pawn;
                }
            }
        }
    }

    private void AddPawnToUiHolder(SelectionTemplate template, Image playerImage, LeaderPawn pawn)
	{
		UpdatePawn(template, cardManager,pawn);
		UpdatePlayerImage(playerImage);
		pawn.isOppened = false;		
	}

    public void ReturnNumberOfPlayers(bool _twoPlayers)
    {
        twoPlayers = _twoPlayers;
    }
}
