using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public int Order;
    public string Explanation;
    public GameObject popUp;
    public Button buttonToPress;


    private void Awake()
    {
        TutorialManager.Instance.Tutorials.Add(this);
    }

    public virtual void CheckIfHappening()
    {

    }
}
