using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PickupItem))]
[RequireComponent(typeof(InteractionController))]
public class PluckItem : NetworkBehaviour
{


    private GameObject player;
    private PickupItem pickupItem;

    private InteractionController interactionController;

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority && GetInput(out NetworkInputPrototype input))
        {
            if (input.ePressed && !pickupItem.itemHeld) {
                var pluckable = interactionController.closestCollider(player.transform.position, interactionController.pluckables)?.GetComponent<Pluckable>();
                var pickupable = pluckable?.toItem();

                if (pickupable != null)
                {
                    if (pluckable.autoPickup)
                    {
                        pickupItem.add(pickupable);
                    }
                }
            }
        }
    }

    private void Start()
    {
        player = transform.parent.gameObject;
        pickupItem = GetComponent<PickupItem>();
        interactionController = GetComponent<InteractionController>();
    }
}
