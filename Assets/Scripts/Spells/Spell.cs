using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spell
{
    public string name;
    public NodeManager goalNode;
    public int eatPower;

    public Spell (string name, NodeManager goalNode,int eatPower)
	{
        this.name = name;
        this.goalNode = goalNode;
        this.eatPower = eatPower;

	}

    public void Cast()
	{
        Debug.Log("Cast spell");
        //CHECK POSSIBLE KICK

        if (goalNode.isTaken)
        {
            //KICK THE OTHER STONE
            goalNode.pawn.ReturnToBase();
        }
    }
}
