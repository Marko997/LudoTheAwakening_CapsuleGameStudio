using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class PlayerScriptController : NetworkBehaviour
{
    public override void OnStartClient()
    {
        PrepareAndUpdatePlayerManagerScripts();
    }

    public override void OnStartServer()
    {
        PrepareAndUpdatePlayerManagerScripts();
    }

    public void PrepareAndUpdatePlayerManagerScripts()
    {
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            Debug.Log("MainScene");
            TurnAllScriptsOff();

            gameObject.AddComponent<PlayerLobbyManager>();
        }

        if (SceneManager.GetActiveScene().name == "MPgameScene")
        {
            Debug.Log("MPgameScene");
            TurnAllScriptsOff();
            //Ukljuci drugu skriptu

        }
    }

    private void TurnAllScriptsOff()
    {
        Destroy(gameObject.GetComponent<PlayerLobbyManager>());
    }
}
