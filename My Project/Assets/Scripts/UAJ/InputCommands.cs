using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCommands : MonoBehaviour
{

    [SerializeField] private bool showDebugButtons = false;
    [SerializeField] private GameObject debugButtons = null;

    private ButtonInteractor interactor;
    public static InputCommands Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        interactor = GetComponent<ButtonInteractor>();
        if (showDebugButtons)
            debugButtons.SetActive(true);
        else
            debugButtons.SetActive(false);
    }
    public bool StartGame()
    {
        MenuNavigationManager navigationManager = FindObjectOfType<MenuNavigationManager>();
        if (navigationManager) {
            navigationManager.GoTo(1);
            return true;
        }
        return false;
    }
    public bool SelectLevel(int level)
    {
        interactor.StartLevel(level);
        return true;
    }
    public bool NextDialogue()
    {
        return ConversationManager.Instance.NextDialogue();
    }
    public bool SelectOption(int option)
    {
        return ConversationManager.Instance.SelectResponse(option);
    }
    public bool InteractWithCharacter(string character)
    {
        DialogableCharacter[] characters = FindObjectsByType<DialogableCharacter>(FindObjectsSortMode.None);
        foreach (var chara in characters)
        {
            if (chara.GetConversationID() == character) {
                chara.StartConversation();
                return true;
            }
        }
        return false;
    }
}
