using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ToogleContoller : MonoBehaviour
{
    public Toggle soundToggle;
    public Toggle vibrateToggle;
    public Toggle notificationToggle;
    public AudioSource audioSource;

    private string soundToggleKey = "SoundToggleState";
    private string vibrateToggleKey = "VibrateToggleState";
    private string notificationToggleKey = "NotificationToggleState";

    void Start()
    {
        // Učitavanje sačuvanih vrednosti kada se igra pokrene
        soundToggle.isOn = PlayerPrefs.GetInt(soundToggleKey, 0) == 1;
        vibrateToggle.isOn = PlayerPrefs.GetInt(vibrateToggleKey, 0) == 1;
        notificationToggle.isOn = PlayerPrefs.GetInt(notificationToggleKey, 0) == 1;
        // Podešavanje reprodukcije zvuka u skladu sa trenutnim stanjem kvačice za zvuk
        if (soundToggle.isOn)
        {
            PlayAudio();
            Debug.Log("Sound ONN");
        }
        else
        {
            StopAudio();
            Debug.Log("Sound OFF");
        }
    }

    public void SaveSoundToggleState(bool state)
    {
        // Čuvanje stanja kvačice (toggle) kada se klikne na nju
        PlayerPrefs.SetInt(soundToggleKey, state ? 1 : 0);
        PlayerPrefs.Save();

        // Ažuriranje reprodukcije zvuka u skladu sa novim stanjem kvačice
        if (state)
        {
            PlayAudio();
        }
        else
        {
            StopAudio();
        }
    }

    public void SaveVibrateToggleState(bool state)
    {
        // Čuvanje stanja kvačice (toggle) kada se klikne na nju
        PlayerPrefs.SetInt(vibrateToggleKey, state ? 1 : 0);
        PlayerPrefs.Save();

        // Ažuriranje vibracije u skladu sa novim stanjem kvačice
        if (state)
        {
            // Uključivanje vibracije
            Handheld.Vibrate();
        }
        else
        {
            // Isključivanje vibracije
            //Moram da nadjem 
        }
    }

    public void SaveNotificationToggleState(bool state)
    {
        // Čuvanje stanja kvačice (toggle) kada se klikne na nju
        PlayerPrefs.SetInt(notificationToggleKey, state ? 1 : 0);
        PlayerPrefs.Save();

        // Ažuriranje vibracije u skladu sa novim stanjem kvačice
        if (state)
        {
            // Uključivanje notifikacije
        }
        else
        {
            // Isključivanje notifikacije
        }
    }

    private void PlayAudio()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private void StopAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    //public Toggle toggle1;
    //public Toggle toggle2;
    //public Toggle toggle3;

    //private string toggle1Key = "Toggle1State";
    //private string toggle2Key = "Toggle2State";
    //private string toggle3Key = "Toggle3State";

    //void Start()
    //{
    //    // Učitavanje sačuvanih vrednosti kada se igra pokrene
    //    toggle1.isOn = PlayerPrefs.GetInt(toggle1Key, 0) == 1;
    //    toggle2.isOn = PlayerPrefs.GetInt(toggle2Key, 0) == 1;
    //    toggle3.isOn = PlayerPrefs.GetInt(toggle3Key, 0) == 1;
    //}

    //public void SaveToggle1State(bool state)
    //{
    //    // Čuvanje stanja kvačice (toggle) kada se klikne na nju
    //    PlayerPrefs.SetInt(toggle1Key, state ? 1 : 0);
    //    PlayerPrefs.Save();
    //}

    //public void SaveToggle2State(bool state)
    //{
    //    // Čuvanje stanja kvačice (toggle) kada se klikne na nju
    //    PlayerPrefs.SetInt(toggle2Key, state ? 1 : 0);
    //    PlayerPrefs.Save();
    //}

    //public void SaveToggle3State(bool state)
    //{
    //    // Čuvanje stanja kvačice (toggle) kada se klikne na nju
    //    PlayerPrefs.SetInt(toggle3Key, state ? 1 : 0);
    //    PlayerPrefs.Save();
    //}
}
