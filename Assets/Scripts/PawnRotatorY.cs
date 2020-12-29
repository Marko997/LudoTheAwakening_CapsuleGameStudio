using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PawnRotatorY : MonoBehaviour, IPointerUpHandler,IPointerDownHandler
{
    bool isPressed;
    bool isMoving;

    Touch touch;

    [SerializeField]
    Transform pawnHolder;

    [SerializeField]
    GameObject pawnSelectionMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isPressed){
            if(Input.touchCount > 0){

                touch = Input.GetTouch(0);

                if(touch.phase == TouchPhase.Moved){

                        pawnHolder.Rotate(0,-touch.deltaPosition.x,0);
                        
                    
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
        if(isMoving == false){
            pawnSelectionMenu.SetActive(true);
        }
        isPressed = false;
        isMoving = false;
    }
}
