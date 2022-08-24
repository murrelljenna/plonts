using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitNodeController : NetworkBehaviour
{
    [Tooltip("Gameobjects representing places where fruit can grow")]
    public GameObject[] nodes;

    public override void Spawned()
    {
        if (nodes.Length < 1)
        {
            Debug.LogWarning("Stages array in this plant is empty. You forgot me.");
            return;
        }

        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].SetActive(false);
        }

        if (Object.HasStateAuthority)
            LightingManager.Get().sunUp.AddListener(activateRandomNode);
    }

    private void activateRandomNode()
    {
        if (!gameObject.activeInHierarchy)
            return;
        var node = nodes[Random.Range(0, nodes.Length - 1)];

        node.SetActive(true);
    }
}
