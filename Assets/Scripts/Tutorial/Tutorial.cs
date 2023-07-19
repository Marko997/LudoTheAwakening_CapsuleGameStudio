using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public int Order;
    [TextArea(2,10)]
    public string Explanation;
    public GameObject popUp;
    public GameObject buttonToPress;


    private void Awake()
    {
        TutorialManager.Instance.Tutorials.Add(this);
    }

    public virtual void CheckIfHappening()
    {

    }
}
