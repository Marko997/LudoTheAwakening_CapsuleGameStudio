using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonRouteManager : MonoBehaviour
{
    Transform[] childNodes;

    public List<Transform> childNodesList = new List<Transform>();


    void Start()
    {
        FillNodes();
    }

    void OnDrawGizmos()
    {
       //Gizmos.color = Color.green; 
       FillNodes();
       for( int i = 0; i<childNodesList.Count;i++){

           Vector3 pos = childNodesList[i].position;

           if(i>0)
           {
               Vector3 prev = childNodesList[i - 1].position;
               //Gizmos.DrawLine(prev,pos);
           }

       }
    }

    void FillNodes(){
        childNodesList.Clear();
        childNodes = GetComponentsInChildren<Transform>();

        foreach(Transform child in childNodes){
            if(child != this.transform){
                
                childNodesList.Add(child);
            }
        }
    }

    public int RequestPosition(Transform nodeTransform){
        return childNodesList.IndexOf(nodeTransform);
    }
}
