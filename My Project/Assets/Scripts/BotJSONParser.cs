using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Defective.JSON;
using UnityEngine;

public class BotJSONParser : MonoBehaviour
{
    public struct DialogueInfo
    {
        public string character, response, unlock;

        public DialogueInfo(string c, string r, string u)
        {
            character = c;
            response = r;
            unlock = u;
        }
    }

    public struct RouteInfo
    {
        public List<int> Responses;
        public int TotalTension;

        public RouteInfo(List<int> r, int t)
        {
            Responses = r;
            TotalTension = t;
        }
    }

    public Dictionary<string, List<Tuple<string, int>>> responseDictionary { get; private set; }
    public Dictionary<string, DialogueInfo> dialogueDictionary { get; private set; }

    public List<RouteInfo> Routes;

    private void Start()
    {
        LoadDialogueJson(Application.dataPath + "/Scripts/Dialogues/Exclusion/DialoguesExclusion.json");
        LoadResponsesJson(Application.dataPath + "/Scripts/Dialogues/Exclusion/ResponsesExclusion.json");
        Routes = new List<RouteInfo>();

        Pathfinder("InicialRaul", new List<int>(), new HashSet<string>(), 0);

        int patata = 14;
    }

    public void LoadDialogueJson(string level)
    {
        dialogueDictionary = new Dictionary<string, DialogueInfo>(); ;

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

                JSONObject unlockJSONObject = dialogues.list[dialogues.list.Count - 1].GetField("unlock");

                dialogueDictionary.Add(dialogueJsonObject.keys[i], new DialogueInfo(character, lastResponse, unlockJSONObject?.stringValue));
            }

            int patata = 14;
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

        int patata = 14;
    }

    public void Pathfinder(string firstDialogueName, List<int> dialogueList, HashSet<string> processedResponses, int accumulativeTension)
    {
        DialogueInfo currDialogue = dialogueDictionary[firstDialogueName];

        if (currDialogue.unlock == null && currDialogue.response == null)
        {
            return;
        }

        if (currDialogue.response == "EndLevel")
        {
            Routes.Add(new RouteInfo(dialogueList, accumulativeTension));
            
            return;
        }

        if(currDialogue.unlock != null)
        {
            Pathfinder(currDialogue.unlock, dialogueList, processedResponses, accumulativeTension);
        }
        else if (currDialogue.response == "None")
        {
            return;
        }
        else
        {
            for (int i = 0; i < responseDictionary[currDialogue.response].Count; ++i)
            {
                var responses = responseDictionary[currDialogue.response][i];
                if (processedResponses.Contains(currDialogue.response + i.ToString()))
                    continue;
                else
                {

                    HashSet<string> tempResponses = new HashSet<string>(processedResponses)
                    {
                        currDialogue.response + i.ToString()
                    };
                    List<int> tempDialogues = new List<int>(dialogueList)
                    {
                        i
                    };

                    Pathfinder(responses.Item1, tempDialogues, tempResponses, accumulativeTension + responses.Item2);
                }
            }
        }
    }
}
