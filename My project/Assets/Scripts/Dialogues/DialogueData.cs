using Defective.JSON;
using System.Collections.Generic;
using UnityEngine;

using LevelDialogues = System.Collections.Generic.Dictionary<string, DialogueGroup>;
using LevelResponses = System.Collections.Generic.Dictionary<string, ResponseGroup>;

public class DialogueData : MonoBehaviour
{
    [SerializeField]
    private TextAsset[] dialoguesJsonFiles;
    [SerializeField]
    private TextAsset[] responsesJsonFiles;

    private Dictionary<string, Level> levels;

    void Awake() {
        levels = new Dictionary<string, Level>();

        foreach (TextAsset dialoguesFile in dialoguesJsonFiles) {
            JSONObject conversationsJson = new JSONObject(dialoguesFile.text);
            string levelName = conversationsJson.keys[0];
            levels[levelName] = new Level(conversationsJson, levelName);
        }

        foreach (TextAsset responsesFile in responsesJsonFiles) {
            JSONObject responsesJson = new JSONObject(responsesFile.text);
            string levelName = responsesJson.keys[0];
            InitLevelResponses(responsesJson[levelName], ref levels[levelName].responses);
        }
    }

    private void InitLevelResponses(JSONObject responsesJson, ref LevelResponses levelResponses) {
        levelResponses = new LevelResponses();
        for (int i = 0; i < responsesJson.count; ++i) {
            string key = responsesJson.keys[i];
            levelResponses[key] = new ResponseGroup(responsesJson.GetField(key));
        }
    }

    public DialogueGroup GetDialogueGroupByID(string level, string id) {
        return levels[level].dialogues[id];
    }
    public ResponseGroup GetResponseGroupByID(string level, string id) {
        return levels[level].responses[id];
    }
    public DialogueGroup GetLevelLost(string level) {
        return levels[level].loseDialogue;
    }
    public DialogueGroup GetLevelBronze(string level) {
        return levels[level].bronzeDialogue;
    }
    public DialogueGroup GetLevelSilver(string level) {
        return levels[level].silverDialogue;
    }
    public DialogueGroup GetLevelGold(string level)
    {
        return levels[level].goldDialogue;
    }
    public float GetLevelInitialTension(string level) {
        return levels[level].initialTension;
    }
}

public class Level {
    public LevelDialogues dialogues;
    public DialogueGroup loseDialogue;
    public DialogueGroup bronzeDialogue;
    public DialogueGroup silverDialogue;
    public DialogueGroup goldDialogue;
    public LevelResponses responses;
    public float initialTension = 5f;
    public Level(JSONObject conversationsJson, string levelName) {
        dialogues = GetLevelDialogues(conversationsJson.GetField(levelName));
        loseDialogue = GetFinishDialogues(conversationsJson, "Lost");
        bronzeDialogue = GetFinishDialogues(conversationsJson, "Bronze");
        silverDialogue = GetFinishDialogues(conversationsJson, "Silver");
        goldDialogue = GetFinishDialogues(conversationsJson, "Gold");
        var it = conversationsJson.GetField("InitialTension");
        if (it != null)
            initialTension = it.floatValue;
    }
    LevelDialogues GetLevelDialogues(JSONObject conversationsJson) {
        LevelDialogues conversations = new LevelDialogues();
        for (int i = 0; i < conversationsJson.count; ++i) {
            string key = conversationsJson.keys[i];
            conversations[key] = new DialogueGroup(conversationsJson.GetField(key));
        }
        return conversations;
    }

    DialogueGroup GetFinishDialogues(JSONObject conversationsJson, string key) {
        return new DialogueGroup(conversationsJson.GetField(key));
    }
}

public class DialogueGroup {
    public Dialogue[] dialogues;
    public DialogueGroup(JSONObject jsonObject) {
        dialogues = new Dialogue[jsonObject.count];
        for (int i = 0; i < jsonObject.count; ++i) {
            dialogues[i] = new Dialogue(jsonObject[i]);
        }
    }
}

public class Dialogue {
    public string character;
    public string Text;
    public string Responses;
    public bool emotions = false;
    public float tension = 0.0f;
    public string unlock = null;
    public string nextStage = null;
    public Dialogue(JSONObject jsonObject) {
        character = jsonObject.GetField("character").stringValue;
        Text = jsonObject.GetField("Text").stringValue;
        Responses = jsonObject.GetField("Responses").stringValue;
        var emo = jsonObject.GetField("emotions");
        if (emo != null)
            emotions = emo.boolValue;
        var ten = jsonObject.GetField("tension");
        if (ten != null)
            tension = ten.floatValue;
        var unl = jsonObject.GetField("unlock");
        if (unl != null)
            unlock = unl.stringValue;
        var nStg = jsonObject.GetField("nextStage");
        if (nStg != null)
            nextStage = nStg.stringValue;
    }
}

public class ResponseGroup {
    public Response[] responses;
    public ResponseGroup(JSONObject jsonObject) {
        responses = new Response[jsonObject.count];
        for (int i = 0; i < jsonObject.count; ++i) {
            responses[i] = new Response(jsonObject[i]);
        }
    }
}

public class Response {
    public string text;
    public string nextDialogueGroup;
    public float tension = 0.0f;
    public string nextStage = null;
    public Response(JSONObject jsonObject) {
        text = jsonObject.GetField("text").stringValue;
        nextDialogueGroup = jsonObject.GetField("nextDialogueGroup").stringValue;
        var ten = jsonObject.GetField("tension");
        if (ten != null)
            tension = ten.floatValue;
        var nStg = jsonObject.GetField("nextStage");
        if (nStg != null)
            nextStage = nStg.stringValue;
    }
}