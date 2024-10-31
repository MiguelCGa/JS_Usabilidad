using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogable : IInteractable {
    public override void OnClick() {
        Debug.Log("Empieza dialogo");
    }
}
