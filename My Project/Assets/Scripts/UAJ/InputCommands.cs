using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCommands : MonoBehaviour
{
    public static InputCommands Instance { get; private set; }
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
    public void StartGame() {
        // Llamar a la función del botón de iniciar juego
    }
    public void SelectLevel(int level) {
        // Llamar a ButtonInteractor StartLevel()
        // Tener un ButtonInteractor en el mismo gameObject y hacer getComponent
    }
    public void NextDialogue() {
        // Llamar a ConversationManager NextDialogue()
        ConversationManager.Instance.NextDialogue();
    }
    public void SelectOption(int option) {
        // Llamar a ConversationManager SelectResponse()
        ConversationManager.Instance.SelectResponse(option);
    }
    public void InteractWithCharacter(string character) {
        // Llamar a ConversationManager StartConversation()
        // Comprobar que no se rompan las posibles interacciones?
    }
    public void EndLevel() {
        // Llamar a ConversationManager NextDialogue() creo?
        ConversationManager.Instance.NextDialogue();
    }
}
