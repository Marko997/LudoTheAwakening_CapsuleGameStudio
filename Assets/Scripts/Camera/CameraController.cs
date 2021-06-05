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


    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        distanceToTarget = 50;
        cam.transform.position = new Vector3(target.position.x, target.position.y + distanceToTarget, target.position.z - distanceToTarget);
        cam.transform.LookAt(target);
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
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
        }
        catch (System.IndexOutOfRangeException)
        {

            //throw;
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
