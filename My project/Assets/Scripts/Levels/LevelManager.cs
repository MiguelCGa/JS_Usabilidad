using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager Instance { get; private set; }

    [SerializeField]
    int totalLevels = 4;

    int lastCompeltedLevel = 0;

    TensionCompletion[] levelsCompletion;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start() {
        // EventoBot("Nivel Actual", lastCompeltedLevel) 
        EventQueue.Instance.AddEvent(new GameEvent(EventType.LevelStart, lastCompeltedLevel));
        levelsCompletion = new TensionCompletion[totalLevels];
        for (int i = 0; i < levelsCompletion.Length; ++i)
            levelsCompletion[i] = TensionCompletion.FAILED;
    }

    public int NextLevel() {
        // EventoBot("Nivel Actual", lastCompeltedLevel + 1) 
        EventQueue.Instance.AddEvent(new GameEvent(EventType.LevelStart, lastCompeltedLevel + 1));
        return lastCompeltedLevel + 1;
    }

    public void CompleteLevel(int currentLevel, TensionCompletion tensionCompletion) {
        if (lastCompeltedLevel < currentLevel)
            lastCompeltedLevel = currentLevel;
        if (levelsCompletion[currentLevel - 1] < tensionCompletion)
            levelsCompletion[currentLevel - 1] = tensionCompletion;
    }

    public TensionCompletion GetLevelCompletion(int level) {
        return levelsCompletion[level - 1];
    }
}
