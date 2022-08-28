using Fusion;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class Pluckable : NetworkBehaviour
{
    [Tooltip("Vegetable/fruit spawned when you pluck")]
    public NetworkPrefabRef[] prefabsToSpawn;

    [Tooltip("Only pluckable if stageController says it is ripe")]
    public bool useStageController = true;

    [Tooltip("Dependent on this being in final stage before being pluckable")]
    public StageController stageController;

    [Tooltip("Pickup item produced if true, otherwise it drops on ground")]
    public bool autoPickup = true;

    [Tooltip("Spawns randomly from prefabsToPlant if true. Otherwise spawns ALL items in prefabsToPlant")]
    public bool spawnRandomPrefab = false;

    public Pickupable toItem()
    {
        if ((useStageController && !stageController.ripe) || prefabsToSpawn.Length < 1)
        {
            return null;
        }

        NetworkObject obj;

        if (spawnRandomPrefab)
        {
            var randomIndex = Random.Range(0, prefabsToSpawn.Length - 1);
            obj = Runner.Spawn(prefabsToSpawn[randomIndex], transform.position, transform.rotation);
        }
        else
        {
            obj = Runner.Spawn(prefabsToSpawn[0], transform.position, transform.rotation); // return the first
            spawnRestOfItems(prefabsToSpawn);
        }

        StartCoroutine(KillMe()); // Async otherwise we can't return Pickupable
        PlayAudio.PlayRandomSourceOnGameobject(gameObject);
        return obj.GetComponent<Pickupable>();
    }

    private IEnumerator KillMe()
    {
        yield return null;
        Runner.Despawn(GetComponent<NetworkObject>());
    }

    private void spawnRestOfItems(NetworkPrefabRef[] prefabs)
    {
        for (int i = 1; i < prefabs.Length; i++)
        {
            Runner.Spawn(prefabs[i], transform.position, transform.rotation);
        }
    }
}
