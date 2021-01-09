using UnityEngine;
using System.Collections;

public class CameraOrbit : MonoBehaviour
{

    private Transform _XForm_Camera;
    private Transform _XForm_Parent;

    private Vector3 _LocalRotation;
    private float _CameraDistance = 10f;

    private float MouseSensitivity = 0.1f;
    private float ScrollSensitvity = 2f;
    private float OrbitDampening = 3f;
    private float ScrollDampening = 6f;

    private bool CameraDisabled = false;


    void Start()
    {
        this._XForm_Camera = this.transform;
        this._XForm_Parent = this.transform.parent;
    }

    
    void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            CameraDisabled = !CameraDisabled;
        }

        if(!CameraDisabled){
            if(Input.GetMouseButton(0) && Input.touchCount ==2){
            //Rotation of the Camera based on Mouse Coordinates
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                _LocalRotation.x += Input.GetAxis("Mouse X") * MouseSensitivity;
                _LocalRotation.y += Input.GetAxis("Mouse Y") * MouseSensitivity;
 
                //Clamp the y Rotation to horizon and not flipping over at the top
                if (_LocalRotation.y < 0f)
                    _LocalRotation.y = 0f;
                else if (_LocalRotation.y > 90f)
                    _LocalRotation.y = 90f;
                }
            }
            //Zooming Input from our Mouse Scroll Wheel
            if(Input.touchCount ==2){
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;
                difference *= 0.2f;
                this._CameraDistance = Mathf.Clamp(this._CameraDistance - difference, 1.5f, 100f);


            }
            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitvity;
 
                ScrollAmount *= (this._CameraDistance * 0.3f);
 
                this._CameraDistance += ScrollAmount * -1f;
 
                this._CameraDistance = Mathf.Clamp(this._CameraDistance, 1.5f, 100f);
            }
        
        }
 
        //Actual Camera Rig Transformations
        Quaternion QT = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
        this._XForm_Parent.rotation = Quaternion.Lerp(this._XForm_Parent.rotation, QT, Time.deltaTime * OrbitDampening);
 
        if ( this._XForm_Camera.localPosition.z != this._CameraDistance * -1f )
        {
            this._XForm_Camera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(this._XForm_Camera.localPosition.z, this._CameraDistance * -1f, Time.deltaTime * ScrollDampening));
        }
    
        
        
    }
}
