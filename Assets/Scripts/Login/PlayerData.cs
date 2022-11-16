using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerData : MonoBehaviour
{
    private string playerName;
    private string playerId;
    private string country;
    private int age;

    public string PlayerName { get => playerName; set => playerName = value; }
    public string PlayerId { get => playerId; set => playerId = value; }
    public string Country { get => country; set => country = value; }
    public int Age { get => age; set => age = value; }
}
