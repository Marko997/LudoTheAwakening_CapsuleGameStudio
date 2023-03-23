using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraStates
{
    RtsCam,
    TargetCam,
    TopCam,
    MiddleCam,
    AngleCam
}

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float distance = 30.0f;
    public float xSpeed = 250.0f;
    public float ySpeed = 120.0f;
    public float yMinLimit = -20.0f;
    public float yMaxLimit = 80.0f;
    public float zoomSpeed = 5.0f;
    public float zoomMin = 10.0f;
    public float zoomMax = 40.0f;
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


    //RTS Camera
    public float panSpeed = 1f;
    public float scrollSpeed = 2f;
    public float minY = 10f;
    public float maxY = 80f;
    public Vector2 minPosition = new Vector2(-15, -15);
    public Vector2 maxPosition = new Vector2(15, 15);
    //public float zoomSpeed = 0.5f;

    private Vector3 lastMousePosition;
    private Vector3 lastTouchPosition;
    private bool isPanning;
    private Vector2 touchDelta;

    public bool canMoveCamera;
    public CameraStates state;

    private Vector3 previousPosition;

    void Start()
    {
        state = SaveSettings.CameraState;

        previousPosition = transform.position;

        switch (state)
        {
            case CameraStates.RtsCam:
                break;
            case CameraStates.TargetCam:
                TargetCamSetup();
                break;
            case CameraStates.TopCam:
                transform.position = new Vector3(0f, 30f, 0f);
                transform.rotation = Quaternion.Euler(90, 0, 0);

                break;
            case CameraStates.MiddleCam:
                transform.position = new Vector3(0f, 19.33f, -19.33f);
                transform.rotation = Quaternion.Euler(45, 0, 0);

                break;
            case CameraStates.AngleCam:
                transform.position = new Vector3(-20f, 13.3f, -20f);
                transform.rotation = Quaternion.Euler(26, 45, 0);

                break;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        transform.position = previousPosition;
    }

    void LateUpdate()
    {
        if (UIElement.isUIElementPressed) { return; }
        switch (state)
        {
            case CameraStates.RtsCam:
                RTSCameraControlls();

                break;
            case CameraStates.TargetCam:
                TargetCamControlls();
                break;

        }
    }
    #region RTS Camera
    public void RTSCameraControlls()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
            isPanning = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isPanning = false;
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            lastTouchPosition = Input.GetTouch(0).position;
            isPanning = true;
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            isPanning = false;
        }

        if (isPanning)
        {
            Vector3 delta;
            if (Input.mousePresent)
                delta = Input.mousePosition - lastMousePosition;
            else
                delta = Input.GetTouch(0).deltaPosition;
            transform.Translate(-delta.x * panSpeed * Time.deltaTime, 0, -delta.y * panSpeed * Time.deltaTime, Space.World);
            lastMousePosition = Input.mousePosition;
            if(!Input.mousePresent)
            {
                lastTouchPosition = Input.GetTouch(0).position;
            }
            
        }

        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            float prevTouchDelta = (touchZero.position - touchZero.deltaPosition - touchOne.position + touchOne.deltaPosition).sqrMagnitude;
            float touchDelta = (touchZero.position - touchOne.position).sqrMagnitude;
            float delta = touchDelta - prevTouchDelta;

            Camera.main.fieldOfView += delta * zoomSpeed;
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, minY, maxY);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 pos = transform.position;
        pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        // Restrict camera position
        pos.x = Mathf.Clamp(pos.x, minPosition.x, maxPosition.x);
        pos.z = Mathf.Clamp(pos.z, minPosition.y, maxPosition.y);
        transform.position = pos;


    }

    #endregion

    #region Target Camera
    private void TargetCamSetup()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
        transform.position = new Vector3(0, zoomMax, 0);
        distance = zoomMax;
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        transform.rotation = rotation;
    }
    private void TargetCamControlls()
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
    #endregion

}
