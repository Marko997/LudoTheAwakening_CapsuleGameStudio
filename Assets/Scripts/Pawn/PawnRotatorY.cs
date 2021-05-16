using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PawnRotatorY : MonoBehaviour, IPointerUpHandler,IPointerDownHandler
{
    bool isPressed;
    bool isMoving;

    public CanvasManager cManager;

    public SelectionTemplate loadOutTemplate;

    Touch touch;

    [SerializeField]
    public Transform pawnHolder;


    // Start is called before the first frame update
    void Awake()
    {

        //pawnHolder = Instantiate(loadOutTemplate.leaderPawn,Vector3.zero, Quaternion.identity).GetComponent<Transform>();

    }

    // Update is called once per frame
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

        

        //if (canvasControllers[i].canvasType != CanvasType.MainMenu)
        //{
        //    pawnHolder.gameObject.SetActive(false);
        //}
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
