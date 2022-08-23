using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class ToSeed : NetworkBehaviour
{
    [Tooltip("Seed that this fruit will spoil into")]
    public NetworkPrefabRef seed;

    [Tooltip("Days until this turns to seed")]
    public short daysTillSeed = 1;

    [Tooltip("Number of seeds produced")]
    public short seedCount = 3;
    private short dayCount = 0;

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
            LightingManager.Get().sunUp.AddListener(maybeGoToSeed);
    }

    private void maybeGoToSeed()
    {
        if (dayCount == daysTillSeed)
        {
            for (int i = 0; i <= seedCount; i++)
            {
                Runner.Spawn(seed, transform.position, transform.rotation);
            }

            Runner.Despawn(GetComponent<NetworkObject>());
        }
        else
        {
            dayCount++;
        }
    }
}
