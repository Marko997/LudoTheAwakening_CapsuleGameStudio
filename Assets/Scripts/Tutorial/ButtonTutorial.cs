using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTutorial : Tutorial
{
    public List<Button> Buttons = new List<Button>(); 

    public override void CheckIfHappening()
    {
        for (int i = 0; i < Buttons.Count; i++)
        {
            if (Buttons[i].GetComponent<CustomTutorialButton>().isClicked)
            {
                Buttons.RemoveAt(i);
                break;
            }
        }
        if (Buttons.Count == 0)
        {
            TutorialManager.Instance.CompletedTutorial();
        }
    }
}
