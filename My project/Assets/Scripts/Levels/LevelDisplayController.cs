using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDisplayController : MonoBehaviour {
    [SerializeField]
    Image[] bronzeMedals;
    [SerializeField]
    Image[] silverMedals;
    [SerializeField]
    Image[] goldMedals;
    // Start is called before the first frame update
    void Start() {
        ActivateIfCompleted(bronzeMedals, TensionCompletion.BRONZE);
        ActivateIfCompleted(silverMedals, TensionCompletion.SILVER);
        ActivateIfCompleted(goldMedals, TensionCompletion.GOLD);
    }

    private void ActivateIfCompleted(Image[] medals, TensionCompletion completion) {
        int level = 1;
        foreach (var medal in medals) {
            if (LevelManager.Instance.GetLevelCompletion(level) >= completion)
                medal.color = Color.white;
            ++level;
        }
    }
}
