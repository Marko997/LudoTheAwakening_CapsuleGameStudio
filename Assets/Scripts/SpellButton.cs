using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{

    public bool pressed;
    public bool visible;

    public SpellButton spellButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spellButton.visible = GameManager.instance.displaySpellButton;
		if (spellButton.visible)
		{
            spellButton.gameObject.SetActive(true);
		}
		else
		{
            spellButton.gameObject.SetActive(false);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
		//GameManager.instance.powerButton.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
        GameManager.instance.PlaySpell();
    }
}
