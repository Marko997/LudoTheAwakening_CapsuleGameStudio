using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InitializeAds : MonoBehaviour, IUnityAdsInitializationListener
{
    public string androidGameId;
    public string iosGameId;

    public bool isTestingMode = false;

    string gameId;

    void AdsInitialization()
    {
        gameId = androidGameId;
        gameId = iosGameId;

#if UNITY_IOS
        gameId = iosGameId;
#elif UNITY_ANDROID
        gameId = androidGameId;
#elif UNITY_EDITOR
        gameId = androidGameId; //for testing
#endif
        if(!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId,isTestingMode, this);
        }

    }

    void IUnityAdsInitializationListener.OnInitializationComplete()
    {
        print("Ads init");
    }

    void IUnityAdsInitializationListener.OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        print("Ads failed");
    }

    // Start is called before the first frame update
    void Awake()
    {
        AdsInitialization();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
