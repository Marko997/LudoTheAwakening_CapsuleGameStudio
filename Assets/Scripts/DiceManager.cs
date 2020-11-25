using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    static Rigidbody rb;
    public static Vector3 diceVelocity;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        diceVelocity = rb.velocity;

        if(Input.GetKeyDown (KeyCode.Space)){
            DiceNumberText.diceNumber = 0;
            float dirX = Random.Range(0,500);
            float dirY = Random.Range(0,500);
            float dirZ = Random.Range(0,500);
            transform.position = new Vector3(-16,0,17);
            transform.rotation = Quaternion.identity;
            rb.AddForce (transform.up*3000);
            rb.AddTorque (dirX, dirY, dirZ);
        }
    }

}
