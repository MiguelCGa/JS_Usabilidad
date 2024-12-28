using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour {
    private Dictionary<string, StageController> stages = new Dictionary<string, StageController>();
    private StageController currentStage = null;
    public void Restart() {
        stages.Clear();
    }

    public bool SetStage(string nextStage) {
        if (!stages.ContainsKey(nextStage))
            return false;
        if (currentStage != null) 
            currentStage.gameObject.SetActive(false);
        currentStage = stages[nextStage];
        currentStage.gameObject.SetActive(true);
        return true;
    }
    public void AddStage(StageController stage) {
        if (stages.ContainsKey(stage.GetID()))
            return;
        stages[stage.GetID()] = stage;
        if (stage.IsInitialStage())
            currentStage = stage;
        else
            stage.gameObject.SetActive(false);
    }
}
