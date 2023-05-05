using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class SoundManager
{
    public enum Sound
    {
        ButtonClick,
        MainMenuBackground,
        DiceRoll,
        AttackClick,
        LevelPassed,
        PieceJump
    }

    public static Dictionary<Sound, float> soundTimerDictionary;
    private static GameObject oneSoundGameObject;
    private static AudioSource oneSoundAudioSource;

    public static void Initialize()
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
        soundTimerDictionary[Sound.ButtonClick] = 0f;
    }

    public static void PlaySound(Sound sound) //this creates and destroys every time
    {
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("Sound");
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

            audioSource.clip = GetAudioClip(sound);
            audioSource.Play();
            Object.Destroy(soundGameObject, audioSource.clip.length);
        }
    }
    public static void PlayOneSound(Sound sound) //more effective way
    {
        if (CanPlaySound(sound))
        {
            if (oneSoundGameObject == null)
            {
                oneSoundGameObject = new GameObject("One Click Sound");
                oneSoundAudioSource = oneSoundGameObject.AddComponent<AudioSource>();
            }
            oneSoundAudioSource.PlayOneShot(GetAudioClip(sound));
        }
    }

    private static bool CanPlaySound(Sound sound)//this is for playing sound with length
    {
        switch (sound)
        {
            default:
                return true;
            //case Sound.ButtonClick:
            //    if (soundTimerDictionary.ContainsKey(sound))
            //    {
            //        float lastTimePlayed = soundTimerDictionary[sound];
            //        float playerMoveTimerMax = .05f; //delay
            //        if(lastTimePlayed + playerMoveTimerMax < Time.time)
            //        {
            //            soundTimerDictionary[sound] = Time.time;
            //            return true;
            //        }
            //        else
            //        {
            //            return false;
            //        }
            //    }
            //    else
            //    {
            //        return true;
            //    }
        }
    }

    private static AudioClip GetAudioClip (Sound sound)
    {
        foreach (var soundClip in GameAssets.instance.soundAudioClipArray)
        {
            if(sound == soundClip.sound)
            {
                return soundClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }

    public static void AddButtonSounds(this Button button)
    {
        button.onClick.AddListener(() => SoundManager.PlayOneSound(Sound.ButtonClick));
    }
}
