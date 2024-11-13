using System.Collections.Generic;
using UnityEngine;

public class DialogueData : MonoBehaviour
{
    [SerializeField]
    private TextAsset dialoguesJsonFile;
    [SerializeField]
    private TextAsset responsesJsonFile;

    private DialogueGroup myDialogueGroup;
    private ResponseGroup myResponseGroup;

    void Awake()
    {
        myDialogueGroup = JsonUtility.FromJson<DialogueGroup>(dialoguesJsonFile.text);
        myResponseGroup = JsonUtility.FromJson<ResponseGroup>(responsesJsonFile.text);
    }

    public DialogueGroup GetDialogueGroupByID(string id)
    {
        DialogueGroup dialogueGroup = null;
        //for(int i = 0; i < myDialogueGroup.Length; i++)
        //{
        //    //if(id == myDialogueGroup[i].id)
        //    //{
        //    //    dialogueGroup = myDialogueGroup[i];
        //    //}
        //}
        return dialogueGroup;
    }
    public ResponseGroup GetResponseGroupByID(string id)
    {
        ResponseGroup responseGroup = null;
        //for(int i = 0; i < myResponseGroup.Length; i++)
        //{
        //    //if(id == myResponseGroup[i].id)
        //    //{
        //    //    responseGroup = myResponseGroup[i];
        //    //}
        //}
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