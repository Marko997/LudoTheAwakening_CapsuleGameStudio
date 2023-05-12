using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class CharacterShowcase : MonoSingleton<CharacterShowcase>
{
    Button heroInfoButton;

    public GameObject heroImage;
    public TMP_Text heroName;
    public TMP_Text heroType;
    public TMP_Text eatPower;
    public TMP_Text description;

    public GameObject arhcerImage;
    public GameObject spearImage;
    public GameObject maceImage;
    public GameObject crossbowImage;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInfoButtonClicked(string hero)
    {
        switch (hero)
        {
            case "Spearman_Image(Clone)":
                heroName.text = "Ser Richard";
                heroImage = ReturnImage(heroName.text);
                heroType.text = "Melee";
                eatPower.text = "+1 tile";
                description.text = "Hits using sword.\nKills an enemy on " +
                                    "+1 tile in front of him after moving on board.";
                break;
            case "Archer_Image(Clone)":
                heroName.text = "Dan";
                heroImage = ReturnImage(heroName.text);
                heroType.text = "Range";
                eatPower.text = "+3 tiles";
                description.text = "Fires arrows using crossbow.\nKills an enemy on " +
                                    "+3 tiles in front of him after moving on board.";
                break;
            case "Crossbowman_Image(Clone)":
                heroName.text = "Takeda";
                heroImage = ReturnImage(heroName.text);
                heroType.text = "Melee";
                eatPower.text = "+2 tiles";
                description.text = "Hits using spear.\nKills an enemy on " +
                                    "+2 tile in front of him after moving on board.";
                break;
            case "Macebearer_Image(Clone)":
                heroName.text = "Theo";
                heroImage = ReturnImage(heroName.text);
                heroType.text = "Melee";
                eatPower.text = "-1 tile";
                description.text = "Hits using mace.\nKills an enemy on " +
                                    "tile behind him after moving on board.";
                break;

        }
    }

    public GameObject ReturnImage(string imageName)
    {
        switch (imageName)
        {
            case "Ser Richard":
                spearImage.SetActive(true);
                arhcerImage.SetActive(false);
                maceImage.SetActive(false);
                crossbowImage.SetActive(false);
                return spearImage;
                //break;
            case "Dan":
                spearImage.SetActive(false);
                arhcerImage.SetActive(true);
                maceImage.SetActive(false);
                crossbowImage.SetActive(false);
                return arhcerImage;
                //break;
            case "Takeda":
                spearImage.SetActive(false);
                arhcerImage.SetActive(false);
                maceImage.SetActive(false);
                crossbowImage.SetActive(true);
                return crossbowImage;
                //break;
            case "Theo":
                spearImage.SetActive(false);
                arhcerImage.SetActive(false);
                maceImage.SetActive(true);
                crossbowImage.SetActive(false);
                return maceImage;
                //break;

        }
        return maceImage;
    }
}
