using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PawnSelectionUIManager : MonoBehaviour
{
    public PawnCardManager cardManager;

    public Image playerImage;

	private void Start()
	{
		cardManager = FindObjectOfType<PawnCardManager>();
		UpdatePlayerImage();
	}

	public void UpdatePlayerImage()
	{
		//SOME UPDATE
	}

	public void SelectCharacter(int _id)
	{
		cardManager.currentIndex = _id;

		cardManager.DisplayInfo();
	}
}
