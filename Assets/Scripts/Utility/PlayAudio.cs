using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayAudio
{
    public static void PlayRandomSourceOnGameobject(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }

        var audiosources = obj.GetComponents<AudioSource>();
        var randomIndex = Random.Range(0, audiosources.Length - 1);
        if (audiosources != null && audiosources.Length > 0)
        {
            AudioSource.PlayClipAtPoint(audiosources[randomIndex].clip, obj.transform.position);
        }
    }
}
