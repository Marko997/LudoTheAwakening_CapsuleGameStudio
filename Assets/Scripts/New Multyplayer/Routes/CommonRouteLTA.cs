using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CommonRouteLTA : NetworkBehaviour
{
    Transform[] childNodes;

    public List<Transform> childNodesList = new List<Transform>();

    public override void OnStartServer()
    {
        FillNodes();
    }

    private void FillNodes()
    {
        childNodesList.Clear();
        childNodes = GetComponentsInChildren<Transform>();

        foreach (Transform child in childNodes)
        {
            if (child != this.transform)
            {

                childNodesList.Add(child);
            }
        }
    }
    public int RequestPosition(Transform nodeTransform)
    {
        return childNodesList.IndexOf(nodeTransform);
    }
}
