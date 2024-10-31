using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : IInteractable {
    public override void OnClick() {
        Debug.Log("Interactua con el objeto");
    }
}