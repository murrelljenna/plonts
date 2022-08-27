using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitNodeController : NetworkBehaviour
{
    private struct SpawnerLocation
    {
        public SpawnerLocation(Vector3 position, Quaternion rotation, bool taken, NetworkObject obj)
        {
            this.position = position;
            this.rotation = rotation;
            this.taken = taken;
            this.spawner = obj;
        }

        public Vector3 position;
        public Quaternion rotation;
        public bool taken;
        public NetworkObject spawner;
    }

    [Tooltip("Gameobjects representing places where fruit can grow")]
    public GameObject[] nodes;

    private SpawnerLocation[] nodesAvailability;

    [Tooltip("Spawner Gameobject")]
    public NetworkPrefabRef Spawner;

    public override void Spawned()
    {
        if (nodes.Length < 1)
        {
            Debug.LogWarning("Stages array in this plant is empty. You forgot me.");
            return;
        }

        nodesAvailability = new SpawnerLocation[nodes.Length];    

        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].SetActive(false);

            nodesAvailability[i] = new SpawnerLocation(nodes[i].transform.position, nodes[i].transform.rotation, false, null);
        }

        if (Object.HasStateAuthority)
            LightingManager.Get().sunUp.AddListener(activateRandomNode);
    }

    private void activateRandomNode()
    {
        if (!gameObject.activeInHierarchy)
            return;

        for (int i = 0; i < nodesAvailability.Length; i++)
        {
            if (nodesAvailability[i].spawner == null)
            {
                nodesAvailability[i].taken = false;
            }
        }

        var randomIndex = Random.Range(0, nodes.Length - 1);

        if (!nodesAvailability[randomIndex].taken)
        {
            nodesAvailability[randomIndex].taken = true;
            var networkObj = Runner.Spawn(Spawner, nodesAvailability[randomIndex].position, nodesAvailability[randomIndex].rotation);
            nodesAvailability[randomIndex].spawner = networkObj;
            networkObj.transform.parent = transform;
        }
    }
}
