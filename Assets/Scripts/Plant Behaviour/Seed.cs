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

    private void PlantWhenThrown(Collider col)
    {
        if (col.GetComponent<CanPlantHere>())
        {
            Debug.Log("Hey there");
            Plant();
        }
    }

    private void Plant()
    {
        Runner.Spawn(prefabToPlant, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
