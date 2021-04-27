using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PawnSelectionUIManager : MonoBehaviour
{
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
            if((leaderPawn[i].isBlue && leaderPawn[i].isOppened) || (secondPawn[i].isBlue && secondPawn[i].isOppened) || (thirdPawn[i].isBlue && thirdPawn[i].isOppened) || (fourthPawn[i].isBlue && fourthPawn[i].isOppened))
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

}
