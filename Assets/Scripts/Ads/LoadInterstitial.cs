using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class LoadInterstitial : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public string androidAdUnityId;
    public string iosAdUnityId;

    string adUnitId;

    private void Awake()
    {
#if UNITY_IOS
        adUnitId = iosAdUnityId;
#elif UNITY_ANDROID
        adUnitId = androidAdUnityId;
#endif
    }

    public void LoadInterstitialAd()
    {
        print("Interstitial add loaded");
        Advertisement.Load(adUnitId, this);
    }

    public void ShowAd()
    {
        print("Showing ad");
        Advertisement.Show(adUnitId, this);
    }



    public void OnUnityAdsAdLoaded(string placementId)
    {
        print("Interstitial loaded");
        ShowAd();
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        print("Interstitial failed to load");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        print("Interstitial clicked");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        print("Interstitial completed");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        print("Interstitial show failed");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        print("Interstitial show start");
    }
}
