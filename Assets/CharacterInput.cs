using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkCharacterControllerPrototype))]
public class CharacterInput : MonoBehaviour
{
    private NetworkCharacterControllerPrototype character;

    void Start()
    {
        character = GetComponent<NetworkCharacterControllerPrototype>();
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
