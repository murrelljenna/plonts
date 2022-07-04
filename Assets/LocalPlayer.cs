using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LocalPlayer
{
    private static CharacterViewHandler cache;

    public static CharacterViewHandler getView()
    {
        if (cache != null)
            return cache;

        List<GameObject> rootObjectsInScene = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(rootObjectsInScene);

        for (int i = 0; i < rootObjectsInScene.Count; i++)
        {
            CharacterViewHandler[] allComponents = rootObjectsInScene[i].GetComponentsInChildren<CharacterViewHandler>(true);
            for (int j = 0; j < allComponents.Length; j++)
            {
                if (allComponents[j].isLocal)
                {
                    cache = allComponents[j];
                    return allComponents[j];
                }
            }
        }

        return null;
    }
}
