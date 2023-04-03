using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectHero : MonoBehaviour
{
    [SerializeField] GameObject[] heroes;
    [SerializeField] HeroRotatorY heroRotator;

    private void Start()
    {
        UpdateMainPawn();  
    }

    public void UpdateMainPawn()
    {
        string[] deckArray = PlayerPrefs.GetString("Deck").Split(',');
        ChangePawn(deckArray[0]);
    }

    public void ChangePawn(string cardName)
    {

        for (int i = 0; i < heroes.Length; i++)
        {
            if(heroes[i].name == "TT_"+cardName) //hardcoded name!
            {
                heroes[i].SetActive(true);
                heroRotator.pawnHolder = heroes[i].transform;
            }
            else
            {
                heroes[i].SetActive(false);
            }
        }
    }
}
