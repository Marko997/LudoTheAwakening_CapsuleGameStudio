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
    #region Multiplayer
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
                //spellCasterPiece.UpdateAnimationStateServerRpc(AnimationState.Idle);
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
                spellCasterPiece.transform.Rotate(0,-180,0);
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

        if(p.pieceName == "Macebearer")
        {
            p.transform.Rotate(0, 180, 0);
        }
    }
    #endregion

    #region Bots
    public void CastBotSpell()
    {
        Pawn spellCasterPiece = gameObject.GetComponent<Pawn>();
        if (!spellCasterPiece.isOut) { return; }

        Node eatNode = null;

        switch (spell)
        {
            case SpellType.ARCHER:
                spellCasterPiece.eatPower = 3;

                eatNode = spellCasterPiece.fullRoute[spellCasterPiece.routePosition + spellCasterPiece.eatPower];

                if (eatNode.isTaken)
                {
                    if (spellCasterPiece.pawnId != eatNode.pawn.pawnId)
                    {
                        var currentRotation = transform.rotation;
                        spellCasterPiece.RotatePawn(eatNode.pawn.currentNode.transform.position);
                        //KICK THE OTHER STONE
                        Debug.Log($"{spellCasterPiece} has attacked {eatNode.pawn}");
                        eatNode.pawn.ReturnToBase();
                        eatNode.isTaken = false;
                        eatNode.pawn = null;
                        
                    }
                }

                break;
            case SpellType.SPEARMAN:
                spellCasterPiece.eatPower = 1;

                eatNode = spellCasterPiece.fullRoute[spellCasterPiece.routePosition + spellCasterPiece.eatPower];

                if (eatNode.isTaken)
                {
                    if (spellCasterPiece.pawnId != eatNode.pawn.pawnId)
                    {
                        spellCasterPiece.RotatePawn(eatNode.pawn.currentNode.transform.position);
                        Debug.Log($"{spellCasterPiece} has attacked {eatNode.pawn}");
                        //KICK THE OTHER STONE
                        eatNode.pawn.ReturnToBase();
                        eatNode.isTaken = false;
                        eatNode.pawn = null;
                    }
                }
                break;
            case SpellType.SLINGSHOOTMAN:
                spellCasterPiece.eatPower = 2;

                eatNode = spellCasterPiece.fullRoute[spellCasterPiece.routePosition + spellCasterPiece.eatPower];

                if (eatNode.isTaken)
                {
                    if (spellCasterPiece.pawnId != eatNode.pawn.pawnId)
                    {
                        spellCasterPiece.RotatePawn(eatNode.pawn.currentNode.transform.position);
                        Debug.Log($"{spellCasterPiece} has attacked {eatNode.pawn}");
                        //KICK THE OTHER STONE
                        eatNode.pawn.ReturnToBase();
                        eatNode.isTaken = false;
                        eatNode.pawn = null;
                    }
                }
                break;
            case SpellType.MACEBEARER:

                spellCasterPiece.eatPower = -1;

                eatNode = spellCasterPiece.fullRoute[spellCasterPiece.routePosition + spellCasterPiece.eatPower];

                if (eatNode.isTaken)
                {
                    if (spellCasterPiece.pawnId != eatNode.pawn.pawnId)
                    {
                        spellCasterPiece.RotatePawn(eatNode.pawn.currentNode.transform.position);
                        Debug.Log($"{spellCasterPiece} has attacked {eatNode.pawn}");
                        //KICK THE OTHER STONE
                        eatNode.pawn.ReturnToBase();
                        eatNode.isTaken = false;
                        eatNode.pawn = null;
                    }
                }
                break;
            default:
                Debug.Log("Spell not found");
                break;
        }
        StartCoroutine(WaitForAttackAnimation(spellCasterPiece));
        
    }

    IEnumerator WaitForAttackAnimation(Pawn spellCasterPiece)
    {
        yield return new WaitForSeconds(1);
        spellCasterPiece.ChangeAnimationState(PawnAnimationState.Idle);
        spellCasterPiece.isSelected = false;
        //spellCasterPiece.RotatePawn(currentNode.transform.position);
    }
    #endregion
}
