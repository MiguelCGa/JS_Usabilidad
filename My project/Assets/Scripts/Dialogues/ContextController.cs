using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextController : MonoBehaviour {
    [SerializeField]
    Image contextBackground = null;
    [SerializeField]
    float fadeDuration = 2f;
    bool contextShown = false;

    float timer = -1;
    private void OnEnable() {
        if (contextShown) {
            contextBackground.gameObject.SetActive(false);
        }
    }
    public void ActivateContext() {
        contextBackground.gameObject.SetActive(true);
    }
    public void DeactivateContext() {
        contextBackground.gameObject.SetActive(false);
    }

}
