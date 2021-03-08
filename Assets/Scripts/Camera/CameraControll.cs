using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    [SerializeField]Transform target;
    Quaternion camRotation;

    // Start is called before the first frame update
    void Start()
    {
        camRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            transform.LookAt(target);
            camRotation.x += Input.GetAxis("Mouse Y")*1;
            camRotation.y += Input.GetAxis("Mouse X") * 1;

            camRotation.x = Mathf.Clamp(camRotation.x, 0, 90);

            transform.localRotation = Quaternion.Euler(camRotation.x, camRotation.y, camRotation.z);
        }
    }
}
