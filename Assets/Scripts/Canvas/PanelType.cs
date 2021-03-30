using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelType : MonoBehaviour
{
    public enum panelType
	{
		LEADER,
		SECONDPAWN,
		THIRDPAWN,
		FOURTHPAWN
	}

	public panelType _panelType;
}
