using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [Tooltip("Gameobjects representing unique phases of plant")]
    public GameObject[] stages;
    private int stageIndex = 0;

    void Update()
    {
        //Detect when the E arrow key is pressed down
        if (Input.GetKeyDown(KeyCode.E))
            nextStage();
    }

    private void Start()
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
    }

    private void nextStage()
    {
        if (stageIndex < (stages.Length - 1))
        {
            stages[stageIndex].SetActive(false);
            stageIndex++;
            stages[stageIndex].SetActive(true);
        }
    }
}
