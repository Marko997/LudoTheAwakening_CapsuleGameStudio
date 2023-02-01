using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeroRotatorY : MonoBehaviour
{
    private Vector3 previousMousePosition;
    private float rotationSpeed = 10f;

    Touch touch;

    public Transform pawnHolder;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 currentMousePosition = Input.mousePosition;
            if (currentMousePosition != previousMousePosition)
            {
                //mouse moved
                pawnHolder.transform.Rotate(0, -Input.GetAxis("Mouse X") * rotationSpeed, 0, Space.Self);
                GetComponentInParent<ScreenSwitcher>().isPawnMoving = true;
            }       
            previousMousePosition = currentMousePosition;

            
        }
        else if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                pawnHolder.Rotate(0, -touch.deltaPosition.x, 0, Space.Self);
                GetComponentInParent<ScreenSwitcher>().isPawnMoving = true;
            }else if (touch.phase == TouchPhase.Stationary)
            {
                GetComponentInParent<ScreenSwitcher>().isPawnMoving = false;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
             GetComponentInParent<ScreenSwitcher>().isPawnMoving = false;
        }

    }
}
