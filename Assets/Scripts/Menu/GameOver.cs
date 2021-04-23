using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{

    public Text first,second,third,fourth;

    // Start is called before the first frame update
    void Start()
    {
        first.text = "1st: " + SaveSettings.winners[0];
        second.text = "2nd: " + SaveSettings.winners[0];
        third.text = "3rd: " + SaveSettings.winners[0];
    }

    public void BackButton(string sceneName)
	{
        SceneManager.LoadScene(sceneName);
        LevelLoaderManager.sceneToLoad = "MainScene";
	}

    
}
