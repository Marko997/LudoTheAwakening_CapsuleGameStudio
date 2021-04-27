using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PawnSelectionUIManager : MonoBehaviour
{
    public PawnCardManager cardManager;

    public Image playerImage;
    public Image playerImage1;
    public Image playerImage2;
    public Image playerImage3;

	public SelectionTemplate templateBlue;
	public SelectionTemplate templateRed;

	public LeaderPawn leaderPawn;
	public LeaderPawn secondPawn;
	public LeaderPawn thirdPawn;
	public LeaderPawn fourthPawn;

	private void Start()
	{
		cardManager = FindObjectOfType<PawnCardManager>();
		//UpdatePlayerImage();
	}

	public void UpdatePlayerImage(Image pawnImage)
	{
		pawnImage.sprite = cardManager.pawnCards[cardManager.currentIndex].cardSprite;
	}
	public void UpdatePawn(SelectionTemplate pawnTemplate, PawnCardManager cardManager, LeaderPawn pawn)
	{
		//SOME UPDATE
		//pawnTemplate.leaderPawn = cardManager.pawnCards[cardManager.currentIndex].pawnObject.gameObject;
		pawn.template = pawnTemplate;
		pawn.pawn = cardManager.pawnCards[cardManager.currentIndex].pawnObject.gameObject;
		//pawn.template.leaderPawn = pawn.pawn;
	}
	public void UpdateLeaderPawn(SelectionTemplate pawnTemplate, PawnCardManager cardManager)
	{
		//SOME UPDATE
		//pawnTemplate.leaderPawn = cardManager.pawnCards[cardManager.currentIndex].pawnObject.gameObject;
		leaderPawn.template = pawnTemplate;
		leaderPawn.pawn = cardManager.pawnCards[cardManager.currentIndex].pawnObject.gameObject;
		//leaderPawn.template.leaderPawn = leaderPawn.pawn;
	}
	public void UpdateSecondPawn(SelectionTemplate pawnTemplate, PawnCardManager cardManager)
	{
		//SOME UPDATE
		secondPawn.template = pawnTemplate;
		secondPawn.pawn = cardManager.pawnCards[cardManager.currentIndex].pawnObject.gameObject;
		secondPawn.template.secondPawn = secondPawn.pawn;
	}
	public void UpdateThirdPawn(SelectionTemplate pawnTemplate, PawnCardManager cardManager)
	{
		//SOME UPDATE
		thirdPawn.template = pawnTemplate;
		thirdPawn.pawn = cardManager.pawnCards[cardManager.currentIndex].pawnObject.gameObject;
		thirdPawn.template.thirdPawn = thirdPawn.pawn;
	}
	public void UpdateFourtPawn(SelectionTemplate pawnTemplate, PawnCardManager cardManager)
	{
		//SOME UPDATE
		fourthPawn.template = pawnTemplate;
		fourthPawn.pawn = cardManager.pawnCards[cardManager.currentIndex].pawnObject.gameObject;
		fourthPawn.template.fourthPawn = fourthPawn.pawn;
	}

	public void SelectCharacter(int _id)
	{
		cardManager.currentIndex = _id;

		cardManager.DisplayInfo();
	}

	public void PressPawnIconToSelectPawn()
	{
		if (cardManager.pawnCards[cardManager.currentIndex].locked == false)
		{
			if (leaderPawn.isBlue && leaderPawn.isOppened)
			{
				var templateSelect = templateBlue;
				AddLeader(templateSelect, playerImage,leaderPawn);
				templateSelect.leaderPawn = leaderPawn.pawn;
				leaderPawn.isBlue = false;
			}
			if (secondPawn.isBlue && secondPawn.isOppened)
			{
				var templateSelect = templateBlue;
				AddLeader(templateSelect, playerImage1,secondPawn);
				templateSelect.secondPawn = secondPawn.pawn;
				secondPawn.isBlue = false;
			}
			if (thirdPawn.isBlue && thirdPawn.isOppened)
			{
				var templateSelect = templateBlue;
				AddLeader(templateSelect, playerImage2, thirdPawn);
				templateSelect.thirdPawn = thirdPawn.pawn;
				thirdPawn.isBlue = false;
			}
			if (fourthPawn.isBlue && fourthPawn.isOppened)
			{
				var templateSelect = templateBlue;
				AddLeader(templateSelect, playerImage3, fourthPawn);
				templateSelect.fourthPawn = fourthPawn.pawn;
				fourthPawn.isBlue = false;
			}
		}





		//{
			

		//	if (secondPawn.isOppened)
		//	{
		//		UpdateSecondPawn(templateBlue, cardManager);
		//		UpdatePlayerImage(playerImage1);
		//		Debug.Log(templateBlue.secondPawn);

		//		secondPawn.isOppened = false;
		//	}
		//	if (thirdPawn.isOppened)
		//	{
		//		UpdateThirdPawn(templateBlue, cardManager);
		//		UpdatePlayerImage(playerImage2);
		//		Debug.Log(templateBlue.leaderPawn);

		//		thirdPawn.isOppened = false;

		//	}
		//	if (fourthPawn.isOppened)
		//	{
		//		UpdateFourtPawn(templateBlue, cardManager);
		//		UpdatePlayerImage(playerImage3);
		//		Debug.Log(templateBlue.secondPawn);

		//		fourthPawn.isOppened = false;
		//	}


		//}
	}

	private void AddLeader(SelectionTemplate template, Image playerImage, LeaderPawn pawn)
	{
		UpdatePawn(template, cardManager,pawn);
		UpdatePlayerImage(playerImage);
		Debug.Log(template.leaderPawn);

		pawn.isOppened = false;
		
	}

	

}
