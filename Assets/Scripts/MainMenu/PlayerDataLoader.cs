using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerDataLoader : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Sprite playerImage;
    [SerializeField] private TMP_Text playerId;

    [SerializeField] private PlayerData playerData; 


    // Start is called before the first frame update
    void Awake()
    {
        playerData = FindObjectOfType<PlayerData>();
        playerName.text = playerData.PlayerName;
        playerId.text = playerData.PlayerId;
        Debug.Log(playerName.text);
        Debug.Log(playerId.text);
    }

}
