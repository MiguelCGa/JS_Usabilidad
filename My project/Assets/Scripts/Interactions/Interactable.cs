using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : IInteractable {
    [SerializeField] UnityEvent onClick;
    public override void OnClick() {
        onClick?.Invoke();
    }
}
