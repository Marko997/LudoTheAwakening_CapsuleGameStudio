using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyCamera : MonoBehaviour
{
    void Update()
    {
        DontDestroyOnLoad(gameObject);
    }
}