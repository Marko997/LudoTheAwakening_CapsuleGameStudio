using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class PlayerSpawnSystemLTA : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab = null;

    private static List<Transform> spawnPoints = new List<Transform>();

    private int nextIndex = 0;

    PlayerEntityLTA.PlayerColors lastNumber;

    public static void AddSpawnPoint(Transform pointLocation)
    {
        spawnPoints.Add(pointLocation);

        spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
    }

    public static void RemoveSpawnPoint(Transform pointLoaction) => spawnPoints.Remove(pointLoaction);

    public override void OnStartServer() => NetworkManagerLTA.OnServerReadied += SpawnPlayer;

    [ServerCallback]
    private void OnDestroy() => NetworkManagerLTA.OnServerReadied -= SpawnPlayer;

    [Server]
    public void SpawnPlayer(NetworkConnection conn)
    {
        Transform spawnPoint = spawnPoints.ElementAtOrDefault(nextIndex);

        if(spawnPoint == null)
        {
            Debug.LogError($"Spawn point not found for player {nextIndex}");
            return;
        }

        GameObject playerInstance = Instantiate(playerPrefab, spawnPoints[nextIndex].position, spawnPoints[nextIndex].rotation);
        playerInstance.GetComponent<PlayerEntityLTA>().playerName = conn.identity.GetComponent<NetworkGamePlayerLTA>().GetDisplayName();
        playerInstance.GetComponent<PlayerEntityLTA>().playerColors = GetRandomColor();


        NetworkServer.Spawn(playerInstance, conn);

        NetworkGameManagerLTA.instance.playerList.Add(playerInstance.GetComponent<PlayerEntityLTA>());


        nextIndex++;
    }

    PlayerEntityLTA.PlayerColors GetRandomColor() //Return random color only if color isn't already used by another player
    {
        PlayerEntityLTA.PlayerColors rand = (PlayerEntityLTA.PlayerColors)Random.Range(0, 3);
        while (rand == lastNumber)
            rand = (PlayerEntityLTA.PlayerColors)Random.Range(0, 3);
        lastNumber = rand;
        return rand;
    }



}
