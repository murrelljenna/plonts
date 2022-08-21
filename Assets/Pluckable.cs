using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class Pluckable : NetworkBehaviour
{
    [Tooltip("Vegetable/fruit spawned when you pluck")]
    public NetworkPrefabRef prefabToPlant;

    public Pickupable toItem()
    {
        var obj = Runner.Spawn(prefabToPlant, transform.position, transform.rotation);
        StartCoroutine(KillMe()); // Async otherwise we can't return Pickupable
        return obj.GetComponent<Pickupable>();
    }

    private IEnumerator KillMe()
    {
        yield return null;
        Runner.Despawn(GetComponent<NetworkObject>());
    }
}
