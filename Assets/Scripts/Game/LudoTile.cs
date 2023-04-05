using System.Collections.Generic;
using UnityEngine;

public class LudoTile
{
    public Piece[] pieces;
    public Transform tileTransform;

    public void AddPiece(Piece piece)
    {
        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] == null)
            {
                pieces[i] = piece;
                RepositionPieces();
                break;
            }
        }
    }
    public void RemovePiece(Piece piece)
    {
        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] == piece)
            {
                pieces[i] = null;
                RepositionPieces();
                break;
            }
        }
    }
    public void RepositionPieces()
    {
        // If there is no more pieces on the board
        int pieceCount = PieceCount();
        if (pieceCount == 0)
            return;

        // Create a list with all the pieces from this tile
        List<Piece> ps = new List<Piece>();
        for (int i = 0; i < pieces.Length; i++)
            if (pieces[i] != null)
                ps.Add(pieces[i]);

        // Position all pieces based on count
        switch (pieceCount)
        {
            case 1:
                ps[0].PositionClientRpc(tileTransform.position);
                break;

            case 2:
                //ps[0].PositionClientRpc(tileTransform.position + (tileTransform.right * 0.25f));
                ps[0].PositionClientRpc(tileTransform.position + tileTransform.right * 0.95f);
                ps[1].PositionClientRpc(tileTransform.position + (-tileTransform.right * 0.95f));
                break;

            case 3:
                ps[0].PositionClientRpc(tileTransform.position + (tileTransform.right * 0.95f));
                ps[1].PositionClientRpc(tileTransform.position + (-tileTransform.right * 0.95f));
                ps[2].PositionClientRpc(tileTransform.position + (-tileTransform.up * 0.95f));
                break;

            case 4:
                ps[0].PositionClientRpc(tileTransform.position + (tileTransform.right * 0.95f));
                ps[1].PositionClientRpc(tileTransform.position + (-tileTransform.right * 0.95f));
                ps[2].PositionClientRpc(tileTransform.position + (tileTransform.up * 0.95f));
                ps[3].PositionClientRpc(tileTransform.position + (-tileTransform.up * 0.95f));
                break;
        }
    }
    public int PieceCount()
    {
        // Count the pieces on this tile
        int r = 0;
        for (int i = 0; i < pieces.Length; i++)
            if (pieces[i] != null)
                r++;

        return r;
    }
    public Piece GetFirstPiece()
    {
        // Return the first piece it finds
        for (int i = 0; i < pieces.Length; i++)
            if (pieces[i] != null)
                return pieces[i];

        return null;
    }

    public Piece GetEnemyPiece(Piece myPiece)
    {
        for (int i = 0; i < pieces.Length; i++)
            if (pieces[i] != null && myPiece.currentTeam != pieces[i].currentTeam)
            {
                Debug.Log($"{pieces[i]} {pieces[i].currentTeam}");
                return pieces[i];
            }
        return null;
    }

}
