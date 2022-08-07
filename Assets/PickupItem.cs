using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PickupItem : MonoBehaviour
{
    /*private Camera camera;

    private void Start()
    {
        camera = GetComponent<Camera>();
        
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
                        pickedUp = hitObject;
                    }
                }
            }
            else
            {
                pickedUp = null;
            }




    }*/
}
