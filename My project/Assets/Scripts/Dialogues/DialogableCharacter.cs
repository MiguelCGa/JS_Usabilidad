using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogableCharacter : MonoBehaviour
{

    public void StartConversation(string conversation) {
        ConversationManager.Instance.StartConversation(conversation);
        DeactivateInteraction();
    }

    public void DeactivateInteraction() {
        GetComponent<Button>().enabled = false;
    }
}
