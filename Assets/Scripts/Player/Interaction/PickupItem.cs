using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : NetworkBehaviour
{
    private List<Collider> lookingAt;
    private GameObject player;

    private Pickupable pickedUp;
    private Transform target;
    private Storage storage;
    private float startTime;

    public bool itemHeld;

    private bool cooledOff = true;
    const float speed = 1.0F;

    private void Start()
    {
        player = transform.parent.gameObject;
        target = transform.Find("HandsLocation");
        lookingAt = new List<Collider>();
        storage = new Storage();
        itemHeld = false;
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputPrototype input))
        {
            if (input.ePressed && cooledOff)
            {
                if (!itemHeld)
                {
                    add(closestCollider(lookingAt)?.GetComponent<Pickupable>());
                    startTime = Time.time;
                }
                else
                {
                    drop();
                }

                StartCoroutine(nameof(pickupCooloff));
            }

            if (itemHeld && input.IsDown(NetworkInputPrototype.BUTTON_FIRE))
            {
                pickedUp.Throw(player.transform);
                drop();
            }

            lerpItemHeld();
        }
    }

    private void lerpItemHeld()
    {
        if (itemHeld)
        {
            float journeyLength = Vector3.Distance(pickedUp.transform.position, target.position);

            // Distance moved equals elapsed time times speed..
            float distCovered = (Time.time - startTime) * speed;

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / journeyLength;

            // Set our position as a fraction of the distance between the markers.
            pickedUp.transform.position = Vector3.Lerp(pickedUp.transform.position, target.position, fractionOfJourney);

            Debug.Log(pickedUp.transform.position);

            pickedUp.transform.rotation = target.rotation;
        }
    }

    public void add(Pickupable obj)
    {
        if (obj && !storage.hasItem())
        {
            storage.add(obj.GetComponent<Serializable>().description);
            pickedUp = obj;
            itemHeld = true;
        }
    }

    public void drop()
    {
        if (storage.hasItem())
        {
            storage.clear();
        }
        if (pickedUp != null)
        {
            pickedUp = null;
        }
        itemHeld = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Pickupable>())
        {
            lookingAt.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Pickupable>())
        {
            lookingAt.Remove(other);
        }
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
