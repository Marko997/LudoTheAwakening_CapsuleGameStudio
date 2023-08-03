using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGameGoal : Quest.QuestGoal
{
    public string NumberOfGamesToPlay;

    public override string GetDescription()
    {
        return $"Play {NumberOfGamesToPlay} games";
    }

    public override void Initialize()
    {
        base.Initialize();
        EventManager.Instance.AddListener<PlayGameEvent>(OnPlay);
    }



    public void OnPlay(PlayGameEvent eventInfo)
    {
        if (eventInfo.NumberOfGames == NumberOfGamesToPlay)
        {
            CurrentAmount++;
            Evaluate();
        }
    }
}
