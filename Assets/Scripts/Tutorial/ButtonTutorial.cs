using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTutorial : Tutorial
{
    public List<GameObject> Buttons = new List<GameObject>();
    bool isHappening = false;

    public override void CheckIfHappening()
    {
        if (isHappening) { return; }

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
            StartCoroutine(WaitForScreen());
            isHappening = true;
        }
        
    }
    IEnumerator WaitForScreen()
    {
        yield return new WaitForSeconds(0.12f);
        TutorialManager.Instance.CompletedTutorial();

    }
}
