using System.Collections;
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
            SaveSettings.players[i] = "BOT";
		}
        sname = "LoadingScene";
        SaveSettings.numberOfPlayers = 2;

    }

    

    public void TwoPlayers(bool on)
	{
		if (on)
		{
           sname = "LoadingScene";
            SaveSettings.numberOfPlayers = 2;
            SaveSettings.players[0] = "HUMAN";
            SaveSettings.players[1] = "HUMAN";
        }
	}
    public void FourPlayers(bool on)
    {
        if (on)
        {
            //sname = "4 players";
            sname = "LoadingScene";
            SaveSettings.numberOfPlayers = 4;
            SaveSettings.players[0] = "HUMAN";
            SaveSettings.players[1] = "HUMAN";
            SaveSettings.players[2] = "HUMAN";
            SaveSettings.players[3] = "HUMAN";
        }
    }
    public void OneCPUoneHuman(bool on)
    {
        if (on)
        {
            sname = "LoadingScene";
            SaveSettings.numberOfPlayers = 2;
            SaveSettings.players[0] = "HUMAN";
            SaveSettings.players[1] = "BOT";
        }
    }
    public void ThreeCpuOneHuman(bool on)
    {
        if (on)
        {
            //sname = "4 players";
            sname = "LoadingScene";
            SaveSettings.numberOfPlayers = 4;
            SaveSettings.players[0] = "HUMAN";
            SaveSettings.players[1] = "BOT";
            SaveSettings.players[2] = "BOT";
            SaveSettings.players[3] = "BOT";
        }
    }
    public void StartTheGame()
    {
        string sceneName = sname;
        LevelLoaderManager.sceneToLoad = "GameScene";
        SceneManager.LoadScene(sceneName);
    }


}
