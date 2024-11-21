using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TensionDisplay : MonoBehaviour {
    [SerializeField]
    TensionController controller;
    [SerializeField]
    Slider slider;

    float currentTension;

    public void SetTension(float tension) { 
        currentTension = tension;
    }
    private void OnDestroy() {
        controller.SetDisplay(null);
    }
    // Start is called before the first frame update
    void Start() {
        controller.SetDisplay(this);
    }

    // Update is called once per frame
    void Update() {
        if (currentTension != slider.value)
            slider.value = Mathf.Lerp(slider.value, currentTension, 0.25f);
    }
}
