using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugInteractionCommand : MonoBehaviour {
    [SerializeField] private TMP_InputField input = null;
    public void Interact() {
        InputCommands.Instance.InteractWithCharacter(input.text);
    }
}
