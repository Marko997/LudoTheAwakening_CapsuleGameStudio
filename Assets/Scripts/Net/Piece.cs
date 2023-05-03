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
    Death,
    Jump
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
    public float _JumpSize = 10f;

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

    public Material[] allPawnBodyColorMaterials;
    public Material[] allPawnHeadColorMaterials;

    public GameObject head;
    public GameObject body;

    public NetworkVariable<AnimationState> networkAnimationState = new NetworkVariable<AnimationState>();
    //public NetworkVariable<bool> isSelected = new NetworkVariable<bool>(false,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isSelected = new NetworkVariable<bool>();

    public bool isSelectedLocal = false;

    public AudioSource source;
    public AudioClip jumpSound;
    public AudioClip endSound;

    public GameObject deathEffect;

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
        head.GetComponent<SkinnedMeshRenderer>().material = allPawnHeadColorMaterials[Utility.TeamToMaterial(currentTeam)];
        body.GetComponent<SkinnedMeshRenderer>().material = allPawnBodyColorMaterials[Utility.TeamToMaterial(currentTeam)];

        // Spawn a collider if you're the owner, to allow selection of the pieces
        if (IsOwner)
        {
            gameObject.AddComponent<BoxCollider>();
            gameObject.GetComponent<BoxCollider>().size = new Vector3(1f,2.5f,1f);
            gameObject.GetComponent<BoxCollider>().center = new Vector3(0f,1.25f,0f);
        }
        //source = FindObjectOfType<AudioSource>();
        //AsignAudioClientRpc();
        
    }
    [ClientRpc]
    private void AsignAudioClientRpc()
    {
        source = FindObjectOfType<AudioSource>();
    }

    [ClientRpc]
    public void ActivateDeathEffectClientRpc(ulong clientId)
    {
        if(clientId == NetworkManager.Singleton.LocalClientId)
        {
            deathEffect.SetActive(true);
            deathEffect.GetComponent<ParticleSystem>().Play();
            StopDeathEffect();
        }
    }
    private IEnumerator StopDeathEffect()
    {
        yield return new WaitForSeconds(2);
        deathEffect.SetActive(false);
    }

    private void Start()
    {
        if (GetComponentInParent<PlayerController>() == null)
        {
            return;
        }
        if(GetComponentInParent<PlayerController>().PlayerType == PlayerTypes.CPU)
        {
            animator = GetComponent<Animator>();
            // Assign the start position field to be restored when piece is eaten
            startPosition = transform.position;
            spell = gameObject.GetComponent<Spell>();

            selector[1] = Instantiate(selector[1]);
            selector[1].transform.SetParent(this.transform);
            selector[1].transform.position = this.transform.position;
            selector[1].SetActive(false);

            int teamId = Utility.RetrieveTeamId(1);
            currentTeam = (Team)teamId;

            //Change material
            //head.GetComponent<MeshRenderer>().material = allPawnColorMaterials[Utility.TeamToMaterial(currentTeam)];
            //body.GetComponent<SkinnedMeshRenderer>().material = allPawnColorMaterials[Utility.TeamToMaterial(currentTeam)];//Change material
            //head.GetComponent<SkinnedMeshRenderer>().material.color = Utility.TeamToColor(currentTeam);
            //body.GetComponent<Renderer>().material.color = Utility.TeamToColor(currentTeam);

            // Spawn a collider if you're the owner, to allow selection of the pieces
            if (IsOwner)
            {
                gameObject.AddComponent<BoxCollider>();
                gameObject.GetComponent<BoxCollider>().size = new Vector3(1f, 2.5f, 1f);
                gameObject.GetComponent<BoxCollider>().center = new Vector3(0f, 1.25f, 0f);
            }
            source = FindObjectOfType<AudioSource>();

        }
    }

    private void Update()
    {
        ClientVisuals();
        //UpdateIsSelected();
        
    }
    private void UpdateIsSelected()
    {
        if(isSelected.Value == true) {
            isSelectedLocal = true;
        }
        else {
            isSelectedLocal = false;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateIsSelectedStateServerRpc(bool newValue)
    {
        isSelected.Value = newValue;
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
                animator.ResetTrigger("jump");
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
            case AnimationState.Jump:
                animator.SetTrigger("jump");
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
        //Debug.Log(name + " " + position);
        // If -1, put the piece on its starting position
        if (position == -Vector3.one)
        {
            transform.position = startPosition;
            StartCoroutine(WaitForDeathToFinish());
        }
        else
        {
            source.clip = jumpSound;
            t = new Task(Move(position));
        }
    }
    public IEnumerator WaitForDeathToFinish()
    {
        yield return new WaitForSeconds(2f);
        UpdateAnimationStateServerRpc(AnimationState.Idle);
    }

    public IEnumerator Move(Vector3 position)
    {
        if (isMoving)
        {
            yield break;
        }
        isMoving = true;

        if(steps > 0)
        {
            UpdateAnimationStateServerRpc(AnimationState.Walk);
        }

        while (steps > 0)
        {
            source.Play();
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

            float jumpSize = 0.5f;
            if (board[56].tileTransform.position == nextPos)
            {
                UpdateAnimationStateServerRpc(AnimationState.Jump);
                jumpSize = _JumpSize;
            }

            while (MoveToNextNode(startPos, nextPos, lookAtTile, jumpSize))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
            timeForPointToPoint = 0;

            steps--;
            UpdateStepsValueClientRpc(steps);
            if (steps == 0)
            {
                //Debug.Log(steps);
                //transform.position = position;
            }
        }

        //transform.position = position; //put more pawns on same tile (reposition)

        if (steps == 0)
        {
            transform.position = position;
        }

        if (currentTile > 0)
        {
            isOut = true;
        }
        isMoving = false;     
    }
    [ClientRpc]
    public void UpdateStepsValueClientRpc(int stepsNewValue)
    {
        steps = stepsNewValue;
    }

    bool MoveToNextNode(Vector3 startPos, Vector3 nextPos, Vector3 lookAtTile, float jumpSize = 0.5f)
    {
        timeForPointToPoint += 5f * Time.deltaTime;
        Vector3 pawnPosition = Vector3.Lerp(startPos, nextPos, timeForPointToPoint);

        RotatePawn(lookAtTile);

        pawnPosition.y += jumpSize * Mathf.Sin(Mathf.Clamp01(timeForPointToPoint) * Mathf.PI);

        return nextPos != (transform.position = Vector3.Lerp(transform.position, pawnPosition, timeForPointToPoint));
    }

    public void RotatePawn(Vector3 lookAtTile)
    {
        float directionToFace = Vector3.Dot(transform.up, lookAtTile - transform.position);
        Vector3 point = lookAtTile - (transform.up * directionToFace);
        transform.LookAt(point, transform.up);
    }

}


