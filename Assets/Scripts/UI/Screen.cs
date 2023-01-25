using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Screen : MonoBehaviour //Used for setting screen type on main screen object so Screen Manager can set it to active or not active
{
    [SerializeField] public ScreenType screenType;

    public static float width { get; internal set; }
    public static float height { get; internal set; }

    public ScreenType ScreenType { get => screenType; set => screenType = value; }

}
