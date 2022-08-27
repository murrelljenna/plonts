using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Pickupable))]
[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(NetworkTransform))]
public class Seed : NetworkBehaviour
{
    [Tooltip("Prefab to spawn - dirt pile + seed?")]
    public NetworkPrefabRef prefabToPlant;
    public override void Spawned()
    {
        if (Object.HasStateAuthority)
            GetComponent<Pickupable>().thrownAtAndHit.AddListener(PlantWhenThrown);
    }

    private void PlantWhenThrown(Collision col)
    {
        if (col.collider.GetComponent<CanPlantHere>())
        {
            Plant(col.GetContact(0).point);
        }
    }

    private void Plant(Vector3 location)
    {
        Runner.Spawn(prefabToPlant, location, Quaternion.identity);
        Runner.Despawn(GetComponent<NetworkObject>());
    }
}