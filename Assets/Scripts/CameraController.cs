using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float mouseZoomSpeed = 15.0f;
    float touchZoomSpeed = 0.1f;
    float zoomMinBound = 20.0f;
    float zoomMaxBound = 60.0f;
    Camera cam;

    [SerializeField] private Transform target;
    private float distanceToTarget;

    float mouseX, mouseY;

    public float rotateSpeed = 100f;
    public float zoomSpeed = 10f;
    public float minHeight = 0f;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        distanceToTarget = 50;
        cam.transform.position = new Vector3(target.position.x, target.position.y + distanceToTarget, target.position.z - distanceToTarget);
        cam.transform.LookAt(target);
    }
    void Zoom2(float scroll, float zoomSpeed)
    {
        transform.Translate(Vector3.forward * scroll * zoomSpeed);
    }
    // Update is called once per frame
    void Update()
    {
        try
        {
            // Rotate the camera around the target
            float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
            float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;
            transform.Rotate(Vector3.up, horizontal, Space.World);
            transform.Rotate(Vector3.right, -vertical, Space.Self);

            // Zoom in and out using the mouse scroll wheel
            float scroll2 = Input.GetAxis("Mouse ScrollWheel");
            Zoom(scroll2, zoomSpeed);

            // Keep the camera looking at the target
            transform.LookAt(target);

            // Restrict the camera's height so it doesn't go below the target
            if (transform.position.y < minHeight)
            {
                transform.position = new Vector3(transform.position.x, minHeight, transform.position.z);
            }




            //if (Input.GetAxis("Mouse ScrollWheel") > 0)
            //{
            //    transform.Translate(Vector3.forward * 100 * Time.deltaTime);
            //}
            //else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            //{
            //    transform.Translate(Vector3.back * 100 * Time.deltaTime);
            //}

            //transform.LookAt(target);

            //if (Input.GetMouseButton(0))
            //{
            //    mouseX += Input.GetAxis("Mouse X") * 10f;
            //    mouseY -= Input.GetAxis("Mouse Y") * 10f;
            //    mouseY = Mathf.Clamp(mouseY, -30, 10);

            //    cam.transform.position = target.position;

            //    cam.transform.rotation = Quaternion.Euler(mouseY, mouseX, 0);

            //    cam.transform.Translate(new Vector3(0, distanceToTarget, -distanceToTarget));
            //    cam.transform.LookAt(target);

            //    //float scroll = Input.GetAxis("Mouse ScrollWheel");
            //    //Zoom(scroll, mouseZoomSpeed);
            //}


            //Camera rotation
            if (Input.touches[0].phase == TouchPhase.Moved && Input.touchCount == 1)
            {
                mouseX += Input.touches[0].deltaPosition.x * 0.1f;
                mouseY -= Input.touches[0].deltaPosition.y * 0.1f;
                mouseY = Mathf.Clamp(mouseY, -30, 10);

                cam.transform.position = target.position;

                cam.transform.rotation = Quaternion.Euler(mouseY, mouseX, 0);

                cam.transform.Translate(new Vector3(0, distanceToTarget, -distanceToTarget));
                cam.transform.LookAt(target);
            }
            if (Input.touchSupported)
            {
                Debug.Log("update");
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
            }
            else
            {
                Debug.Log("this");
                float scroll = Input.GetAxis("Mouse ScrollWheel");
                Zoom(scroll, mouseZoomSpeed);
            }
        }
        catch (System.IndexOutOfRangeException)
        {

            //throw;
        }

        if (cam.fieldOfView < zoomMinBound)
        {
            cam.fieldOfView = 30.0f;
        }
        else if (cam.fieldOfView > zoomMaxBound)
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
