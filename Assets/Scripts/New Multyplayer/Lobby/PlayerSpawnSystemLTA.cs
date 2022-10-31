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

    PlayerEntityLTA.PlayerColors lastColor;

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

        PlayerEntityLTA player = playerInstance.GetComponent<PlayerEntityLTA>();
        player.playerName = conn.identity.GetComponent<NetworkGamePlayerLTA>().GetDisplayName();
        player.playerColors = GetRandomColor();
        player.playerTypes = PlayerEntityLTA.PlayerTypes.HUMAN;
        //player.rollButtonState = false;

        GameManager2.instance.playerDictionary.Add(conn.connectionId, playerInstance.GetComponent<PlayerEntityLTA>());
        GameManager2.instance.playerList.Add(player);

        NetworkServer.Spawn(playerInstance, conn);


        nextIndex++;
    }

    PlayerEntityLTA.PlayerColors GetRandomColor() //Return random color only if color isn't already used by another player
    {
        PlayerEntityLTA.PlayerColors rand = (PlayerEntityLTA.PlayerColors)Random.Range(0, 3);
        while (rand == lastColor)
            rand = (PlayerEntityLTA.PlayerColors)Random.Range(0, 3);
        lastColor = rand;
        return rand;
    }



}
