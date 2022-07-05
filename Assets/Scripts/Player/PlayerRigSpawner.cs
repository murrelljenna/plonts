using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRigSpawner : SimulationBehaviour, ISpawned
{
    public GameObject playerRigPrefab;
    public void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Instantiate(playerRigPrefab, transform);
        }
    }
}
