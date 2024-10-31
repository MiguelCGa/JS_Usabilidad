using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : IInteractable {
    [SerializeField] string sceneToStart = "";

    public override void OnClick() {
        SceneManager.LoadScene(sceneToStart);
    }
}
