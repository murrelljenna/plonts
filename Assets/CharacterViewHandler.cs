using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterViewHandler : NetworkBehaviour
{
    private const float ySensitivity = 0f;
    private Camera maybeLocalCamera;

    private void Awake()
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
            float mouseInputY = Input.GetAxis("Mouse Y") * ySensitivity;

            Quaternion cameraYRotation = Quaternion.Euler(-mouseInputY, 0f, 0f);

            maybeLocalCamera.transform.localRotation = cameraYRotation;
        }
    }
}
