using UnityEngine;
using Unity.Netcode;
using System.Collections;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class Piece : NetworkBehaviour
{
    [Header("INFO")]
    public string pieceName;
    public string cardId;
    public Spell spell;
    public Team currentTeam;
    public int currentTile = -1;
    public Vector3 startPosition;
    public int eatPower = 3;
    public int lookTileInt = 0;

    public GameObject pieceCardImage;

    [Header("BOARD MOVEMENT")]
    public int routePosition;
    public int steps;
    float timeForPointToPoint = 0f;
    public bool isMoving = false;
    public bool isOut = false;
    public bool isSelected = false;
    //public LudoTile[] board;
    public Task t;

    public List<LudoTile> board = new List<LudoTile>();

    private Animator animator;


    public override void OnNetworkSpawn()
    {
        animator = GetComponent<Animator>();
        // Assign the start position field to be restored when piece is eaten
        startPosition = transform.position;
        spell = gameObject.GetComponent<Spell>();

        int teamId = Utility.RetrieveTeamId(OwnerClientId);
        currentTeam = (Team)teamId;
        GetComponent<MeshRenderer>().material.color = Utility.TeamToColor(currentTeam);

        // Spawn a collider if you're the owner, to allow selection of the pieces
        if (IsOwner)
            gameObject.AddComponent<BoxCollider>();
    }

    // This is used with the scriptable rendering pipeline to add a select effect
    public void EnableInteraction()
    {
        gameObject.layer = LayerMask.NameToLayer("ActivePiece");
    }
    public void DisableInteraction()
    {
        gameObject.layer = LayerMask.NameToLayer("Piece");
    }

    [ClientRpc]
    public void PositionClientRpc(Vector3 position)
    {
        // If -1, put the piece on its starting position
        if (position == -Vector3.one)
        {
            transform.position = startPosition;
        }
        else
        {
            //StartCoroutine(Move(position));
            t = new Task(Move(position));
            //transform.position = position;
            //isSelected = true;
        }
    }

    public IEnumerator Move(Vector3 position)
    {
        animator.SetBool("canJump", true);
        animator.SetBool("loopJump", true);
        if (isMoving)
        {
            yield break;
        }
        isMoving = true;
        //lookTileInt = 0;
        while (steps > 0)
        {
            routePosition++;

            if (routePosition == 50)
            {
                lookTileInt = 1;
            }
            else
            {
                lookTileInt = 0;
            }

            Vector3 startPos = board[routePosition-1].tileTransform.position;
            Vector3 nextPos = board[routePosition].tileTransform.position;
            Vector3 lookAtTile = board[routePosition + lookTileInt].tileTransform.position;

            while (MoveToNextNode(startPos, nextPos, lookAtTile))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
            timeForPointToPoint = 0;

            steps--;

            if(steps == 0)
            {
                //transform.position = position;
            }
        }

        //transform.position = position; //put more pawns on same tile (reposition)

        if (currentTile > 0)
        {
            isOut = true;
        }
        isMoving = false;

        animator.SetBool("loopJump", false);
        animator.SetBool("canJump", false);
        
    }

    bool MoveToNextNode(Vector3 startPos, Vector3 nextPos, Vector3 lookAtTile)
    {
        timeForPointToPoint += 5f * Time.deltaTime;
        Vector3 pawnPosition = Vector3.Lerp(startPos, nextPos, timeForPointToPoint);

        RotatePawn(lookAtTile);

        pawnPosition.y += 0.5f * Mathf.Sin(Mathf.Clamp01(timeForPointToPoint) * Mathf.PI);

        return nextPos != (transform.position = Vector3.Lerp(transform.position, pawnPosition, timeForPointToPoint));
    }

    public void RotatePawn(Vector3 lookAtTile)
    {
        float directionToFace = Vector3.Dot(transform.up, lookAtTile - transform.position);
        Vector3 point = lookAtTile - (transform.up * directionToFace);
        transform.LookAt(point, transform.up);
    }
}


