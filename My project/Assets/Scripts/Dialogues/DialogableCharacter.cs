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
    [SerializeField]
    GameObject speechBubble;
    Button button;

    public void Start() {
        button = GetComponent<Button>();
        button.interactable = !locked;
        if (locked) {
            ConversationManager.Instance.AddUnlockableConversation(conversation, this);
            speechBubble?.SetActive(false);
        }
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
        speechBubble?.SetActive(false);
    }

    public void Unlock() {
        button.interactable = true;
        speechBubble?.SetActive(true);
    }
}
