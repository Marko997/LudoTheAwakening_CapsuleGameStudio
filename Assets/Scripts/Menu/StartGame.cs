﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    string sname;
    // Start is called before the first frame update
    void Start()
    {
		for (int i = 0; i < SaveSettings.players.Length; i++)
		{
            SaveSettings.players[i] = "CPU";
		}
        sname = "LoadingScene 1";


    }

    

    public void TwoPlayers(bool on)
	{
		if (on)
		{
           sname = "2 players";

        }
	}
    public void FourPlayers(bool on)
    {
        if (on)
        {
            //sname = "4 players";
            sname = "LoadingScene 1";
        }
    }
    public void StartTheGame()
    {
        string sceneName = sname;
        SceneManager.LoadScene(sceneName);
    }


}
