using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointLTA : MonoBehaviour
{
    private void Awake() => PlayerSpawnSystemLTA.AddSpawnPoint(transform);

    private void OnDestroy() => PlayerSpawnSystemLTA.RemoveSpawnPoint(transform);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 1f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
    }

}
