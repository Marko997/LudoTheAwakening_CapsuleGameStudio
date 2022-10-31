using UnityEngine;

public enum Team
{
    Red = 0,
    Green = 1,
    Yellow = 2,
    Blue = 3
}


public class Utility : MonoBehaviour
{
    public static int GetPieceIndex(Piece[] a, Piece p)
    {
        for (int i = 0; i < a.Length; i++)
        {
            if(p == a[i])
            {
                return i;
            }
        }
        return -1;
    }

    public static int RetrieveTeamId(int clientId)
    {
        return (clientId == 0) ? 0 : (clientId - 1);
    }

    public static Color TeamToColor(Team t)
    {
        switch (t)
        {
            case Team.Red:
                return Color.red;
            case Team.Green:
                return Color.green;
            case Team.Yellow:
                return Color.yellow;
            case Team.Blue:
                return Color.blue;
            default:
                return Color.white;
        }
    }
}
