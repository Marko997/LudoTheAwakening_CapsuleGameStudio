using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro; 

public class GetPlayerInfo : MonoBehaviour
{
    public TMP_Text displayNameText;
    public TMP_Text playerIdText;

    public TMP_Text errorChangeName;



    //bool isDataRecieved;

    #region Singleton
    private static GetPlayerInfo instance;
    public static GetPlayerInfo Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GetPlayerInfo>();
            }
            if (instance == null)
            {
                Debug.Log("There is no PlayFab manager!");
            }
            return instance;
        }
    }
    #endregion

    private void Awake()
    {
        GetAccountInfoRequest request = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(request, OnGetAccountInfoSuccess, OnGetAccountInfoError);

    }

    //public void Start()
    //{
    //    //if (isDataRecieved) { return; }
    //    GetAccountInfoRequest request = new GetAccountInfoRequest();
    //    PlayFabClientAPI.GetAccountInfo(request, OnGetAccountInfoSuccess, OnGetAccountInfoError);
    //}

    void OnGetAccountInfoSuccess(GetAccountInfoResult result)
    {
        PlayFabManager.Instance.GetVirtualCurrencies();
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