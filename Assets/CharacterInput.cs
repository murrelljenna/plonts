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
    }

    private void RotateView()
    {
        m_MouseLook.LookRotation(transform, m_Camera.transform);
    }

    private void Update()
    {
        RotateView();
    }
    
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            character.Jump();
        }

        if (Input.GetKey(KeyCode.W))
            character.Move(Vector3.forward);
        if (Input.GetKey(KeyCode.D))
            character.Move(Vector3.right);
        if (Input.GetKey(KeyCode.A))
            character.Move(Vector3.left);
        if (Input.GetKey(KeyCode.S))
            character.Move(Vector3.back);
    }
}
