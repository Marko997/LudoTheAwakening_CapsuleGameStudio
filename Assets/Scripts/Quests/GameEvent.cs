using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent
{
    public string EventDescription;

}
public class PlayGameEvent : GameEvent
{
    public string NumberOfGames;
    public PlayGameEvent(string number)
    {
        NumberOfGames = number;
    }


}
