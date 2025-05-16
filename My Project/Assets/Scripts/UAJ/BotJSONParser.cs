using System;
using System.Collections.Generic;
using System.IO;
using Defective.JSON;

public class BotJSONParser 
{
    public Dictionary<string, List<Tuple<string, int>>> responseDictionary { get; private set; }
    public Dictionary<string, Tuple<string, string>> dialogueDictionary { get; private set; }

    public void LoadDialogueJson(string level)
    {
        dialogueDictionary = new Dictionary<string, Tuple<string, string>>(); ;

        using (StreamReader r = new StreamReader(level))
        {
            string json = r.ReadToEnd();

            JSONObject levelJsonObject = new JSONObject(json);
            JSONObject dialogueJsonObject = levelJsonObject.list[0];
            string character;
            string lastResponse;
            for (int i = 0; i < dialogueJsonObject.list.Count; ++i)
            {
                JSONObject dialogues = dialogueJsonObject.list[i];
                character = dialogues.list[dialogues.list.Count - 1].GetField("character").stringValue;
                lastResponse = dialogues.list[dialogues.list.Count - 1].GetField("Responses").stringValue;
                dialogueDictionary.Add(dialogueJsonObject.keys[i], new Tuple<string, string>(character, lastResponse));
            }
        }
    }

    public void LoadResponsesJson(string level)
    {
        responseDictionary = new Dictionary<string, List<Tuple<string, int>>>();

        using (StreamReader r = new StreamReader(level))
        {
            string json = r.ReadToEnd();

            JSONObject levelJsonObject = new JSONObject(json);
            JSONObject responseJsonObject = levelJsonObject.list[0];

            for (int i = 0; i < responseJsonObject.list.Count; ++i)
            {
                responseDictionary.Add(responseJsonObject.keys[i], new List<Tuple<string, int>>());
                for (int j = 0; j < responseJsonObject.list[i].count; ++j)
                {
                    JSONObject dialogues = responseJsonObject.list[i];
                    string nextDialogueGroup = dialogues.list[j].GetField("nextDialogueGroup").stringValue;
                    int tension = dialogues.list[j].GetField("tension").intValue;
                    responseDictionary[responseJsonObject.keys[i]].Add(new Tuple<string, int>(nextDialogueGroup, tension));
                }
            }
        }
    }
}
