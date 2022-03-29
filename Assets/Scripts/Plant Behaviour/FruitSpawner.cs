using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static game.assets.utility.Helpers;

namespace game.assets.plants {
    public class FruitSpawner : MonoBehaviour
    {
        [Tooltip("Nodes on the plant where fruit can be spawned")]
        public FruitSpawnerNode[] spawnableNodes;

        private FruitSpawnerNode randomNode(FruitSpawnerNodeStage stageWeWant) {
            for (int i = 0; i < spawnableNodes.Length; i++)
            {
                var node = spawnableNodes[i];

                if (node.stage == stageWeWant)
                {
                    return node;
                }
            }

            return null;
        }

        private void tick() {
            randomNode(FruitSpawnerNodeStage.Empty)?.tick();
            randomNode(FruitSpawnerNodeStage.Unripe)?.tick();
        }
    }
}
