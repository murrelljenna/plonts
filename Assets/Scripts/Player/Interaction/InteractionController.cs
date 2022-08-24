using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public List<Collider> pluckables = new List<Collider>();
    public List<Collider> pickupables = new List<Collider>();
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Looking at the thingie");
        if (other.GetComponent<Pluckable>())
        {
            Debug.Log("Thingie is pluckable");
            pluckables.Add(other);
        }

        if (other.GetComponent<Pickupable>())
        {
            pickupables.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Pluckable>())
        {
            pluckables.Remove(other);
        }

        if (other.GetComponent<Pickupable>())
        {
            pickupables.Remove(other);
        }
    }

    public Collider closestCollider(Vector3 to, List<Collider> colliders)
    {
        Collider closest = null;
        float closestDistance = float.MaxValue;

        colliders.ForEach(collider =>
        {
            if (collider == null)
            {
                return;
            }
            float dist = Vector3.Distance(to, collider.transform.position);
            if (dist < closestDistance)
            {
                closest = collider;
                closestDistance = dist;
            }
        });
        return closest;
    }
}
