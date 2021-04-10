﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float mouseZoomSpeed = 15.0f;
    float touchZoomSpeed = 0.1f;
    float zoomMinBound = 30.0f;
    float zoomMaxBound = 150.0f;
    Camera cam;
    public AllPawnsTemplates template;

    [SerializeField] private Transform target;
    [SerializeField] private float distanceToTarget = 100;

    float mouseX, mouseY;


    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {

        //Camera rotation
        if (Input.GetMouseButton(0) || Input.touchCount == 1)
        {
            //mouseX += Input.GetAxis("Mouse X") * 3;
            //mouseY -= Input.GetAxis("Mouse Y") * 3;
            mouseX += Input.touches[0].deltaPosition.x *0.1f;
            mouseY += Input.touches[0].deltaPosition.y *0.1f;
            mouseY = Mathf.Clamp(mouseY, 0, 90);

            cam.transform.position = target.position;
             
            target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
            cam.transform.rotation = target.transform.rotation;

            cam.transform.Translate(new Vector3(0, 0, -distanceToTarget));

        }
        if (Input.touchSupported)
		{            
            //PinchZoom
            if (Input.touchCount == 2)
			{
                //Get touch position
                Touch tZero = Input.GetTouch(0);
                Touch tOne = Input.GetTouch(1);

                //Get possition
                Vector2 tZeroPrevious = tZero.position - tZero.deltaPosition;
                Vector2 tOnePrevious = tOne.position - tOne.deltaPosition;

                float oldTouchDistance = Vector2.Distance(tZeroPrevious, tOnePrevious);
                float currentDistance = Vector2.Distance(tZero.position, tOne.position);

                //Get offset value

                float deltaDistance = oldTouchDistance - currentDistance;
                Zoom(deltaDistance, touchZoomSpeed);

			}
		}else
		{
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            Zoom(scroll, mouseZoomSpeed);
		}

        if(cam.fieldOfView < zoomMinBound)
		{
            cam.fieldOfView = 30.0f;
		}else if(cam.fieldOfView > zoomMaxBound)
		{
            cam.fieldOfView = 150.0f;
		}


    }
    void Zoom(float DeltaDistanceDiff, float speed)
	{
        cam.fieldOfView += DeltaDistanceDiff * speed;

        //Set clamp form cam

        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, zoomMinBound, zoomMaxBound);

	}
}