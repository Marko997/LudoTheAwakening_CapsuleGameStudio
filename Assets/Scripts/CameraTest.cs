using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
//Created by ChatGPT
public class CameraTest : MonoBehaviour
{
    public Transform target;
    public float distance = 30.0f;
    public float xSpeed = 250.0f;
    public float ySpeed = 120.0f;
    public float yMinLimit = -20.0f;
    public float yMaxLimit = 80.0f;
    public float zoomSpeed = 5.0f;
    public float zoomMin = 10.0f;
    public float zoomMax = 100.0f;
    public float touchSensitivity = 10.0f;
    public float pinchZoomSpeed = 0.5f;

    private float x = 0.0f;
    private float y = 0.0f;
    private Vector2 touchStartPosition;
    private Vector2 touchEndPosition;
    private Vector2 touchStartPosition2;
    private Vector2 touchEndPosition2;
    private float touchStartDistance;
    private float touchEndDistance;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
        transform.position = new Vector3(0,zoomMax,0);
        distance = zoomMax;
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        transform.rotation = rotation;
    }

    void LateUpdate()
    {
        if (target)
        {
            if (Input.GetMouseButton(0))
            {
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f; 
            }
            distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            distance = Mathf.Clamp(distance, zoomMin, zoomMax);

            if (Input.touchCount == 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    touchStartPosition = Input.GetTouch(0).position;
                }

                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    touchEndPosition = Input.GetTouch(0).position;
                    float xDelta = touchStartPosition.x - touchEndPosition.x;
                    float yDelta = touchStartPosition.y - touchEndPosition.y;
                    x += xDelta * touchSensitivity * 0.02f;
                    y -= yDelta * touchSensitivity * 0.02f;
                }
            }

            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                touchStartPosition = touchZero.position - touchOne.position;
                touchStartPosition2 = touchZero.position;
                touchStartDistance = touchStartPosition.magnitude;

                if (touchZero.phase == TouchPhase.Moved && touchOne.phase == TouchPhase.Moved)
                {
                    touchEndPosition = touchZero.position - touchOne.position;
                    touchEndPosition2 = touchZero.position;
                    touchEndDistance = touchEndPosition.magnitude;

                    float distanceDelta = touchStartDistance - touchEndDistance;
                    distance -= distanceDelta * pinchZoomSpeed;
                    distance = Mathf.Clamp(distance, zoomMin, zoomMax);
                }
            }
            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);

            transform.rotation = rotation;

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            transform.position = position;
        }
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
        {
            angle += 360;
        }
        if (angle > 360)
        {
            angle -= 360;
        }
        return Mathf.Clamp(angle, min, max);
    }
}
