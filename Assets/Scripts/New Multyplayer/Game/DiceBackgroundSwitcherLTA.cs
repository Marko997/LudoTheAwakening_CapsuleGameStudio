using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class DiceBackgroundSwitcherLTA : NetworkBehaviour
{
    public static DiceBackgroundSwitcherLTA instance;

    public Sprite[] backgroundImages;
    public Image backgroundImage;

    private void Awake()
    {
        instance = this;
    }

    public void ChangeBackgroundImage(int colorId)
    {
        backgroundImage.sprite = ChangeSprite(colorId);
    }

    public Sprite ChangeSprite(int colorId)
    {
        //int x = colorId;
        //return backgroundImages[x];
        return backgroundImages[colorId];
    }
}
