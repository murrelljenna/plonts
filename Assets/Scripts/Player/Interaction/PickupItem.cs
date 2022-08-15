using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : NetworkBehaviour
{
    private List<Collider> lookingAt;
    private GameObject player;

    public Pickupable pickedUp;
    private Transform target;

    private bool cooledOff = true;

    private void Start()
    {
        player = transform.parent.gameObject;
        target = transform.Find("PickupLocation");
        lookingAt = new List<Collider>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputPrototype input))
        {
            if (input.ePressed)
            {
                if (cooledOff)
                {
                    if (pickedUp == null)
                        pickedUp = closestCollider(lookingAt)?.GetComponent<Pickupable>();
                    else
                        pickedUp = null;

                    StartCoroutine(nameof(pickupCooloff));
                }
            }

            if (input.IsDown(NetworkInputPrototype.BUTTON_FIRE))
            {
                if (pickedUp != null)
                {
                    pickedUp.Throw(player.transform);
                    pickedUp = null;
                }
            }
        }

        if (pickedUp != null)
        {
            pickedUp.transform.position = target.position;
            pickedUp.transform.rotation = target.rotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Pickupable>())
            lookingAt.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Pickupable>())
            lookingAt.Remove(other);
    }

    private Collider closestCollider(List<Collider> colliders) {
        Collider closest = null;
        float closestDistance = float.MaxValue;

        colliders.ForEach(collider =>
        {
            float dist = Vector3.Distance(player.transform.position, collider.transform.position);
            if (dist < closestDistance)
            {
                closest = collider;
                closestDistance = dist;
            }
        });
        return closest;
    }

    private IEnumerator pickupCooloff()
    {
        cooledOff = false;
        yield return new WaitForSeconds(0.2f);
        cooledOff = true;
    }
}
