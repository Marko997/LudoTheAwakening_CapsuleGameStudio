using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spearman : PawnManager
{
    public string pClass = "Spearman";
    
     private void Start() {
        startNodeIndex = commonRoute.RequestPosition(startNode.gameObject.transform);
        CreateFullRoute();

        SetSelector(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
