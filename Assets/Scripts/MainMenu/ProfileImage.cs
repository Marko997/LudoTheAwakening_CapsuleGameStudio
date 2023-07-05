using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ProfileImage : MonoBehaviour
{
    [SerializeField] private Image mainPlayerImage;
    [SerializeField] private Image mainPlayerImageMainScene;
    [SerializeField] private GameObject[] allPlayerImages;

    [SerializeField] private Transform imagesParent;

    private void Start()
    {
        //Debug.Log(PlayerPrefs.GetInt("IMAGE"));
        //PlayerPrefs.DeleteKey("IMAGE");
        mainPlayerImage.sprite = allPlayerImages[PlayerPrefs.GetInt("IMAGE")].GetComponent<Image>().sprite;
        mainPlayerImageMainScene.sprite = allPlayerImages[PlayerPrefs.GetInt("IMAGE")].GetComponent<Image>().sprite;

        int indexButton = 1;
        foreach (var item in allPlayerImages.Skip(1))
        {
            //Debug.Log(indexButton);
            var image = Instantiate(item, imagesParent);
            var copy = image;
            var ind = indexButton;
            copy.AddComponent<ScreenSwitcher>();
            copy.GetComponent<Button>().onClick.AddListener(() => UpdatePlayerImage(ind, copy));
            
            
            indexButton++;
        }
    }

    public void UpdatePlayerImage(int index, GameObject button)//Index cannot be 0
    {
        PlayerPrefs.SetInt("IMAGE",index);
        mainPlayerImage.sprite = allPlayerImages[index].GetComponent<Image>().sprite;
        mainPlayerImageMainScene.sprite = allPlayerImages[index].GetComponent<Image>().sprite;

        button.GetComponent<ScreenSwitcher>().desiredScreenType = ScreenType.MainMenu;
        button.GetComponent<ScreenSwitcher>().OnButtonClicked();
        
    }
}
