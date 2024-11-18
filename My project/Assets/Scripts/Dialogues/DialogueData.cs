using Defective.JSON;
using System.Collections.Generic;
using UnityEngine;

public class DialogueData : MonoBehaviour
{
    [SerializeField]
    private TextAsset dialoguesJsonFile;
    [SerializeField]
    private TextAsset dialoguesJsonFile1;
    [SerializeField]
    private TextAsset dialoguesJsonFile2;
    [SerializeField]
    private TextAsset dialoguesJsonFile3;
    [SerializeField]
    private TextAsset dialoguesJsonFile4;
    [SerializeField]
    private TextAsset responsesJsonFile;

    private DialogueGroup myDialogueGroup;
    private DialogueGroup myDialogueGroup1;
    private DialogueGroup myDialogueGroup2;
    private DialogueGroup myDialogueGroup3;
    private DialogueGroup myDialogueGroup4;
    private ResponseGroup myResponseGroup;
    private JSONObject myJson;

    void Awake()
    {
        myJson = new JSONObject(dialoguesJsonFile.text);
        string neutralData =myJson.GetField("Neutral");
        Dictionary<string,string> testObj = myJson.ToDictionary();
        myDialogueGroup = JsonUtility.FromJson<DialogueGroup>(dialoguesJsonFile.text);
        myDialogueGroup1 = JsonUtility.FromJson<DialogueGroup>(dialoguesJsonFile1.text);
        myDialogueGroup2 = JsonUtility.FromJson<DialogueGroup>(dialoguesJsonFile2.text);
        myDialogueGroup3 = JsonUtility.FromJson<DialogueGroup>(dialoguesJsonFile3.text);
        myDialogueGroup4 = JsonUtility.FromJson<DialogueGroup>(dialoguesJsonFile4.text);
        myResponseGroup = JsonUtility.FromJson<ResponseGroup>(responsesJsonFile.text);
    }

    public DialogueGroup GetDialogueGroupByID(string id)
    {
        DialogueGroup dialogueGroup = null;
        switch (id){
            case "InitialDialogue":
                dialogueGroup = myDialogueGroup;
                break;
            case "Neutral":
                dialogueGroup = myDialogueGroup1;
                break;
            case "Receptor":
                dialogueGroup = myDialogueGroup2;
                break;
            case "Sentimientos":
                dialogueGroup = myDialogueGroup3;
                break;
            case "FutbolMal":
                dialogueGroup = myDialogueGroup4;
                break;
        }
        return dialogueGroup;
    }
    public ResponseGroup GetResponseGroupByID(string id)
    {
        ResponseGroup responseGroup = myResponseGroup;
        return responseGroup;
    }
}
[System.Serializable]
public class DialogueGroup
{
    public Dialogue[] dialogues;
}
[System.Serializable]
public class Dialogue
{
    public string character;
    public string Text;
    public string Responses;
}
[System.Serializable]
public class ResponseGroup
{
    public Response[] responses;
}
[System.Serializable]
public class Response
{
    public string text;
    public string nextDialogueGroup;
}