using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : NetworkBehaviour
{
    [Tooltip("Gameobjects representing unique phases of plant")]
    public GameObject[] stages;

    [Networked(OnChanged = nameof(enableNewPlantState))]
    private int stageIndex { get; set; } = 0;

    public override void Spawned()
    {
        if (stages.Length < 1)
        {
            Debug.LogWarning("Stages array in this plant is empty. You forgot me.");
            return;
        }

        for (int i = 0; i < stages.Length; i++)
        {
            stages[i].SetActive(false);
        }

        stages[0].SetActive(true);

        if (Object.HasStateAuthority)
            LightingManager.Get().sunUp.AddListener(nextStage);
    }

    private void nextStage()
    {
        if (stageIndex < (stages.Length - 1))
        {
            stageIndex++;
        }
    }

    public static void enableNewPlantState(Changed<StageController> state)
    {
        var stageController = state.Behaviour;
        stageController.stages[stageController.stageIndex - 1].SetActive(false);
        stageController.stages[stageController.stageIndex].SetActive(true);
    }
}
