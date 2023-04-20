using UnityEngine;

public enum Team
{
    Red = 0,
    Green = 1,
    Yellow = 3,
    Blue = 2
}

public class Utility : MonoBehaviour
{
    public static int GetPieceIndex(Piece[] a, Piece p)
    {
        // Get the index of the piece in the piece[]
        for (int i = 0; i < a.Length; i++)
        {
            if (p == a[i])
                return i;
        }

        return -1;
    }

    public static int RetrieveTeamId(ulong clientId)
    {
        //Debug.Log(clientId);
        // Utility to swap the clientID 0 to 1, then -1 for arrays usage
        return (clientId == 0) ? 0 : (int)(clientId); //removec -1 from (clientId -1) because it was returning 0 
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
    public static int TeamToMaterial(Team t)
    {
        switch (t)
        {
            case Team.Red:
                return 0;
            case Team.Green:
                return 1;
            case Team.Yellow:
                return 3;
            case Team.Blue:
                return 2;
            default:
                return -1;
        }
    }

    public static Quaternion TeamToRotataion(Team t)
    {
        switch (t)
        {
            case Team.Red:
                return new Quaternion(0, 180, 0,0);
            case Team.Green:
                return new Quaternion(0, -90, 0,0);
            case Team.Yellow:
                return new Quaternion(0, 90, 0,0);
            case Team.Blue:
                return new Quaternion(0,0,0,0);
            default:
                return new Quaternion(0, 180, 0,0);
        }
    }
}
