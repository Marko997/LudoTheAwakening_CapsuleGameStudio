using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GridVisualizer gridVisualizer;

    [Range(3,19)]
    public int width, length = 11;
    private MapGrid grid;
    
    void Start()
    {
        grid = new MapGrid(width, length);

        for (int i = 0; i < grid.CellGrid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.CellGrid.GetLength(1); j++)
            {
                gridVisualizer.grounds = Grounds.Sand;
                gridVisualizer.VisualizeGrid(grid.GetCell(i, j).X, grid.GetCell(i, j).Z);
                //gridVisualizer.grounds = Grounds.Grass;
                //gridVisualizer.VisualizeGrid(grid.GetCell(0, 0).X, grid.GetCell(0, 0).Z);
                
            }

        }

    }

}
