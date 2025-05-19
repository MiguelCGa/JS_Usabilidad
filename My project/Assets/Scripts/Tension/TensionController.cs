using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum TensionCompletion { FAILED, BRONZE, SILVER, GOLD };
public class TensionController : MonoBehaviour {
    float currentTension;
    [SerializeField]
    float minTension = 0, maxTension = 10;
    [SerializeField]
    float gold = 1, silver = 3, bronze = 5;

    private TensionDisplay tensionDisplay = null;

    public void SetInitialTension(float tension) {
        currentTension = tension;
    }

    public void AddTension(float tension) {
        currentTension = Mathf.Clamp(currentTension + tension, minTension, maxTension);
        tensionDisplay?.SetTension(GetNormalizedTension());
    }

    public float GetNormalizedTension() {
        return (currentTension - minTension) / (maxTension - minTension);
    }

    public float GetTension() {
        return currentTension;
    }
    public float GetNormalizedGold() {  
        return (gold - minTension) / (maxTension - minTension);
    }

    public float GetNormalizedSilver() {
        return (silver - minTension) / (maxTension - minTension);
    }

    public float GetNormalizedBronze() {  
        return (bronze - minTension) / (maxTension - minTension); 
    }

    public TensionCompletion GetCompletionLevel() {
        if (currentTension <= gold)
            return TensionCompletion.GOLD;
        if (currentTension <= silver)
            return TensionCompletion.SILVER;
        if (currentTension <= bronze)
            return TensionCompletion.BRONZE;
        return TensionCompletion.FAILED;
    }

    public float SetDisplay(TensionDisplay display) { 
        tensionDisplay = display;
        return GetNormalizedTension();
    }
}
