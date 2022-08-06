using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PickupItem : MonoBehaviour
{
    private Camera camera;

    private GameObject pickedUp;
    private Transform target;

    private void Start()
    {
        camera = GetComponent<Camera>();
        target = transform.Find("PickupLocation");
    }

    private void Update()
    {
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;

        if (Input.GetKeyDown(KeyCode.E))
            if (pickedUp == null)
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    var hitObject = hit.collider.gameObject.GetComponent<Pickupable>();
                    if (hitObject != null)
                    {
                        pickedUp = hitObject.gameObject;
                    }
                }
            }
            else
            {
                pickedUp = null;
            }

        if (Input.GetMouseButtonDown(0))
        {
            if (pickedUp != null)
            {
                pickedUp.GetComponent<Rigidbody>().AddForce(transform.forward * 1500f);
                pickedUp = null;
            }
        }

        if (pickedUp != null)
        {
            pickedUp.transform.position = target.position;
            pickedUp.transform.rotation = target.rotation;
        }
    }
}
