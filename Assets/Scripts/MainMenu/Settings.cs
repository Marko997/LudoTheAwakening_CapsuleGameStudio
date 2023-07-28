using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    public GameObject[] menuColorChangeButtons;
    public TMP_Text buildVersionText;

    private void Start()
    {
        buildVersionText.text = Application.version;
        SetPawnColor(PlayerPrefs.GetInt("COLOR"));
    }

    //Update is called once per frame  
    public void CameraDropdown(int index)
    {
        //Debug.Log(cameraDropdown.value);
        switch (index)
        {
            case 0:
                SaveSettings.CameraState = CameraStates.RtsCam;
                break;
            case 1:
                SaveSettings.CameraState = CameraStates.MiddleCam;
                break;
            case 2:
                SaveSettings.CameraState = CameraStates.AngleCam;
                break;
            case 3:
                SaveSettings.CameraState = CameraStates.RtsCam;
                break;
            case 4:
                SaveSettings.CameraState = CameraStates.TargetCam;
                break;
        }
    }


    public void SetPawnColor(int playerId)
    {
        for (int i = 0; i < menuColorChangeButtons.Length; i++)
        {
            menuColorChangeButtons[i].SetActive(false);
        }
        menuColorChangeButtons[playerId].SetActive(true);
        SaveSettings.playerColorId = playerId;
        PlayerPrefs.SetInt("COLOR", playerId);
    }
}