using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        thrownAtAndHit.AddListener(testing);
    }

    private void testing(Collider other)
    {
        Debug.Log(other.gameObject.name);
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
