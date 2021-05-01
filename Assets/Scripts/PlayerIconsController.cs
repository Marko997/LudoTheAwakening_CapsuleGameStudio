using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIconsController : MonoBehaviour
{
    public GameObject redIcon;
    public GameObject greenIcon;
    public GameObject blueIcon;
    public GameObject yellowIcon;


    void Start()
    {
        if(SaveSettings.numberOfPlayers != 2)
        {
            greenIcon.SetActive(true);
            yellowIcon.SetActive(true);
        }

    }
}
