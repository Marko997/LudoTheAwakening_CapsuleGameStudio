using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Spell : MonoBehaviour
{
    public enum SpellType{
        ARCHER,
        SPEARMAN,
        SLINGSHOOTMAN,
        MACEBEARER
    }

    public SpellType spell;

    public void CastSpell(int targetTile, LudoTile[] board)
    {
        Piece spellCasterPiece = gameObject.GetComponent<Piece>();
        if (!spellCasterPiece.isOut) { return; }

        Debug.Log("SPELL CASTED");

        switch (spell)
        {
            case SpellType.ARCHER:
                spellCasterPiece.eatPower = 3;
                //Debug.Log(targetTile);
                //Debug.Log(spellCasterPiece.currentTile);
                EatPieceServerRpc(targetTile, spellCasterPiece,board);
                break;
            case SpellType.SPEARMAN:
                spellCasterPiece.eatPower = 1;
                //Debug.Log(targetTile);
                //Debug.Log(spellCasterPiece.currentTile);
                EatPieceServerRpc(targetTile, spellCasterPiece, board);
                break;
            case SpellType.SLINGSHOOTMAN:
                spellCasterPiece.eatPower = 2;
                EatPieceServerRpc(targetTile, spellCasterPiece, board);
                break;
            case SpellType.MACEBEARER:
                spellCasterPiece.eatPower = -1;
                EatPieceServerRpc(targetTile, spellCasterPiece, board);
                break;
            default:
                Debug.Log("Spell not found");
                break;
        }
        spellCasterPiece.animator.SetBool("isAttacking",false);
    }
    [ServerRpc]
    private static void EatPieceServerRpc(int targetTile, Piece spellCasterPiece, LudoTile[] board)
    {
        Piece p = board[targetTile].GetFirstPiece();

        if (p != null && p.currentTeam != spellCasterPiece.currentTeam)
        {
            spellCasterPiece.board[targetTile].RemovePiece(p);
            p.currentTile = -1;
            p.isOut = false;
            p.routePosition = 0;
            p.PositionClientRpc(-Vector3.one); // start position is set localy
        }
    }
}
