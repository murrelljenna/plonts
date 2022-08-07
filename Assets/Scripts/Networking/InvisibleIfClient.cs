using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleIfClient : SimulationBehaviour, ISpawned
{
    public void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            var transforms = gameObject.GetComponentsInChildren<Transform>();
            foreach (Transform transform in transforms)
                transform.gameObject.layer = LayerMask.NameToLayer("Player-Camera-Ignore");
        }
    }
}
