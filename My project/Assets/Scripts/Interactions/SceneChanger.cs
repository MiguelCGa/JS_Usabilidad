using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : IInteractable {
    [SerializeField] string sceneToStart = "";

    public override void OnClick() {
        GameManager.Instance.ChangeScene(sceneToStart);
    }
}
