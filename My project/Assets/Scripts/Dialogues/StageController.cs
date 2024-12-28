using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour {

    [SerializeField]
    string id;
    [SerializeField]
    bool initialStage = false;

    public string GetID() {
        return id;
    }
    public bool IsInitialStage() { 
        return initialStage; 
    }

    // Start is called before the first frame update
    void Start() {
        ConversationManager.Instance.stageManager.AddStage(this);
    }
}
