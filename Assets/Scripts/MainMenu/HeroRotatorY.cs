using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeroRotatorY : MonoBehaviour//, IPointerUpHandler, IPointerDownHandler
{
    bool isPressed;
    bool isMoving;

    public float rotationSpeed = 90f;

    //void Update()
    //{
    //    if (Input.GetMouseButton(0))
    //    {
    //        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    //    }
    //}

    Touch touch;

    [SerializeField]
    public Transform pawnHolder;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            pawnHolder.Rotate(0, -Input.GetAxis("Mouse X") * rotationSpeed, 0);
            isMoving = true;
        }else if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved || Input.GetMouseButton(0))
            {

                pawnHolder.Rotate(0, -touch.deltaPosition.x, 0);


                isMoving = true;
            }
        }


        //if (isPressed)
        //{
        //    if (Input.touchCount > 0 || Input.GetMouseButton(0))
        //    {

        //        touch = Input.GetTouch(0);

        //        if (touch.phase == TouchPhase.Moved || Input.GetMouseButton(0))
        //        {

        //            pawnHolder.Rotate(0, -touch.deltaPosition.x, 0);


        //            isMoving = true;
        //        }
        //    }
        //}
    }

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    isPressed = true;
    //}

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    if (isMoving == false)
    //    {
    //        //pawnSelectionMenu.SetActive(true);
    //    }
    //    isPressed = false;
    //    isMoving = false;
    //}
}
