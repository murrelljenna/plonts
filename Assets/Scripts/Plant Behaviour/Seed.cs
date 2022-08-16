using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Pickupable))]
public class Seed : NetworkBehaviour
{
    [Tooltip("Prefab to spawn - dirt pile + seed?")]
    public NetworkPrefabRef prefabToPlant;
    public override void Spawned()
    {
        Debug.Log(name);
        if (Object.HasStateAuthority)
            GetComponent<Pickupable>().thrownAtAndHit.AddListener(PlantWhenThrown);

    }

    private void PlantWhenThrown(Collision col)
    {
        if (col.collider.GetComponent<CanPlantHere>())
        {
            Debug.Log("Hey there");
            Plant(col.GetContact(0).point);
        }
    }

    private void Plant(Vector3 location)
    {
        Debug.Log(location);
        Runner.Spawn(prefabToPlant, location, Quaternion.identity);
        Destroy(gameObject);
    }
}