using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGameGoal : Quest.QuestGoal
{
    public string PlayGame;

    public override string GetDescription()
    {
        return $"Play {PlayGame} games";
    }

    public override void Initialize()
    {
        base.Initialize();
        EventManager.Instance.AddListener<PlayGameEvent>(OnPlay);
    }



    public void OnPlay(PlayGameEvent eventInfo)
    {
        if (eventInfo.NumberOfGames == PlayGame)
        {
            CurrentAmount++;
            Evaluate();
        }
    }
}
