using UnityEngine;

public class DialogueData : MonoBehaviour
{
    [SerializeField]
    private TextAsset dialoguesJsonFile;
    [SerializeField]
    private TextAsset responsesJsonFile;

    private DialogueGroup[] myDialogueGroup;
    private ResponseGroup[] myResponseGroup;

    void Start()
    {
        myDialogueGroup = JsonUtility.FromJson<DialogueGroup[]>(dialoguesJsonFile.text);
        myResponseGroup = JsonUtility.FromJson<ResponseGroup[]>(responsesJsonFile.text);
    }

    public DialogueGroup GetDialogueGroupByID(string id)
    {
        DialogueGroup dialogueGroup = null;
        for(int i = 0; i < myDialogueGroup.Length; i++)
        {
            if(id == myDialogueGroup[i].id)
            {
                dialogueGroup = myDialogueGroup[i];
            }
        }
        return dialogueGroup;
    }
    public ResponseGroup GetResponseGroupByID(string id)
    {
        ResponseGroup responseGroup = null;
        for(int i = 0; i < myResponseGroup.Length; i++)
        {
            if(id == myResponseGroup[i].id)
            {
                responseGroup = myResponseGroup[i];
            }
        }
        return responseGroup;
    }
}

[System.Serializable]
public class DialogueGroup
{
    public string id;
    public Dialogue[] dialogues;
}
[System.Serializable]
public class Dialogue
{
    public string text;
    public string nextResponseGroup;
}
[System.Serializable]
public class ResponseGroup
{
    public string id;
    public Response[] responses;
}
[System.Serializable]
public class Response
{
    public string text;
    public string nextDialogueGroup;
    public bool chosen;
}