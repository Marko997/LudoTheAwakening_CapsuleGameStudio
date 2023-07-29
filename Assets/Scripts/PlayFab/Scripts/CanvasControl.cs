using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class CanvasControl : MonoBehaviour
{
    public GameObject canvas;

    void Start()
    {
        PlayFabClientAPI.OnLoginSuccess += CheckDisplayName;
    }

    void CheckDisplayName(LoginResult result)
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccountInfoSuccess, OnGetAccountInfoFailure);
    }

    void OnGetAccountInfoSuccess(GetAccountInfoResult result)
    {
        if (string.IsNullOrEmpty(result.AccountInfo.PlayFabId))
        {
            canvas.SetActive(true);
        }
        else
        {
            canvas.SetActive(false);
        }
    }

    void OnGetAccountInfoFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
}
