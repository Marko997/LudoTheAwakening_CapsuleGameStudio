using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.EconomyModels;
using TMPro;
using UnityEngine;

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

    public string rubyId = "d6f4ae84-dd6e-4f9e-b38d-0249fbd3d1a4";
    public string coinId = "f7c88f86-c896-42a2-96fa-ec9f0cab12c9";

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
            if (response.Items[i].Id == "d6f4ae84-dd6e-4f9e-b38d-0249fbd3d1a4") //ruby
            {
                int rubies = response.Items[i].Amount;
                rubyText.text = $"{rubies}";
            }
            if (response.Items[i].Id == "f7c88f86-c896-42a2-96fa-ec9f0cab12c9") // coin
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
}
