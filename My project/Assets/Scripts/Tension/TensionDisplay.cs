using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TensionDisplay : MonoBehaviour {
    [SerializeField]
    TensionController controller;
    [SerializeField]
    Image slider;
    [SerializeField]
    float slidingFactor = 0.1f;

    float currentTension = 0;

    public void SetTension(float tension) { 
        currentTension = tension;
    }
    private void OnDestroy() {
        controller.SetDisplay(null);
    }
    // Start is called before the first frame update
    void Start() {
        currentTension = controller.SetDisplay(this);
        slider.fillAmount = currentTension;
    }

    // Update is called once per frame
    void Update() {
        if (currentTension != slider.fillAmount)
            slider.fillAmount = Mathf.Lerp(slider.fillAmount, currentTension, slidingFactor);
    }
}
