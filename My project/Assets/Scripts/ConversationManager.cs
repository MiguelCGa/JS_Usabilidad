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
    [SerializeField]
    private TensionDisplay finalTensionMeter;

    private DialogueBox dialogueBox;
    private ContextController contextController;

    private DialogueGroup currentConversation;
    private int currentDialogue;
    private ResponseGroup currentResponses;
    private string currentLevel;

    bool dialoging = false;
    bool finishingLevel = false;
    bool concludingLevel = false;
    bool inContext = false;
    bool inputActive = true;

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
        // EventoBot("Respuesta/Emoción inciada", responses) 
        EventQueue.Instance().AddEvent(new GameEvent(EventType.ResponseStarted, responses));
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
    private DialogueGroup GetLevelConclusionDialogue() {
        switch (tensionController.GetCompletionLevel()) {
            case TensionCompletion.FAILED:
                return data.GetLevelLostConclusion(currentLevel);
            case TensionCompletion.BRONZE:
                return data.GetLevelBronzeConclusion(currentLevel);
            case TensionCompletion.SILVER:
                return data.GetLevelSilverConclusion(currentLevel);
            case TensionCompletion.GOLD:
                return data.GetLevelGoldConclusion(currentLevel);
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

        InputReader.Instance.onUse += () => { if (inputActive) NextDialogue(); };
    }

    public void EnableInput() {
        inputActive = true;
    }
    public void DisableInput() {
        inputActive = false;
    }

    public void SetCurrentLevel(string level) {
        currentLevel = level;
        tensionController.SetInitialTension(data.GetLevelInitialTension(currentLevel));
        tensionMeter.SetActive(true);
        finalTensionMeter.ForceTension(1);
        finalTensionMeter.gameObject.SetActive(false);
        stageManager.Restart();
        DialogueGroup context = data.GetLevelInitialContext(currentLevel);
        if (context != null) {
            dialoging = true;
            inContext = true;
            contextController.ActivateContext();
            currentConversation = context;
            InitConversation(true);
        }
    }

    public void SetDialogBox(DialogueBox box) {
        dialogueBox = box;
        contextController = box.gameObject.GetComponent<ContextController>();
    }

    private void InitConversation(bool preparing = false) {
        currentDialogue = 0;
        Dialogue d = GetCurrentDialogue();
        InitDialogue(d, preparing);
    }
    private void InitDialogue(Dialogue d, bool preparing = false) {
        tensionController.AddTension(d.tension);
        if (preparing) dialogueBox.prepareDialogue(d.Text, d.character);
        else dialogueBox.dialogue(d.Text, d.character);
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

    public bool NextDialogue() {
        if (!dialoging)
            return false;
        if (dialogueBox.Next())
            return true;
        if (CheckResponses())
            return true;
        if (CheckNextStage())
            return true;
        Dialogue d = GetNextDialogue();
        if (d != null) {
            InitDialogue(d);
            return true;
        }
        StopDialogue();
        return true;
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

    public bool SelectResponse(int id) {
        if (id >= currentResponses.responses.Length)
            return false;
        EventQueue.Instance().AddEvent(new GameEvent(EventType.SelectedResponse, id));

        responseManager.HideResponses();
        Response resp = currentResponses.responses[id];
        tensionController.AddTension(resp.tension);
        if (resp.nextStage != null && stageManager.SetStage(resp.nextStage)) 
            return true;
        currentConversation = data.GetDialogueGroupByID(currentLevel, resp.nextDialogueGroup);
        InitConversation();
        return true;
    }

    public void StopDialogue() {
        //EventoBot("Dialogo Terminado");
        EventQueue.Instance().AddEvent(new GameEvent(EventType.ConversationEnded));
        if (inContext) {
            EndContext();
        }
        dialoging = false;
        dialogueBox.gameObject.SetActive(false);
        if (finishingLevel) {
            //EventoBot("Nivel Concluido");
            EventQueue.Instance().AddEvent(new GameEvent(EventType.LevelConclusion));
            finishingLevel = false;
            concludingLevel = true;
            tensionMeter.SetActive(false);
            finalTensionMeter.gameObject.SetActive(true);
            finalTensionMeter.SetTension(tensionController.GetNormalizedTension());
            dialoging = true;
            dialogueBox.gameObject.SetActive(true);
            currentConversation = GetLevelConclusionDialogue();
            InitConversation();
        }
        else if (concludingLevel) {
            concludingLevel = false;
            GameManager.Instance.CompleteLevel(tensionController.GetCompletionLevel());
            finalTensionMeter.gameObject.SetActive(false);
            finalTensionMeter.ForceTension(1);
        }
    }

    public void EndContext()
    {
        inContext = false;
        contextController.DeactivateContext();
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
