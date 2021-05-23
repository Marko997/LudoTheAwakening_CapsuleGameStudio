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
    public SelectionTemplate loadoutTemplate;

	public LeaderPawn[] leaderPawn;
	public LeaderPawn[] secondPawn;
	public LeaderPawn[] thirdPawn;
	public LeaderPawn[] fourthPawn;

    public Button startButton;



    private void Start()
	{
        cardManager = FindObjectOfType<PawnCardManager>();
        startButton.enabled = false;   
    }
    private void Update()
    {
        if (twoPlayers)
        {
            //DEACTIVATE OTHER PLAYER UI
            greenPlayerUI.SetActive(false);
            yellowPlayerUI.SetActive(false);

            //CHECK IF PAWNS ARE EMPTY
            CheckForEmptyPawns(SaveSettings.bluePawns);
            CheckForEmptyPawns(SaveSettings.redPawns);

        }
        else
        {
            //ACTIVATE OTHER PLAYER UI
            greenPlayerUI.SetActive(true);
            yellowPlayerUI.SetActive(true);

            //CHECK OTHER PLAYERS IF PAWNS ARE EMPTY
            CheckForEmptyPawns(SaveSettings.bluePawns);
            CheckForEmptyPawns(SaveSettings.redPawns);
            CheckForEmptyPawns(SaveSettings.greenPawns);
            CheckForEmptyPawns(SaveSettings.redPawns);
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
                AddPawnsToTemplate(templateBlue, SaveSettings.bluePawns);
                leaderPawn[i].isBlue = false;
                secondPawn[i].isBlue = false;
                thirdPawn[i].isBlue = false;
                fourthPawn[i].isBlue = false;
            }
            if ((leaderPawn[i].isRed && leaderPawn[i].isOppened) || (secondPawn[i].isRed && secondPawn[i].isOppened) || (thirdPawn[i].isRed && thirdPawn[i].isOppened) || (fourthPawn[i].isRed && fourthPawn[i].isOppened))
            {
                AddPawnsToTemplate(templateRed, SaveSettings.redPawns);
                leaderPawn[i].isRed = false;
                secondPawn[i].isRed = false;
                thirdPawn[i].isRed = false;
                fourthPawn[i].isRed = false;
            }
            if ((leaderPawn[i].isGreen && leaderPawn[i].isOppened) || (secondPawn[i].isGreen && secondPawn[i].isOppened) || (thirdPawn[i].isGreen && thirdPawn[i].isOppened) || (fourthPawn[i].isGreen && fourthPawn[i].isOppened))
            {
                AddPawnsToTemplate(templateGreen,SaveSettings.greenPawns);
                leaderPawn[i].isGreen = false;
                secondPawn[i].isGreen = false;
                thirdPawn[i].isGreen = false;
                fourthPawn[i].isGreen = false;
            }
            if ((leaderPawn[i].isYellow && leaderPawn[i].isOppened) || (secondPawn[i].isYellow && secondPawn[i].isOppened) || (thirdPawn[i].isYellow && thirdPawn[i].isOppened) || (fourthPawn[i].isYellow && fourthPawn[i].isOppened))
            {
                AddPawnsToTemplate(templateYellow,SaveSettings.yellowPawns);
                leaderPawn[i].isYellow = false;
                secondPawn[i].isYellow = false;
                thirdPawn[i].isYellow = false;
                fourthPawn[i].isYellow = false;
            }
            if ((leaderPawn[i].isLoadout && leaderPawn[i].isOppened) || (secondPawn[i].isLoadout && secondPawn[i].isOppened) || (thirdPawn[i].isLoadout && thirdPawn[i].isOppened) || (fourthPawn[i].isLoadout && fourthPawn[i].isOppened))
            {
                AddPawnsToTemplate(loadoutTemplate,SaveSettings.loadOutPawns);
                leaderPawn[i].isLoadout = false;
                secondPawn[i].isLoadout = false;
                thirdPawn[i].isLoadout = false;
                fourthPawn[i].isLoadout = false;
            }
        }

    }

    private void AddPawnsToTemplate(SelectionTemplate template, GameObject[] playerPawns)
    {
        if (cardManager.pawnCards[cardManager.currentIndex].locked == false)
        {
            for (int i = 0; i < leaderPawn.Length; i++)
            {
                if (leaderPawn[i].isOppened)
                {
                    AddPawnToUiHolder(template, playerImage[i], leaderPawn[i]);
                    template.leaderPawn = leaderPawn[i].pawn;
                    GameObject newLeader = cardManager.pawnCards[cardManager.currentIndex].pawnObject.gameObject;
                    playerPawns[0] = cardManager.pawnCards[cardManager.currentIndex].pawnObject.gameObject;

                }
                if (secondPawn[i].isOppened)
                {
                    AddPawnToUiHolder(template, playerImage1[i], secondPawn[i]);
                    playerPawns[1] = cardManager.pawnCards[cardManager.currentIndex].pawnObject.gameObject;
                }
                if (thirdPawn[i].isOppened)
                {
                    AddPawnToUiHolder(template, playerImage2[i], thirdPawn[i]);
                    playerPawns[2] = cardManager.pawnCards[cardManager.currentIndex].pawnObject.gameObject;
                }
                if (fourthPawn[i].isOppened)
                {
                    AddPawnToUiHolder(template, playerImage3[i], fourthPawn[i]);
                    playerPawns[3] = cardManager.pawnCards[cardManager.currentIndex].pawnObject.gameObject;

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

    public void CheckForEmptyPawns(GameObject[] playerPawns)
    {
        for (int i = 0; i < playerPawns.Length; i++)
        {
            if (playerPawns[i] == null)
            {
                startButton.enabled = false;
            }
            else
            {
                startButton.enabled = true;
            }
        }
    }

}
