using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceBackgoundSwitcher : MonoBehaviour
{
    public static DiceBackgoundSwitcher instance;

    public Sprite[] backgoundImages;
    public Image backgoundImage;

    private void Awake()
    {
        instance = this;
        
    }


    public void ChangeBackgroundImage(int colorId)
    {
        backgoundImage.sprite = ChangeSprite(colorId);
    }

    public Sprite ChangeSprite(int colorId)
    {
        int x = colorId;

        return backgoundImages[x];
    }
}
