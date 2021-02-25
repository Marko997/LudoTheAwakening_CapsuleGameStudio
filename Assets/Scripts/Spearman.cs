using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spearman : PawnManager
{
    public string pClass = "Spearman";
    public GameObject powerButton;
    
     private void Start() {
        startNodeIndex = commonRoute.RequestPosition(startNode.gameObject.transform);
        CreateFullRoute();

        SetSelector(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {

        if (hasTurn)
        {

            if (!isOut)
            {

                LeaveBase();

            }
            else
            {

                StartTheMove(GameManager.instance.rolledHumanDice);
            }

            GameManager.instance.DeactivateAllSelectors();
        }

    }
}
