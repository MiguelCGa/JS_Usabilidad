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
    public void BackToMainMenu() { 
        GameManager.Instance.BackToMainMenu();
    }
    public void QuitApplication() {
        GameManager.Instance.Quit();
    }

    public void StartLevel(int id) {
        GameManager.Instance.StartLevel(id);
    }
}
