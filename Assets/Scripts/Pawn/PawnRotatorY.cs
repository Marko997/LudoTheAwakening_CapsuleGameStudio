using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PawnRotatorY : MonoBehaviour, IPointerUpHandler,IPointerDownHandler
{
    bool isPressed;
    bool isMoving;    

    Touch touch;

    [SerializeField]
    Transform pawnHolder;


    // Start is called before the first frame update
   // void Awake()
   // {
   //     for (int i = 0; i < panels.Length; i++)
   //     {
   //         var type = panels[i]._panelType;
   //         if(type == PanelType.panelType.LEADER)
			//{
   //             pawnHolder = Instantiate(loadOutTemplate.leaderPawn, new Vector3(-1.6f, -1f, 0f), Quaternion.identity).GetComponent<Transform>();

   //         }
   //     }
   // }

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
		if (isMoving == false)
		{
			//pawnSelectionMenu.SetActive(true);
		}
		isPressed = false;
		isMoving = false;
	}
}
