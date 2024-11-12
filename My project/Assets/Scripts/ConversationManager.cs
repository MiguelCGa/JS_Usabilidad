using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[RequireComponent (typeof(DialogueData))]
[RequireComponent (typeof(ResponseDisplayManager))]
public class ConversationManager : MonoBehaviour {
    private DialogueData data;
    private ResponseDisplayManager responseManager;

    [SerializeField]
    private DialogueBox dialogueBox;

    private DialogueGroup currentConversation;
    private int currentDialogue;
    private ResponseGroup currentResponses;

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
        string responses = GetCurrentDialogue().nextResponseGroup;
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

        InputReader.Instance.onUse += NextDialogue;
    }

    private void InitDialogue() {
        currentDialogue = 0;
        dialogueBox.dialogue(GetCurrentDialogue().text, "TO DO: poner nombre de quien habla en json. atte. ConversationManager");
    }

    public void StartConversation(string id) {
        currentConversation = data.GetDialogueGroupByID(id);
        InitDialogue();
    }

    void NextDialogue() {
        if (dialogueBox.Next())
            return;
        CheckResponses();
        GetNextDialogue();
    }

    public void SelectResponse(int id) {
        currentConversation = data.GetDialogueGroupByID(currentResponses.responses[id].nextDialogueGroup);
        InitDialogue();
    }

    public void StopDialogue() {

    }
}
