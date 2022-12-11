using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDown : MonoBehaviour
{
    public GameObject canvas;
    //public Camera cam;
    //public void ChangeNavigationView(int option)
    //{

    //    if (option == 0)
    //    {
    //        Camera.main.transform.rotation = Quaternion.Euler(90, 0, 0);
    //    }
    //    else if (option == 1)
    //    {
    //        //CameraRotate(-90, 0, 0);
    //        Camera.main.transform.rotation = Quaternion.Euler(45, 0, 0);
    //    }
    //    else
    //    {
    //        Camera.main.transform.rotation = Quaternion.Euler(0, 45, 0);
    //    }

    //    //if (index == 0)
    //    //{
    //    //    Camera.main.transform.rotation = Quaternion.Euler(90, 0, 0);
    //    //}
    //    //else if (index == 1)
    //    //{
    //    //    Camera.main.transform.rotation = Quaternion.Euler(45, 0, 0);
    //    //}
    //    //else
    //    //{
    //    //    Camera.main.transform.rotation = Quaternion.Euler(0, 45, 0);
    //    //}

    //}

    //private void CameraRotate(float xRotation, float yRotation, float zRotation)
    //{
    //    Camera.main.transform.eulerAngles = new Vector3(xRotation, yRotation, zRotation);
    //}

    //public Camera cam;
    public Dropdown mydropdown;
    // Update is called once per frame  
    void Update()
    {
        switch (mydropdown.value)
        {
            case 0:
                
                //canvas.transform.position = new Vector3(0f, 30f, 0f);
                //canvas.transform.rotation = Quaternion.Euler(90, 0, 0);
                Camera.main.transform.position = new Vector3(0f, 30f, 0f);
                Camera.main.transform.rotation = Quaternion.Euler(90, 0, 0);
                break;
            case 1:
                
                //canvas.transform.position = new Vector3(0f, 19.33f, -19.33f);
                //canvas.transform.rotation = Quaternion.Euler(45, 0, 0);
                Camera.main.transform.position = new Vector3(0f, 19.33f, -19.33f);
                Camera.main.transform.rotation = Quaternion.Euler(45, 0, 0);
                break;
            case 2:
                //canvas.transform.position = new Vector3(-20f, 13.3f, -20f);
                //canvas.transform.rotation = Quaternion.Euler(26, 45, 0);
                Camera.main.transform.position = new Vector3(-20f, 13.3f, -20f);
                Camera.main.transform.rotation = Quaternion.Euler(26, 45, 0);
                break;
        }
    }




    //public GameObject gCameraRight, gCameraLeft;

    //void Start()
    //{
    //    canvas.SetActive(false);
    //    GameObject cRight, cLeft;

    //    cRight = Instantiate(canvas) as GameObject;
    //    cLeft = Instantiate(canvas) as GameObject;

    //    cRight.SetActive(true);
    //    cLeft.SetActive(true);

    //    Canvas cL, cR;
    //    cL = cLeft.GetComponent<Canvas>();
    //    cR = cRight.GetComponent<Canvas>();

    //    cL.renderMode = RenderMode.WorldSpace;
    //}

    // Update is called once per frame


}