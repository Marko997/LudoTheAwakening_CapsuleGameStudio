using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseHeroSpell : Quest.QuestGoal
{
    public string NumberOfSpecificSpells;
    public string HeroName;

    public override string GetDescription()
    {
        return $"Use {HeroName} spell {NumberOfSpecificSpells} times";
    }

    public override void Initialize()
    {
        base.Initialize();
        EventManager.Instance.AddListener<UseSpellEvent>(OnPlay);
    }



    public void OnPlay(UseSpellEvent eventInfo)
    {
        if (eventInfo.NumberOfUsedSpells == NumberOfSpecificSpells)
        {
            CurrentAmount++;
            Evaluate();
        }
    }
}
