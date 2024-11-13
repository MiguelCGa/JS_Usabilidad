using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteractor : MonoBehaviour {
    public void ChangeScene(string sceneToStart) {
        GameManager.Instance.ChangeScene(sceneToStart);
    }
    public void Pause() {
        GameManager.Instance.Pause();
    }
    public void Resume() {
        GameManager.Instance.Resume();
    }
    public void QuitApplication() {
        GameManager.Instance.Quit();
    }

    public void StartConversation(string conversation) {
        ConversationManager.Instance.StartConversation(conversation);
        gameObject.SetActive(false);
    }
}
