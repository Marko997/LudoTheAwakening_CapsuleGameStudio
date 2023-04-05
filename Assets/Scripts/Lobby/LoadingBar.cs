using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    public GameObject slider;
    public float rotationSpeed = -50f; // Adjust this value to control the speed of the spinning progress bar.

    private float currentRotation = 0f;

    public void SetProgress(float progress)
    {
        currentRotation = progress * 360f;
        slider.transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
    }

    private void Update()
    {
        currentRotation += rotationSpeed * Time.deltaTime;
        if (currentRotation > 360f)
        {
            currentRotation -= 360f;
        }
        slider.transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
    }
}
