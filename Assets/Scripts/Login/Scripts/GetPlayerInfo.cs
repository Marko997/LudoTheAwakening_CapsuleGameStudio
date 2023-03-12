using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class GetPlayerInfo : MonoBehaviour
{
    public Text displayNameText;
    public Text playerIdText;

    public void Update()
    {
        GetAccountInfoRequest request = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(request, OnGetAccountInfoSuccess, OnGetAccountInfoError);
    }

    void OnGetAccountInfoSuccess(GetAccountInfoResult result)
    {
        string displayName = result.AccountInfo.TitleInfo.DisplayName;
        string playerId = result.AccountInfo.PlayFabId;

        displayNameText.text = "Name: " + displayName;
        playerIdText.text = "ID: " + playerId;
    }

    void OnGetAccountInfoError(PlayFabError error)
    {
        Debug.Log("Error getting account info: " + error.ErrorMessage);
    }
}