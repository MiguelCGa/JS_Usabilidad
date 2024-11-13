using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent (typeof(DialogueData))]
[RequireComponent (typeof(ResponseDisplayManager))]

public class ConversationManager : MonoBehaviour {

    private DialogueData data;
    private ResponseDisplayManager responseManager;

    private DialogueBox dialogueBox;

    private DialogueGroup currentConversation;
    private int currentDialogue;
    private ResponseGroup currentResponses;

    bool dialoging = false;

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
        currentResponses = data.GetResponseGroupByID(responses);
        responseManager.SetResponses(currentResponses);
        return true;
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

        InputReader.Instance.onUse += NextDialogue;
        StartConversation("InitialDialogue");
    }

    public void SetDialogBox(DialogueBox box) {
        dialogueBox = box;
    }

    private void InitDialogue() {
        currentDialogue = 0;
        dialogueBox.dialogue(GetCurrentDialogue().Text, "TO DO: poner nombre de quien habla en json. atte. ConversationManager");
    }

    public void StartConversation(string id) {
        dialoging = true;
        dialogueBox.gameObject.SetActive(true);
        currentConversation = data.GetDialogueGroupByID(id);
        InitDialogue();
    }

    void NextDialogue() {
        if (!dialoging)
            return;
        if (dialogueBox.Next())
            return;
        if (CheckResponses())
            return;
        if (GetNextDialogue() != null)
            return;
        StopDialogue();
    }

    public void SelectResponse(int id) {
        responseManager.HideResponses();
        currentConversation = data.GetDialogueGroupByID(currentResponses.responses[id].nextDialogueGroup);
        InitDialogue();
    }

    public void StopDialogue() {
        dialoging = false;
        dialogueBox.gameObject.SetActive(false);
    }
}
