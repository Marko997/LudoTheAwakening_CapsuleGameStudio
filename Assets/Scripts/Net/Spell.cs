using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;

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

        switch (spell)
        {
            case SpellType.ARCHER:
                spellCasterPiece.eatPower = 3;
                //Debug.Log(targetTile);
                //Debug.Log(spellCasterPiece.currentTile);
                EatPieceServerRpc(targetTile, spellCasterPiece,board);
                spellCasterPiece.UpdateAnimationStateServerRpc(AnimationState.Idle);
                break;
            case SpellType.SPEARMAN:
                spellCasterPiece.eatPower = 1;
                //Debug.Log(targetTile);
                //Debug.Log(spellCasterPiece.currentTile);
                EatPieceServerRpc(targetTile, spellCasterPiece, board);
                //spellCasterPiece.UpdateAnimationStateServerRpc(AnimationState.Idle);
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
        //spellCasterPiece.animator.SetBool("isAttacking",false);
        //spellCasterPiece.UpdateAnimationStateServerRpc(AnimationState.Idle);
    }
    [ServerRpc]
    public void EatPieceServerRpc(int targetTile, Piece spellCasterPiece, LudoTile[] board) //it was static I don't know way
    {
        //Piece p = board[targetTile].GetFirstPiece();

        //if (p != null && p.currentTeam != spellCasterPiece.currentTeam)
        //{
        //    p.UpdateAnimationStateServerRpc(AnimationState.Death);
        //    board[targetTile].RemovePiece(p);
        //    p.currentTile = -1;
        //    p.isOut = false;
        //    p.routePosition = 0;
        //    p.PositionClientRpc(-Vector3.one); // start position is set localy
        //}
        Piece[] pList = board[targetTile].GetEnemyPieces(spellCasterPiece);

        if (pList.Count() == 0) { return; }

        foreach (var p in pList)
        {
            if (p != null && p.currentTeam != spellCasterPiece.currentTeam)
            {
                p.UpdateAnimationStateServerRpc(AnimationState.Death);
                board[targetTile].RemovePiece2(p, spellCasterPiece);
                p.currentTile = -1;
                p.isOut = false;
                p.routePosition = 0;
                p.PositionClientRpc(-Vector3.one);
                p.transform.Rotate(0, Utility.TeamToRotataion(p.currentTeam), 0);
            }

        }

        //spellCasterPiece.UpdateAnimationStateServerRpc(AnimationState.Idle);
        StartCoroutine(WaitForAttackToFinish(spellCasterPiece));
    }

    IEnumerator WaitForAttackToFinish(Piece p)
    {
        yield return new WaitForSeconds(0.8f);
        p.UpdateAnimationStateServerRpc(AnimationState.Idle);
    }
}
