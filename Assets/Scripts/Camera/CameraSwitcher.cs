using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CameraSwitcher : MonoBehaviour
{
    public CameraType desiredCameraType;

    CameraManager cameraManager;
    Button menuButton;

    private void Start()
    {
        menuButton = GetComponent<Button>();
        menuButton.onClick.AddListener(OnButtonClicked);
        cameraManager = CameraManager.GetInstance();
    }

    void OnButtonClicked()
    {
        cameraManager.SwitchCamera(desiredCameraType);
    }
}
