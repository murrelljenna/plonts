using Fusion;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(InteractionController))]
[RequireComponent(typeof(Serializable))]
public class PickupItem : NetworkBehaviour
{
    private GameObject player;

    private Pickupable pickedUp;
    private Transform target;
    private Storage storage;
    private float startTime;

    public bool itemHeld = false;

    private bool cooledOff = true;
    const float speed = 1.0F;
    private InteractionController interactionController;

    private void Start()
    {
        player = transform.parent.gameObject;
        target = transform.Find("HandsLocation");
        storage = new Storage();
        itemHeld = false;
        interactionController = GetComponent<InteractionController>();
    }

public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputPrototype input))
        {
            if (input.ePressed && cooledOff)
            {
                if (!itemHeld)
                {
                    add(interactionController.closestCollider(player.transform.position, interactionController.pickupables)?.GetComponent<Pickupable>());
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

            pickedUp.transform.rotation = target.rotation;
        }
    }

    public void add(Pickupable obj)
    {
        if (obj && !storage.hasItem())
        {
            startTime = Time.time;
            obj.GetComponent<Rigidbody>().useGravity = false;
            storage.add(obj.GetComponent<Serializable>().description);
            pickedUp = obj;
            itemHeld = true;
        }
    }

    public void drop()
    {
        pickedUp.GetComponent<Rigidbody>().useGravity = true;
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

    private IEnumerator pickupCooloff()
    {
        cooledOff = false;
        yield return new WaitForSeconds(0.2f);
        cooledOff = true;
    }
}
