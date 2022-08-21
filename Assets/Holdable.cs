using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NetworkTransform))]
[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Pickupable))]
[RequireComponent(typeof(Serializable))]
public class Holdable : NetworkBehaviour
{
    // Start is called before the first frame update
    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            GetComponent<Pickupable>().thrownAtAndHit.AddListener(onHit);
        }
    }

    private void onHit(Collision collision)
    {
        collision.collider.gameObject.GetComponentInChildren<PickupItem>()?.add(GetComponent<Pickupable>());
    }
}