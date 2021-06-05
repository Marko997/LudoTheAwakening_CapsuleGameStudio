using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerNamesController : MonoBehaviour
{
    public TextMeshProUGUI[] displayedPLayerNames;

    void Start()
    {
        for (int i = 0; i < displayedPLayerNames.Length; i++)
        {
            displayedPLayerNames[i].text = SaveSettings.playerNames[i];
        }
    }

}
