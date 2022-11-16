using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DontDestroyCanvas : MonoBehaviour
{
    public GameObject Background;
    public GameObject MainMenuScreen;
    public GameObject ShopScreen;
    public GameObject DeckScreen;
    public GameObject ProfileScreen;
    public GameObject SettingsScreen;
    public GameObject EventScreen;
    public GameObject PremiumPassScreen;
    public GameObject Currency;
    public GameObject Hero;
    public GameObject Canvas;
    public GameObject Image;
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
            //Destroy(Background);
            //Destroy(MainMenuScreen);
            //Destroy(ShopScreen);
            //Destroy(DeckScreen);
            //Destroy(ProfileScreen);
            //Destroy(EventScreen);
            //DontDestroyOnLoad(SettingsScreen);
            //SettingsScreen.SetActive(true);
            //DontDestroyOnLoad(Canvas);
            //Destroy(PremiumPassScreen);
            //Destroy(PlayeScreen);
            //Destroy(LobyScreen);
            DontDestroyOnLoad(Background);
            Destroy(MainMenuScreen);
            Destroy(ShopScreen);
            Destroy(DeckScreen);
            Destroy(ProfileScreen);
            Destroy(EventScreen);
            DontDestroyOnLoad(SettingsScreen);
            //SettingsScreen.SetActive(true);
            DontDestroyOnLoad(Canvas);
            Destroy(PremiumPassScreen);
            Destroy(Currency);
            //PlayeScreen.SetActive(false);
            Destroy(Hero);
            Destroy(Image);
            //LobyScreen.SetActive(false);
        }
    }
}
