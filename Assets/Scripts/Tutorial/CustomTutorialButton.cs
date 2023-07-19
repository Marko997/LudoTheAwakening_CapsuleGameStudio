using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomTutorialButton : MonoBehaviour,IPointerDownHandler, IPointerUpHandler
{
    public bool isClicked = false;
    int numberOfClicks = 0;

    public void OnPointerDown(PointerEventData eventData)
    {
        CheckNumberOfClicks(gameObject);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }

    void CheckNumberOfClicks(GameObject btn)
    {
        if(btn.name == "Character-Close__Button")
        {
            numberOfClicks++;
            if(numberOfClicks == 4)
            {
                isClicked = true;
            }
            return;
        }
        isClicked = true;
    }
}
