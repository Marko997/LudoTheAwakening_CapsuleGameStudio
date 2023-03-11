using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static bool isUIElementPressed = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isUIElementPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isUIElementPressed = false;
    }
}

