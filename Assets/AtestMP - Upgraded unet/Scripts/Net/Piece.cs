using UnityEngine;
using Mirror;

public class Piece : NetworkBehaviour
{
    public Team currentTeam;
    public int currentTile = -1;
    public Vector3 startPosition;

    public override void OnStartServer() // nisam siguran
    {
        startPosition = transform.position;

        int teamId = Utility.RetrieveTeamId(NetworkClient.connection.connectionId);//nisam sig da radi
        currentTeam = (Team)teamId;

        GetComponent<MeshRenderer>().material.color = Utility.TeamToColor(currentTeam);

        if (hasAuthority)
        {
            gameObject.AddComponent<BoxCollider>();
        }

    }

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
        if(position == -Vector3.one)
        {
            transform.position = startPosition;
        }
        else
        {
            transform.position = position;
        }
    }

}
