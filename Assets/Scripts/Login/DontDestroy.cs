using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        DontDestroyOnLoad(gameObject);
    }
}
