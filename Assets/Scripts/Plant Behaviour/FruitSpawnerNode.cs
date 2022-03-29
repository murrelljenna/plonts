using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game.assets.plants
{
    public enum FruitSpawnerNodeStage
    {
        Empty,
        Unripe,
        Ripe
    }

    public class FruitSpawnerNode : MonoBehaviour
    {
        [System.NonSerialized]
        public FruitSpawnerNodeStage stage;

        public void tick()
        {
            if (stage == FruitSpawnerNodeStage.Empty)
            {
                spawnUnripeFruit();
            }
            else if (stage == FruitSpawnerNodeStage.Unripe)
            {
                ripenFruit();
            }
            else if (stage == FruitSpawnerNodeStage.Ripe)
            {
                Debug.LogWarning("FruitSpawnerNode is being told to tick even though its at the end of the road");
            }
        }

        private void spawnUnripeFruit()
        {

        }

        private void ripenFruit()
        {

        }
    }
}