using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.EconomyModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayFabManager : MonoBehaviour
{
    #region Singleton
    private static PlayFabManager instance;
    public static PlayFabManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayFabManager>();
            }
            if (instance == null)
            {
                Debug.Log("There is no PlayFab manager!");
            }
            return instance;
        }
    }
    #endregion

    public TMP_Text coinText;
    public TMP_Text rubyText;

    public string rubyId = "fb8ac24b-c500-44c0-89ff-48758bf9c222";
    public string coinId = "5e20b312-8e7f-44d8-bc92-0083793786bf";

    public void AddVirtualCurrency(string id, int amount)
    {
        PlayFabEconomyAPI.AddInventoryItems(new AddInventoryItemsRequest
        {
            Item = new InventoryItemReference
            {
                Id = id
            },
            Amount = amount
        }, OnAddCurrencySuccess, OnError);
    }

    void OnAddCurrencySuccess(AddInventoryItemsResponse response)
    {
        Debug.Log($"Added {response.TransactionIds[0]} rubies");
        GetVirtualCurrencies();

    }

    public void GetVirtualCurrencies()
    {
        PlayFabEconomyAPI.GetInventoryItems(new GetInventoryItemsRequest
        {
            Filter = "type eq 'currency'"

        }, OnGetUserInventorySuccess, OnError);
    }

    void OnGetUserInventorySuccess(GetInventoryItemsResponse response)
    {
        for (int i = 0; i < response.Items.Count; i++)
        {
            if (response.Items[i].Id == "fb8ac24b-c500-44c0-89ff-48758bf9c222") //ruby
            {
                int rubies = response.Items[i].Amount;
                if(SceneManager.GetActiveScene().name == "MainScene")
                {
                    rubyText.text = $"{rubies}";
                }
                
            }
            if (response.Items[i].Id == "5e20b312-8e7f-44d8-bc92-0083793786bf") // coin
            {
                int coins = response.Items[i].Amount;
                if (SceneManager.GetActiveScene().name == "MainScene")
                {
                    coinText.text = $"{coins}";
                }
                
            }
        }
    }
    void OnError(PlayFabError error)
    {
        Debug.Log("Error " + error.ErrorMessage);
    }
}
