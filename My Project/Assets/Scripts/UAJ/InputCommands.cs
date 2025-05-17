using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCommands : MonoBehaviour {

    [SerializeField] private bool showDebugButtons = false;
    [SerializeField] private GameObject debugButtons = null;

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

    private void Start() {
        interactor = GetComponent<ButtonInteractor>();
        if (showDebugButtons)
            debugButtons.SetActive(true);
        else
            debugButtons.SetActive(false);
    }
    public void StartGame() {
        MenuNavigationManager navigationManager = FindObjectOfType<MenuNavigationManager>();
        if (navigationManager)
        {
            // EventoBot("Menu de Niveles") 
            EventQueue.Instance().AddEvent(new GameEvent(EventType.LevelsMenu));
            navigationManager.GoTo(1);
        }
    }
    public void SelectLevel(int level) {
        interactor.StartLevel(level);
    }
    public void NextDialogue() {
        ConversationManager.Instance.NextDialogue();
    }
    public void SelectOption(int option) {
        ConversationManager.Instance.SelectResponse(option);
    }
    public void InteractWithCharacter(string character) {
        DialogableCharacter[] characters = FindObjectsByType<DialogableCharacter>(FindObjectsSortMode.None);
        foreach (var chara in characters) {
            if (chara.GetConversationID() == character) {
                chara.StartConversation();
                break;
            }
        }
    }
    public void EndLevel() {
        ConversationManager.Instance.NextDialogue();
    }
}
