using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeroRotatorY : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    bool isPressed;
    bool isMoving;

    Touch touch;

    [SerializeField]
    public Transform pawnHolder;

    void Update()
    {
        if (isPressed)
        {
            if (Input.touchCount > 0)
            {

                touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved)
                {

                    pawnHolder.Rotate(0, -touch.deltaPosition.x, 0);


                    isMoving = true;
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isMoving == false)
        {
            //pawnSelectionMenu.SetActive(true);
        }
        isPressed = false;
        isMoving = false;
    }
}
