using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.EconomyModels;
using TMPro; 

public class GetPlayerInfo : MonoBehaviour
{
    public TMP_Text displayNameText;
    public TMP_Text playerIdText;

    public TMP_Text errorChangeName;

    public TMP_Text coinText;

    //bool isDataRecieved;

    public void Start()
    {
        //if (isDataRecieved) { return; }
        GetAccountInfoRequest request = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(request, OnGetAccountInfoSuccess, OnGetAccountInfoError);
    }

    public void GetVirtualCurrencies()
    {
        PlayFabEconomyAPI.GetInventoryItems(new GetInventoryItemsRequest {
            Filter = "type eq 'currency'"

        },OnGetUserInventorySuccess,OnError);
    }

    void OnGetUserInventorySuccess(GetInventoryItemsResponse response)
    {
        for (int i = 0; i < response.Items.Count; i++)
        {
            if (response.Items[i].StackId == "LCN")
            {
                int coins = response.Items[i].Amount;
                coinText.text = $"{coins}";
            }
        }
    }
    void OnError(PlayFabError error)
    {
        Debug.Log("Error " + error.ErrorMessage);
    }



    void OnGetAccountInfoSuccess(GetAccountInfoResult result)
    {
        GetVirtualCurrencies();
        string displayName = result.AccountInfo.TitleInfo.DisplayName;
        string playerId = result.AccountInfo.PlayFabId;

        PlayerPrefs.SetString("NAME", displayName);

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