using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerEntityLTA : NetworkBehaviour
{
	[SyncVar]
	[SerializeField] public string playerName;
	public enum PlayerColors
	{
		BLUE,
		RED,
		YELLOW,
		GREEN
	}
	public PawnManager[] allPawns;
	public bool hasTurn;
	public enum PlayerTypes
	{
		HUMAN,
		BOT,
		NO_PLAYER
	}
	public PlayerTypes playerTypes;
	[SyncVar(hook = nameof(HandleColorChanged))]
	public PlayerColors playerColors;
	public bool hasWon;

	private PlayerColors lastNumber;

	private NetworkManagerLTA room;

	private NetworkManagerLTA Room
	{
		get
		{
			if (room != null) { return room; }
			return room = NetworkManager.singleton as NetworkManagerLTA;
		}
	}

	public override void OnStartServer()
    {
		UpdateColors();

	}

    #region Color Updates
    private void HandleColorChanged(PlayerColors oldValue, PlayerColors newValue) => UpdateColors();

	private void UpdateColors()
    {
        if (!isServer) { return; }
        if (!hasAuthority)
        {
            foreach (var player in NetworkGameManagerLTA.instance.playerList)
            {
                if (player.hasAuthority)
                {
                    player.UpdateColors();
                    break;
                }
            }

            return;
        }

        for (int i = 0; i < NetworkGameManagerLTA.instance.playerList.Count; i++)
		{
			playerColors = GetRandom();	
		}
	}

	PlayerColors GetRandom() //Return random color only if color isn't already used by another player
	{
		PlayerColors rand = (PlayerColors)Random.Range(0, 3);
		while (rand == lastNumber)
			rand = (PlayerColors)Random.Range(0, 3);
		lastNumber = rand;
		return rand;
	}
	#endregion

}
