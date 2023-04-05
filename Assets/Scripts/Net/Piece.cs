using UnityEngine;
using Unity.Netcode;
using System.Collections;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine.ProBuilder.Shapes;

public enum AnimationState
{
    Idle,
    Walk,
    Attack,
    Death
}

public class Piece : NetworkBehaviour
{
    [Header("INFO")]
    public string pieceName;
    public string cardId;
    public Spell spell;
    public Team currentTeam;
    public int currentTile = -1;
    public Vector3 startPosition;
    public int eatPower;
    public int lookTileInt = 0;

    public GameObject pieceCardImage;

    [Header("BOARD MOVEMENT")]
    public int routePosition;
    public int steps;
    float timeForPointToPoint = 0f;
    public bool isMoving = false;
    public bool isOut = false;
    
    //public LudoTile[] board;
    public Task t;

    public List<LudoTile> board = new List<LudoTile>();

    public Animator animator;

    public GameObject[] selector;

    public Material[] allPawnColorMaterials;

    public GameObject head;
    public GameObject body;

    public NetworkVariable<AnimationState> networkAnimationState = new NetworkVariable<AnimationState>();
    public NetworkVariable<bool> isSelected = new NetworkVariable<bool>(false,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        animator = GetComponent<Animator>();
        // Assign the start position field to be restored when piece is eaten
        startPosition = transform.position;
        spell = gameObject.GetComponent<Spell>();

        selector[NetworkManager.Singleton.LocalClientId] = Instantiate(selector[NetworkManager.Singleton.LocalClientId]);
        selector[NetworkManager.Singleton.LocalClientId].transform.SetParent(this.transform);
        selector[NetworkManager.Singleton.LocalClientId].transform.position = this.transform.position;
        selector[NetworkManager.Singleton.LocalClientId].SetActive(false);

        int teamId = Utility.RetrieveTeamId(OwnerClientId);
        currentTeam = (Team)teamId;

        //Change material
        head.GetComponent<MeshRenderer>().material = allPawnColorMaterials[Utility.TeamToMaterial(currentTeam)];
        body.GetComponent<Renderer>().material = allPawnColorMaterials[Utility.TeamToMaterial(currentTeam)];

        // Spawn a collider if you're the owner, to allow selection of the pieces
        if (IsOwner)
        {
            gameObject.AddComponent<BoxCollider>();
            gameObject.GetComponent<BoxCollider>().size = new Vector3(1f,2.5f,1f);
            gameObject.GetComponent<BoxCollider>().center = new Vector3(0f,1.25f,0f);
        }    
    }

    private void Update()
    {
        ClientVisuals();
    }

    [ServerRpc(RequireOwnership =false)]
    public void UpdateAnimationStateServerRpc(AnimationState newAnimState)
    {
        networkAnimationState.Value = newAnimState;
    }

    private void ClientVisuals()
    {
        switch (networkAnimationState.Value)
        {
            case AnimationState.Idle:
                animator.SetBool("isWalking", false);
                animator.SetBool("isDead", false);
                animator.ResetTrigger("attack");
                break;
            case AnimationState.Walk:
                animator.SetBool("isWalking", true);
                break;
            case AnimationState.Attack:
                //animator.SetBool("isAttacking", true);
                animator.SetTrigger("attack");
                break;
            case AnimationState.Death:
                animator.SetBool("isDead", true);
                break;
            default:
                break;
        }
    }

    // This is used with the scriptable rendering pipeline to add a select effect
    public void EnableInteraction()
    {
        gameObject.layer = LayerMask.NameToLayer("ActivePiece");
        selector[NetworkManager.Singleton.LocalClientId].SetActive(true);
    }
    public void DisableInteraction()
    {
        gameObject.layer = LayerMask.NameToLayer("Piece");
        selector[NetworkManager.Singleton.LocalClientId].SetActive(false);
    }

    [ClientRpc]
    public void PositionClientRpc(Vector3 position)
    {
        // If -1, put the piece on its starting position
        if (position == -Vector3.one)
        {
            transform.position = startPosition;
            UpdateAnimationStateServerRpc(AnimationState.Idle);
        }
        else
        {
            //StartCoroutine(Move(position));
            t = new Task(Move(position));

            t.Finished += delegate (bool manual)
            {
                if (!manual) {
                    //animator.ResetTrigger("jump");
                    //transform.position = position;
                }
                    
            };
            //transform.position = position;
            //isSelected = true;
        }
    }

    public IEnumerator Move(Vector3 position)
    {
        if (isMoving)
        {
            yield break;
        }
        isMoving = true;
        //lookTileInt = 0;

        if(steps > 0)
        {
            UpdateAnimationStateServerRpc(AnimationState.Walk);
        }

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

            if (steps == 0)
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


