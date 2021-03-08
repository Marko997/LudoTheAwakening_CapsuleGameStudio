using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
	//private string redName, greenName, blueName, yellowName;
    //------------------RED-------------------------------------------
    public void SetRedHumanType(bool on)
	{
		if (on)
		{
			SaveSettings.players[0] = "HUMAN";
		}
	}
	public void SetRedBotType(bool on)
	{
		if (on)
		{
			SaveSettings.players[0] = "BOT";
		}
	}
	public void SetRedName(string name)
	{

			SaveSettings.playerNames[0] = name;
		
	}

	//------------------GREEN-------------------------------------------
	public void SetGreenHumanType(bool on)
	{
		if (on)
		{
			SaveSettings.players[1] = "HUMAN";
		}
	}
	public void SetGreenBotType(bool on)
	{
		if (on)
		{
			SaveSettings.players[1] = "BOT";
		}
	}
	public void SetGreenName(string name)
	{

		SaveSettings.playerNames[1] = name;

	}

	//------------------BLUE-------------------------------------------
	public void SetBlueHumanType(bool on)
	{
		if (on)
		{
			SaveSettings.players[2] = "HUMAN";
		}
	}
	public void SetBlueBotType(bool on)
	{
		if (on)
		{
			SaveSettings.players[2] = "BOT";
		}
	}
	public void SetBlueName(string name)
	{

		SaveSettings.playerNames[2] = name;

	}

	//------------------YELLOW-------------------------------------------
	public void SetYellowHumanType(bool on)
	{
		if (on)
		{
			SaveSettings.players[3] = "HUMAN";
		}
	}
	public void SetYellowBotType(bool on)
	{
		if (on)
		{
			SaveSettings.players[3] = "BOT";
		}
	}
	public void SetYellowName(string name)
	{

		SaveSettings.playerNames[3] = name;

	}
	public void SetAllYellowPawns(bool on)
	{
		if (on)
		{
			SaveSettings.yellowPawns[0] = "Pawn";
		}
	}
}

public static class SaveSettings
{
    //Red Green Blue Yellow
    public static string[] players = new string[4];
	public static string[] playerNames = new string[4];
	public static string[] yellowPawns = new string[4];

	public static string[] winners = new string[3] { string.Empty, string.Empty,string.Empty};
}
