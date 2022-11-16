using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Collections;

//using System.Collections.Generic;

//using UnityEngine;

using UnityEngine.UI;
public class TestVibrate : MonoBehaviour
{

    public void VibrateToggle(bool vibrate)
    {

        //if (vibrate)
        //{
        //    Handheld.Vibrate();
        //    AudioListener.volume = 0;
        //}

        //else
        //{
        //    AudioListener.volume = 1;
        //}
        if (vibrate)
        {

            Handheld.Vibrate();
            AudioListener.volume = 0;
            
        }
        else
        {
            AudioListener.volume = 1;
        }
        
    }
}
