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

    [SerializeField]
    Image gold;
    [SerializeField]
    Image silver;
    [SerializeField]
    Image bronze;
    [SerializeField]
    Image noMedals;

    [SerializeField]
    float currentTension = 0;

    public void SetTension(float tension) { 
        currentTension = tension;
    }
    private void OnDestroy() {
        controller.SetDisplay(null);
    }

    private void updateMedals()
    {
        if (slider.fillAmount <= controller.GetNormalizedGold()) 
        { 
            gold.enabled = true;
            silver.enabled = false;
            bronze.enabled = false;
            noMedals.enabled = false;
        }
        else if (slider.fillAmount <= controller.GetNormalizedSilver())
        {
            silver.enabled = true;
            gold.enabled = false;
            bronze.enabled = false;
            noMedals.enabled = false;
        }
        else if (slider.fillAmount <= controller.GetNormalizedBronze())
        {
            bronze.enabled = true;
            gold.enabled = false;
            silver.enabled = false;
            noMedals.enabled = false;
        }
        else
        {
            noMedals.enabled = true;
            gold.enabled = false;
            silver.enabled = false;
            bronze.enabled = false;
        }
    }
    // Start is called before the first frame update
    void Start() {
        currentTension = controller.SetDisplay(this);
        slider.fillAmount = currentTension;
        slider.color = new Color(Mathf.Clamp(slider.fillAmount * 2, 0, 1), Mathf.Clamp((1 - slider.fillAmount) * 2, 0, 1), 0);
        updateMedals();
    }

    // Update is called once per frame
    void Update() {
        if (currentTension != slider.fillAmount) {
            slider.color = new Color(Mathf.Clamp(slider.fillAmount * 2, 0, 1), Mathf.Clamp((1 - slider.fillAmount) * 2, 0, 1), 0);
            slider.fillAmount = Mathf.Lerp(slider.fillAmount, currentTension, slidingFactor);
            updateMedals();
        }
            
    }
}
