using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDisplayController : MonoBehaviour {
    [SerializeField]
    GameObject[] bronzeMedals;
    [SerializeField]
    GameObject[] silverMedals;
    [SerializeField]
    GameObject[] goldMedals;
    // Start is called before the first frame update
    void Start() {
        ActivateIfCompleted(bronzeMedals, TensionCompletion.BRONZE);
        ActivateIfCompleted(silverMedals, TensionCompletion.SILVER);
        ActivateIfCompleted(goldMedals, TensionCompletion.GOLD);
    }

    private void ActivateIfCompleted(GameObject[] medals, TensionCompletion completion) {
        int level = 1;
        foreach (var medal in medals) {
            if (LevelManager.Instance.GetLevelCompletion(level) >= completion)
                medal.SetActive(true);
            ++level;
        }
    }
}
