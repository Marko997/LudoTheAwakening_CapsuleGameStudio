using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatSystem : MonoBehaviour
{
    public Button btn;
    bool isOn;

    public GameObject cheatButtons;


    // Start is called before the first frame update
    void Start()
    {
        isOn = false;
        btn.image.color = Color.red;
        btn.onClick.AddListener(ChangeButton);
    }

    private void ChangeButton()
    {
        if (isOn)
        {
            btn.image.color = Color.red;
            cheatButtons.SetActive(false);
            isOn = false;
        }
        else
        {
            btn.image.color = Color.green;
            cheatButtons.SetActive(true);
            isOn = true;
        }
    }
}
