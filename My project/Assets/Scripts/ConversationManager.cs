using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent (typeof(DialogueData))]
[RequireComponent (typeof(ResponseDisplayManager))]
[RequireComponent (typeof(TensionController))]

public class ConversationManager : MonoBehaviour {

    private DialogueData data;
    private ResponseDisplayManager responseManager;
    private TensionController tensionController;

    [SerializeField]
    private GameObject tensionMeter;

    private DialogueBox dialogueBox;

    private DialogueGroup currentConversation;
    private int currentDialogue;
    private ResponseGroup currentResponses;
    private string currentLevel;

    bool dialoging = false;
    bool finishingLevel = false;

    public static ConversationManager Instance { get; private set; }


    private Dialogue GetCurrentDialogue() {
        if (currentDialogue >= currentConversation.dialogues.Length)
            return null;
        return currentConversation.dialogues[currentDialogue];
    }

    private Dialogue GetNextDialogue() {
        ++currentDialogue;
        return GetCurrentDialogue();
    }

    private bool CheckResponses() {
        string responses = GetCurrentDialogue().Responses;
        if (responses == "None")
            return false;
        if (responses == "EndLevel") {
            finishingLevel = true;
            currentConversation = GetLevelEndDialogue();
            InitDialogue();
            return true;
        }
        currentResponses = data.GetResponseGroupByID(currentLevel, responses);
        responseManager.SetResponses(currentResponses, GetCurrentDialogue().emotions);
        return true;
    }

    private DialogueGroup GetLevelEndDialogue() {
        switch (tensionController.GetCompletionLevel()) {
            case TensionCompletion.FAILED:
                return data.GetLevelLost(currentLevel);
            case TensionCompletion.BRONZE:
                return data.GetLevelBronze(currentLevel);
            case TensionCompletion.SILVER:
                return data.GetLevelSilver(currentLevel);
            case TensionCompletion.GOLD:
                return data.GetLevelGold(currentLevel);
        }
        return null;
    }

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
        data = GetComponent<DialogueData>();
        responseManager = GetComponent<ResponseDisplayManager>();
        tensionController = GetComponent<TensionController>();

        InputReader.Instance.onUse += NextDialogue;
    }

    public void SetCurrentLevel(string level) {
        currentLevel = level;
        tensionMeter.SetActive(true);
    }

    public void SetDialogBox(DialogueBox box) {
        dialogueBox = box;
    }

    private void InitDialogue() {
        currentDialogue = 0;
        Dialogue d = GetCurrentDialogue();
        tensionController.AddTension(d.tension);
        dialogueBox.dialogue(d.Text, d.character);
    }

    public void StartConversation(string id) {
        dialoging = true;
        dialogueBox.gameObject.SetActive(true);
        currentConversation = data.GetDialogueGroupByID(currentLevel, id);
        InitDialogue();
    }

    void NextDialogue() {
        if (!dialoging)
            return;
        if (dialogueBox.Next())
            return;
        if (CheckResponses())
            return;
        Dialogue d = GetNextDialogue();
        if (d != null) {
            tensionController.AddTension(d.tension);
            dialogueBox.dialogue(d.Text, d.character);
            return;
        }
        StopDialogue();
    }

    public void SelectResponse(int id) {
        responseManager.HideResponses();
        Response resp = currentResponses.responses[id];
        tensionController.AddTension(resp.tension);
        currentConversation = data.GetDialogueGroupByID(currentLevel, resp.nextDialogueGroup);
        InitDialogue();
    }

    public void StopDialogue() {
        dialoging = false;
        dialogueBox.gameObject.SetActive(false);
        if (finishingLevel) {
            finishingLevel = false;
            GameManager.Instance.CompleteLevel(tensionController.GetCompletionLevel());
            tensionMeter.SetActive(false);
        }
    }
}
