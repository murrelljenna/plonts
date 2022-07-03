using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

[RequireComponent(typeof(CharacterController))]
public class CharacterInput : NetworkBehaviour
{
    [SerializeField] private MouseLook m_MouseLook;
    private NetworkCharacterControllerPrototype character;
    private Camera m_Camera;

    void Start()
    {
        if (!Object.HasInputAuthority)
            return;

        character = GetComponent<NetworkCharacterControllerPrototype>();
        m_Camera = Camera.main;
        m_MouseLook.Init(transform, m_Camera.transform);
        m_MouseLook.clampVerticalRotation = false;
    }

    private void RotateView()
    {
        m_MouseLook.LookRotation(transform, m_Camera.transform);
    }

    private void Update()
    {
        RotateView();
    }
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            Debug.Log(gameObject.name + ": " + data.BUTTON_FORWARD);
            //Debug.Log("GOt this NetworkInputData: " + data);
            Vector3 direction = Vector3.zero;
            if (data.BUTTON_JUMP)
                character.Jump();

            if (data.BUTTON_FORWARD)
                direction += transform.forward;
            if (data.BUTTON_BACKWARD)
                direction += -transform.forward;
            if (data.BUTTON_LEFT_STRAFE)
                direction += -transform.right;
            if (data.BUTTON_RIGHT_STRAFE)
                direction += transform.right;

            character.Move(direction);
        }
    }

    public void Initialize()
    {
        
    }
}
