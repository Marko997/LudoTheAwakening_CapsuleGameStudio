using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro; 

public class GetPlayerInfo : MonoBehaviour
{
    public TMP_Text displayNameText;
    public TMP_Text playerIdText;

    public TMP_Text errorChangeName;

    //bool isDataRecieved;

    public void Update()
    {
        //if (isDataRecieved) { return; }
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
        Debug.Log("Error: " + error.ErrorMessage);//getting account info
        errorChangeName.text = error.ErrorMessage;
    }

    public void OnChangeNameClicked()
    { 
        displayNameText.text = "Name: " + PlayerPrefs.GetString("NAME");
        Debug.Log(PlayerPrefs.GetString("NAME"));
    }
}