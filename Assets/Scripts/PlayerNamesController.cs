using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerNamesController : MonoBehaviour
{
    public TextMeshProUGUI[] displayedPLayerNames;
    public TextMeshProUGUI playerName;

    void Start()
    {
        for (int i = 0; i < displayedPLayerNames.Length; i++)
        {
            //displayedPLayerNames[i].text = SaveSettings.playerNames[i];
            displayedPLayerNames[i].text = PlayFabUserLogin.displayName;
        }
        playerName.text = PlayFabUserLogin.displayName;
    }

}
