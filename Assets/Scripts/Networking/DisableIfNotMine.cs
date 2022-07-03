using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableIfNotMine : NetworkBehaviour
{
    [Tooltip("Disables this monobehaviour")]
    public GameObject toDisable;

    [Tooltip("Checks this NetworkObject for ownership")]
    public NetworkObject toCheck;

    public void Spawned()
    {
        if (!toCheck.HasInputAuthority)// || !Object.HasStateAuthority)
            toDisable.SetActive(false);
    }
}
