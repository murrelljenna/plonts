using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterViewHandler : NetworkBehaviour
{
    private const float ySensitivity = 4f;
    private Camera maybeLocalCamera;
    private float cameraRotation = 0f;

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            maybeLocalCamera = GetComponentInChildren<Camera>();
        }
    }

    private void Update()
    {
        if (Object.HasInputAuthority && maybeLocalCamera != null)
        {

            cameraRotation += Input.GetAxis("Mouse Y") * ySensitivity;
            cameraRotation = Mathf.Clamp(cameraRotation, -90, 90);
            Debug.Log(cameraRotation);
            Quaternion cameraYRotation = Quaternion.Euler(-cameraRotation, 0f, 0f);

            maybeLocalCamera.transform.localRotation = cameraYRotation;
        }
    }
}
