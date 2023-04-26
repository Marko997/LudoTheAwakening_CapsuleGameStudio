using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButtonParticleController : MonoBehaviour
{
    public GameObject effect;

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<Button>().interactable == true)
        {
            effect.SetActive(true);
        }
        else
        {
            effect.SetActive(false);
        }
    }
}
