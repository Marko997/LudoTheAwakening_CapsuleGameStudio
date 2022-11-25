using UnityEngine;

public class ChekDevice : MonoBehaviour
{
    public GameObject AndroidLogin;
    public GameObject IosLogin;

    void Start()
    {
    #if UNITY_ANDROID
        AndroidLogin.SetActive(true);
        IosLogin.SetActive(false);
    #endif
    #if UNITY_IPHONE
        AndroidLogin.SetActive(false);
        IosLogin.SetActive(true);
    #endif
    }
}
