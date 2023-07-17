using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomTutorialButton : MonoBehaviour,IPointerDownHandler, IPointerUpHandler
{
    public bool isClicked = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isClicked = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }
}
