using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class PlayerData
{
    private static TMP_Text name;
    private static TMP_Text age;

    public static TMP_Text Name { get => name; set => name = value; }
    public static TMP_Text Age { get => age; set => age = value; }
}
