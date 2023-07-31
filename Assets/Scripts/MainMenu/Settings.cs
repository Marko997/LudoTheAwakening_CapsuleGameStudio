using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public GameObject[] menuColorChangeButtons;
    public TMP_Text buildVersionText;

    [Header("Toggles")]
    public Toggle soundToggle;
    public Toggle vibrateToggle;
    public Toggle notificationToggle;

    public RectTransform handleSoundRectTransform;
    public Image soundToggleBackground;
    Vector2 handlePosition;

    private void Start()
    {
        buildVersionText.text = Application.version;

        SetPawnColor(PlayerPrefs.GetInt("COLOR"));

        ToggleLoad();
    }

    public void SetPawnColor(int playerId)
    {
        if (SceneManager.GetActiveScene().name != "MainScene")
        {
            return;
        }
        for (int i = 0; i < menuColorChangeButtons.Length; i++)
        {
            menuColorChangeButtons[i].SetActive(false);
        }
        menuColorChangeButtons[playerId].SetActive(true);
        SaveSettings.playerColorId = playerId;
        PlayerPrefs.SetInt("COLOR", playerId);
    }

    public void ToggleLoad()
    {
        handlePosition = handleSoundRectTransform.anchoredPosition;
        soundToggle.isOn = PlayerPrefs.GetInt("SOUND", 0) == 1;
        SoundManager.Initialize();

        if (soundToggle.isOn)
        {
            if(SceneManager.GetActiveScene().name == "MainScene")
            {
                SoundManager.PlayOneSound(SoundManager.Sound.MainMenuBackground);
            }
            else if(SceneManager.GetActiveScene().name == "BotsGame")
            {
                SoundManager.PlayOneSound(SoundManager.Sound.InGameBackground);
            }
        }
        else
        {
            SoundManager.StopAllSounds();
        }
    }

    public void ChangeSoundToggleState(bool state)
    {
        PlayerPrefs.SetInt("SOUND", state ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log(handlePosition);
        handleSoundRectTransform.anchoredPosition = state ? handlePosition : new Vector2(handlePosition.x *-1,handlePosition.y);

        handleSoundRectTransform.GetComponent<Image>().sprite = state ? GameAssets.instance.goldToggleHandle_OnState : GameAssets.instance.grayToggleHandle_OffState;
        soundToggleBackground.sprite = state ? GameAssets.instance.goldToggleBackground_OnState : GameAssets.instance.grayToggleBackground_OffState;

        //SoundManager.PlayOneSound(SoundManager.Sound.ButtonClick);

        if (state)
        {
            PlayerPrefs.GetInt("SOUND", 1);
            //SoundManager.PlayOneSound(SoundManager.Sound.MainMenuBackground);
            if (SceneManager.GetActiveScene().name == "MainScene")
            {
                SoundManager.PlayOneSound(SoundManager.Sound.MainMenuBackground);
            }
            else if (SceneManager.GetActiveScene().name == "BotsGame")
            {
                SoundManager.PlayOneSound(SoundManager.Sound.InGameBackground);
            }
        }
        else
        {
            PlayerPrefs.GetInt("SOUND", 0);
            SoundManager.StopAllSounds();
        }
    }
}