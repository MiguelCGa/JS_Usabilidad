using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCommands : MonoBehaviour
{
    [SerializeField] private MenuNavigationManager navigationManager;

    private ButtonInteractor interactor;
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

    private void Start()
    {
        interactor = GetComponent<ButtonInteractor>();
    }
    public void StartGame() {
        // Llamar a la función del botón de iniciar juego
        if (navigationManager)
            navigationManager.GoTo(1);
    }
    public void SelectLevel(int level) {
        // Llamar a ButtonInteractor StartLevel()
        interactor.StartLevel(level);
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
        ConversationManager.Instance.StartConversation(character);
    }
    public void EndLevel() {
        // Llamar a ConversationManager NextDialogue() creo?
        ConversationManager.Instance.NextDialogue();
    }
}
