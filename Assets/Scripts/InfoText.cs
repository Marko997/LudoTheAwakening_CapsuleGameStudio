using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoText : MonoBehaviour
{
    public static InfoText instance;
    
    public Text infoText;

    private void Awake() {
        instance = this;
        infoText.text = "";
    }

    public void ShowMessage(string message){
        
        infoText.text = message;
    }
    
}
