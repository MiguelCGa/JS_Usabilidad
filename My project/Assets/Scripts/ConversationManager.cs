using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent (typeof(DialogueData))]
[RequireComponent (typeof(ResponseDisplayManager))]
[RequireComponent (typeof(TensionController))]
[RequireComponent (typeof(StageManager))]

public class ConversationManager : MonoBehaviour {

    private DialogueData data;
    private ResponseDisplayManager responseManager;
    private TensionController tensionController;
    public StageManager stageManager { get; private set; }

    [SerializeField]
    private GameObject tensionMeter;

    private DialogueBox dialogueBox;
    private ContextController contextController;

    private DialogueGroup currentConversation;
    private int currentDialogue;
    private ResponseGroup currentResponses;
    private string currentLevel;

    bool dialoging = false;
    bool finishingLevel = false;
    bool inContext = false;

    Dictionary<string, DialogableCharacter> unlockableConversations = new Dictionary<string, DialogableCharacter>();

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
            InitConversation();
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
        stageManager = GetComponent<StageManager>(); 

        InputReader.Instance.onUse += NextDialogue;
    }

    public void SetCurrentLevel(string level) {
        currentLevel = level;
        tensionController.SetInitialTension(data.GetLevelInitialTension(currentLevel));
        tensionMeter.SetActive(true);
        stageManager.Restart();
        DialogueGroup context = data.GetLevelInitialContext(currentLevel);
        if (context != null) {
            dialoging = true;
            inContext = true;
            contextController.ActivateContext();
            currentConversation = context;
            InitConversation();
        }
    }

    public void SetDialogBox(DialogueBox box) {
        dialogueBox = box;
        contextController = box.gameObject.GetComponent<ContextController>();
    }

    private void InitConversation() {
        currentDialogue = 0;
        Dialogue d = GetCurrentDialogue();
        InitDialogue(d);
    }
    private void InitDialogue(Dialogue d) {
        tensionController.AddTension(d.tension);
        dialogueBox.dialogue(d.Text, d.character);
        DoUnlock(d);
    }

    public void StartConversation(string id) {
        if (dialoging)
            return;
        dialoging = true;
        dialogueBox.gameObject.SetActive(true);
        currentConversation = data.GetDialogueGroupByID(currentLevel, id);
        InitConversation();
    }

    void NextDialogue() {
        if (!dialoging)
            return;
        if (dialogueBox.Next())
            return;
        if (CheckResponses())
            return;
        if (CheckNextStage())
            return;
        Dialogue d = GetNextDialogue();
        if (d != null) {
            InitDialogue(d);
            return;
        }
        StopDialogue();
    }

    private bool CheckNextStage() {
        string nextStage = GetCurrentDialogue().nextStage;
        if (nextStage == null)
            return false;
        if (stageManager.SetStage(nextStage)) {
            StopDialogue();
            return true;
        }
        return false;
    }

    public void SelectResponse(int id) {
        responseManager.HideResponses();
        Response resp = currentResponses.responses[id];
        tensionController.AddTension(resp.tension);
        if (resp.nextStage != null && stageManager.SetStage(resp.nextStage)) 
            return;
        currentConversation = data.GetDialogueGroupByID(currentLevel, resp.nextDialogueGroup);
        InitConversation();
    }

    public void StopDialogue() {
        if (inContext) {
            inContext = false;
            contextController.DeactivateContext();
        }
        dialoging = false;
        dialogueBox.gameObject.SetActive(false);
        if (finishingLevel) {
            finishingLevel = false;
            GameManager.Instance.CompleteLevel(tensionController.GetCompletionLevel());
            tensionMeter.SetActive(false);
        }
    }

    public void AddUnlockableConversation(string name, DialogableCharacter character) {
        if (unlockableConversations.ContainsKey(name))
            return;
        unlockableConversations.Add(name, character);
    }

    public void RemoveUnlockableConversation(string name) {
        unlockableConversations.Remove(name);
    }

    private void DoUnlock(Dialogue dialogue) {
        if (dialogue.unlock == null)
            return;
        unlockableConversations[dialogue.unlock].Unlock();
    }
}
