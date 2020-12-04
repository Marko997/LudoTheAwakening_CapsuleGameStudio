using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceController : MonoBehaviour
{
    public Rigidbody rb;

    public bool hasLanded;
    public bool thrown;

    public Vector3 initPosition;

    public int diceValue;

    public DiceButton button;

    public DiceSide[] diceSides;

    public void Start() {
        rb = GetComponent<Rigidbody>();
        button = FindObjectOfType<DiceButton>();
        initPosition = transform.position;
        rb.useGravity = false;
    }

    public void Update() {
        // if(button.pressed){
        //     RollDice();
        // }

        // if(rb.IsSleeping() && !hasLanded && thrown){
        //     hasLanded = true;
        //     rb.useGravity = false;
        //     SideValueCheck();
  

        // }else if(rb.IsSleeping() && hasLanded && diceValue ==0){
        //     RollAgain();
        // }
    }

    public void RollDice(){
        if(!thrown && !hasLanded){
            thrown = true;
            rb.useGravity = true;
            rb.AddTorque(Random.Range(0,500),Random.Range(0,500),Random.Range(0,500));

        
        }else if(thrown && hasLanded){
            Reset();
        }
    }

    public void Reset(){
        transform.position = initPosition;
        thrown = false;
        hasLanded = false;
        rb.useGravity = false;

    }

    public void RollAgain(){
        Reset();
        thrown = true;
        rb.useGravity = true;
        rb.AddTorque(Random.Range(0,500),Random.Range(0,500),Random.Range(0,500));

    }

    public void SideValueCheck(){
        diceValue = 0;
        foreach(DiceSide side in diceSides){
            if(side.OnGround()){
                diceValue = side.sideValue;
            }
        }
    }


    


}
