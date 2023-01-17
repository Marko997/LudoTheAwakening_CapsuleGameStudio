using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Dropdown cameraDropdown;

    //Update is called once per frame  
    void Update()
    {
        switch (cameraDropdown.value)
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

}