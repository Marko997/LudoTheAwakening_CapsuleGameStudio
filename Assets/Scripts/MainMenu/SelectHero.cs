using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectHero : MonoBehaviour
{
    [SerializeField] GameObject[] heroes;

    private void Start()
    {
        UpdateMainPawn();  
    }

    public void UpdateMainPawn()
    {
        string[] deckArray = PlayerPrefs.GetString("Deck").Split(',');
        ChangePawn(Array.IndexOf(deckArray, deckArray[0]));
        Debug.Log(Array.IndexOf(deckArray, deckArray[0]));
    }

    public void ChangePawn(int pawnIndex)
    {
        for (int i = 0; i < heroes.Length; i++)
        {

            if (i == pawnIndex)
            {
                heroes[pawnIndex].gameObject.SetActive(true);
            }
            else
            {
                heroes[i].gameObject.SetActive(false);
            }
        }
    }
}
