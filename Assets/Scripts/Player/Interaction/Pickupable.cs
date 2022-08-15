using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NetworkTransform))]
[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Pickupable : MonoBehaviour
{
    public bool thrown = false;
    public UnityEvent<Collider> thrownAtAndHit = new UnityEvent<Collider>();
    public void Throw(Transform from)
    {
        thrown = true;
        GetComponent<Rigidbody>().AddForce((from.forward + (from.up /3)) * 1500f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (thrown)
        {
            thrownAtAndHit.Invoke(collision.collider);
            thrown = false;
        }
    }
}
