using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LudoTile : MonoBehaviour
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
        int pieceCount = PieceCount();
        if(pieceCount == 0)
        {
            return;
        }

        List<Piece> ps = new List<Piece>();
        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] != null)
            {
                ps.Add(pieces[i]);
            }
        }

        switch (pieceCount)
        {
            case 1:
                ps[0].PositionClientRpc(tileTransform.position);
                break;
            case 2:
                ps[0].PositionClientRpc(tileTransform.position + (tileTransform.right * 0.25f));
                ps[1].PositionClientRpc(tileTransform.position + (-tileTransform.right * 0.25f));
                break;
            case 3:
                ps[0].PositionClientRpc(tileTransform.position + (tileTransform.right * 0.25f));
                ps[1].PositionClientRpc(tileTransform.position + (-tileTransform.right * 0.25f));
                ps[2].PositionClientRpc(tileTransform.position + (tileTransform.up * 0.25f));
                break;
            case 4:
                ps[0].PositionClientRpc(tileTransform.position + (tileTransform.right * 0.25f));
                ps[1].PositionClientRpc(tileTransform.position + (-tileTransform.right * 0.25f));
                ps[2].PositionClientRpc(tileTransform.position + (tileTransform.up * 0.25f));
                ps[3].PositionClientRpc(tileTransform.position + (-tileTransform.up * 0.25f));
                break;
        }
    }

    public int PieceCount()
    {
        int r = 0;
        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] != null)
            {
                r++;
            }
        }
        return r;
    }

    public Piece GetFirstPiece()
    {
        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] != null)
            {
                return pieces[i];
            }
        }
        return null;
    }
}
