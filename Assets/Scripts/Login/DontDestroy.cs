using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DontDestroy : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        DontDestroyOnLoad(gameObject);
    }


    //Brise objekat Ime
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // here you can use scene.buildIndex or scene.name to check which scene was loaded
        if (scene.name == "Game")
        {
            // Destroy the gameobject this script is attached to
            Destroy(gameObject);
        }
    }
}
