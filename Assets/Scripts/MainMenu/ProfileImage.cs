using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ProfileImage : MonoBehaviour
{
    [SerializeField] private Image mainPlayerImage;
    [SerializeField] private GameObject[] allPlayerImages;

    [SerializeField] private Transform imagesParent;

    private void Start()
    {
        //Debug.Log(PlayerPrefs.GetInt("IMAGE"));
        //PlayerPrefs.DeleteKey("IMAGE");
        mainPlayerImage.sprite = allPlayerImages[PlayerPrefs.GetInt("IMAGE")].GetComponent<Image>().sprite;

        int indexButton = 1;
        foreach (var item in allPlayerImages.Skip(1))
        {
            //Debug.Log(indexButton);
            var image = Instantiate(item, imagesParent);
            var copy = image;
            var ind = indexButton;
            copy.GetComponent<Button>().onClick.AddListener(() => UpdatePlayerImage(ind));
            indexButton++;
        }
    }

    public void UpdatePlayerImage(int index)//Index cannot be 0
    {
        Debug.Log("updated "+index);
        PlayerPrefs.SetInt("IMAGE",index);
        mainPlayerImage.sprite = allPlayerImages[index].GetComponent<Image>().sprite;
    }
}
