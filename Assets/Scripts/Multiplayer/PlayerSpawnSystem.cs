﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class PlayerSpawnSystem : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab = null;

    private static List<Transform> spawnPoints = new List<Transform>();

    private int nextIndex = 0;

    public static void AddSpawnPoint(Transform position)
    {
        spawnPoints.Add(position);
        spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
    }

    public static void RemoveSpawnPoint(Transform position) => spawnPoints.Remove(position);

    public override void OnStartServer()
    {
        MirrorServer.OnServerIsReady += SpawnPlayer;
    }

    [ServerCallback]
    private void OnDestroy()
    {
        MirrorServer.OnServerIsReady -= SpawnPlayer;
    }

    [Server]
    public void SpawnPlayer(NetworkConnection conn)
    {
        Transform spawnPoint = spawnPoints.ElementAtOrDefault(nextIndex);

        if(spawnPoint == null)
        {
            Debug.Log($"Missing spawn point for player {nextIndex}");
            return;
        }

        GameObject playerInstance = Instantiate(playerPrefab, spawnPoints[nextIndex].position, spawnPoints[nextIndex].rotation);
        NetworkServer.Spawn(playerInstance, conn);

        nextIndex++;
    }

}
