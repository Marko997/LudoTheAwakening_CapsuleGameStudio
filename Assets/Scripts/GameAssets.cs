using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    #region Singleton
    private static GameAssets _i;

    public static GameAssets instance
    {
        get
        {
            if(_i == null)
            {
                _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            }
            return _i;
        }
    }
    #endregion

    public SoundAudioClip[] soundAudioClipArray;

    public Sprite grayToggleBackground_OffState;
    public Sprite goldToggleBackground_OnState;

    public Sprite grayToggleHandle_OffState;
    public Sprite goldToggleHandle_OnState;

    public Sprite[] soundIcons;
}

[System.Serializable]
public class SoundAudioClip
{
    public SoundManager.Sound sound;
    public AudioClip audioClip;
}


