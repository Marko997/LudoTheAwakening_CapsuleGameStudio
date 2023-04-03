using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class TimerSystem : NetworkBehaviour
{
    [SerializeField] private Image uiFill;

    private int duration;
    private int remainingDuration;

    //GameManager gm;

}
