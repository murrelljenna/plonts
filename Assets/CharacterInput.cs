using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

[RequireComponent(typeof(NetworkCharacterControllerPrototype))]
public class CharacterInput : MonoBehaviour
{
    [SerializeField] private MouseLook m_MouseLook;
    private NetworkCharacterControllerPrototype character;
    private Camera m_Camera;

    void Start()
    {
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
        Debug.Log(transform.rotation);
    }
    
    void FixedUpdate()
    {
        Vector3 position = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            character.Jump();
        }

        if (Input.GetKey(KeyCode.W))
            position = Vector3.forward;
        if (Input.GetKey(KeyCode.D))
            position = Vector3.right;
        if (Input.GetKey(KeyCode.A))
            position = Vector3.left;
        if (Input.GetKey(KeyCode.S))
            position = Vector3.back;

        character.Move(position);
    }
}
