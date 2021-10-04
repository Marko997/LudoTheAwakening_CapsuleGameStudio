using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Grounds
{
    Sand,
    Grass
}

public class GridVisualizer : MonoBehaviour
{
    public Grounds grounds;
    public GameObject groundPrefab;
    public GameObject groundPrefab1;

    public void VisualizeGrid(int width, int length)
    {
        Vector3 position = new Vector3(width, 0, length);

        if( grounds == Grounds.Sand)
        {
            Instantiate(groundPrefab1, position, Quaternion.identity);
        }
        if(grounds == Grounds.Grass)
        {
            Instantiate(groundPrefab, position, Quaternion.identity);
        }

        
    }
}
