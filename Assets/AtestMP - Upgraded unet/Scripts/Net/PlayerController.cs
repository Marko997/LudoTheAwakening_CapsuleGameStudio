using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class PlayerController : NetworkBehaviour
{
    [SyncVar] public string playerName;
    [SyncVar] public Color playerColor;

    private GameObject myPlayerListItem;
    private TextMeshProUGUI playerNameLabel;

    public override void OnStartServer()
    {
        RegisterEvents();

        //myPlayerListItem = Instantiate(LobbyScene.Instance.playerListItemPrefab, Vector3.zero, Quaternion.identity);
        //myPlayerListItem.transform.SetParent(LobbyScene.Instance.playerListContainer, false);

        //playerNameLabel = myPlayerListItem.GetComponentInChildren<TextMeshProUGUI>();

        if (hasAuthority)
        {
            playerName = Random.Range(1000, 9999).ToString();
            playerColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        }
        else
        {
            //playerNameLabel.text = playerName;
        }


    }

    public void OnDestroy()
    {
        Destroy(myPlayerListItem);
        UnregisterEvents();
    }

    private void RegisterEvents()
    {
        //playerName.OnValueChanged += OnPlayerNameChange;
    }

    private void UnregisterEvents()
    {
        //playerName.OnValueChanged -= OnPlayerNameChange;
    }

    private void OnPlayerNameChange(string previousValue, string newValue)
    {
        playerNameLabel.text = playerName;
    }

}

