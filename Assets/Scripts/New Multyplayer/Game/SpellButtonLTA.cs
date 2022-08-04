using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;

public class SpellButtonLTA : NetworkBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public bool pressed;
    public bool visible;

    public SpellButtonLTA spellButton;

    private void Update()
    {
        if (spellButton.visible)
        {
            spellButton.gameObject.SetActive(true);
        }
        else
        {
            spellButton.gameObject.SetActive(true);//??????
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
        NetworkGameManagerLTA.instance.PlaySpell();//videti da se spell radi u posebnoj spell skripti na pawnu a ne u game manageu
    }
}
