using Fusion;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class Pluckable : NetworkBehaviour
{
    [Tooltip("Vegetable/fruit spawned when you pluck")]
    public NetworkPrefabRef prefabToPlant;

    [Tooltip("Dependent on this being in final stage before being pluckable")]
    public StageController stageController;

    public Pickupable toItem()
    {
        if (!stageController.ripe)
        {
            return null;
        }
        var obj = Runner.Spawn(prefabToPlant, transform.position, transform.rotation);
        StartCoroutine(KillMe()); // Async otherwise we can't return Pickupable
        PlayAudio.PlayRandomSourceOnGameobject(gameObject);
        return obj.GetComponent<Pickupable>();
    }

    private IEnumerator KillMe()
    {
        yield return null;
        Runner.Despawn(GetComponent<NetworkObject>());
    }
}
