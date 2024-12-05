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
        slider.color = new Color(Mathf.Clamp(slider.fillAmount * 2, 0, 1), Mathf.Clamp((1 - slider.fillAmount) * 2, 0, 1), 0);
    }

    // Update is called once per frame
    void Update() {
        if (currentTension != slider.fillAmount)
        {

            slider.color = new Color(Mathf.Clamp(slider.fillAmount * 2, 0, 1), Mathf.Clamp((1 - slider.fillAmount) * 2, 0, 1), 0);
            slider.fillAmount = Mathf.Lerp(slider.fillAmount, currentTension, slidingFactor);
            if(slider.fillAmount <= 0)
            {
                gold.color = silver.color = bronze.color = Color.white;
            }
            else if (slider.fillAmount <= 0.25){
                gold.color = Color.black;
                silver.color = bronze.color = Color.white;
            }
            else if(slider.fillAmount <= 0.5)
            {
                gold.color = silver.color = Color.black;
                bronze.color = Color.white;
            }
            else
            {
                gold.color = silver.color = bronze.color = Color.black;
            }

        }
            
    }
}
