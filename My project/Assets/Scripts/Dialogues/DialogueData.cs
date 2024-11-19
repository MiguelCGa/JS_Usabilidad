using Defective.JSON;
using System.Collections.Generic;
using UnityEngine;

public class DialogueData : MonoBehaviour
{
    [SerializeField]
    private TextAsset dialoguesJsonFile;
    [SerializeField]
    private TextAsset responsesJsonFile;

    private Dictionary<string, DialogueGroup> conversations;
    private Dictionary<string, ResponseGroup> convResponses;

    void Awake() {
        conversations = new Dictionary<string, DialogueGroup>();
        JSONObject conversationsJson = new JSONObject(dialoguesJsonFile.text);
        for (int i = 0; i < conversationsJson.count; ++i) {
            string key = conversationsJson.keys[i];
            conversations[key] = new DialogueGroup(conversationsJson.GetField(key));
        }

        convResponses = new Dictionary<string, ResponseGroup>();
        JSONObject convResponsesJson = new JSONObject(responsesJsonFile.text);
        for (int i = 0; i  < convResponsesJson.count; ++i) {
            string key = convResponsesJson.keys[i];
            convResponses[key] = new ResponseGroup(convResponsesJson.GetField(key));
        }

    }

    public DialogueGroup GetDialogueGroupByID(string id) {
        return conversations[id];
    }
    public ResponseGroup GetResponseGroupByID(string id) {
        return convResponses[id];
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

    public Dialogue(JSONObject jsonObject) {
        character = jsonObject.GetField("character").stringValue;
        Text = jsonObject.GetField("Text").stringValue;
        Responses = jsonObject.GetField("Responses").stringValue;
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
    public Response(JSONObject jsonObject) { 
        text = jsonObject.GetField("text").stringValue;
        nextDialogueGroup = jsonObject.GetField("nextDialogueGroup").stringValue;
    }
}