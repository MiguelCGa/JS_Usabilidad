using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogableCharacter : MonoBehaviour
{
    [SerializeField]
    string conversation;
    [SerializeField]
    bool locked = false;
    Button button;

    public void Start() {
        button = GetComponent<Button>();
        button.interactable = !locked;
        if (locked)
            ConversationManager.Instance.AddUnlockableConversation(conversation, this);
    }
    private void OnDestroy()
    {
        if (locked)
            ConversationManager.Instance.RemoveUnlockableConversation(conversation);
    }

    public void StartConversation() {
        ConversationManager.Instance.StartConversation(conversation);
        DeactivateInteraction();
    }

    public void DeactivateInteraction() {
        button.interactable = false;
    }

    public void Unlock() {
        button.interactable = true;
    }
}
