using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Defective.JSON;
using UnityEngine;

public class BotJSONParser 
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

    /*
     * Tipos de errores:
     * No existe dialogo Inicial
     * El orden de los dialogos es incorrecto
     * Tensiones de rutas mal implementadas en el juego 
     * Dialogos sin respuestas correspondientes y vi
     * 
     */

    private Dictionary<string, List<Tuple<string, int>>> responseDictionary;
    private Dictionary<string, DialogueInfo> dialogueDictionary;

    private List<RouteInfo> Routes;

    private List<string> characters;

    private static BotJSONParser instance;
    
   static  public BotJSONParser Instance()
    {

        if (instance == null)
        {

            instance = new BotJSONParser();
            return instance;
        }

        else
        {

            return instance;

           }
    }

    public List<RouteInfo> ParseLevel(string level)
    {
        Routes = new List<RouteInfo>();
        characters = new List<string>();
        LoadDialogueJson(Application.dataPath + "/Scripts/Dialogues/"+level+"/Dialogues"+level +".json");
        LoadResponsesJson(Application.dataPath + "/Scripts/Dialogues/"+level+"/Responses"+level+".json");
        Pathfinder("Inicial" + characters[0], new List<int>(), new HashSet<string>(), 0, 0);
        return Routes;


    }
   /* private void Start()
    {
        Routes = new List<RouteInfo>();
        characters = new List<string>();
        LoadDialogueJson(Application.dataPath + "/Scripts/Dialogues/Tutorial/DialoguesTutorial.json");
        LoadResponsesJson(Application.dataPath + "/Scripts/Dialogues/Tutorial/ResponsesTutorial.json");

        Pathfinder("Inicial" + characters[0], new List<int>(), new HashSet<string>(), 0, 0);

        int patata = 14;
    }*/

    

   private void LoadDialogueJson(string level)
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

                foreach (var dialogue in dialogues)
                {
                    string auxCharacter = dialogue.GetField("character").stringValue;
                    if (!characters.Contains(auxCharacter) && auxCharacter != "Yo")
                    {
                        characters.Add(auxCharacter);
                    }
                }
                character = dialogues.list[dialogues.list.Count - 1].GetField("character").stringValue;
                lastResponse = dialogues.list[dialogues.list.Count - 1].GetField("Responses").stringValue;

                JSONObject unlockJSONObject = dialogues.list[dialogues.list.Count - 1].GetField("unlock");

                dialogueDictionary.Add(dialogueJsonObject.keys[i], new DialogueInfo(character, lastResponse, unlockJSONObject?.stringValue));
            }

            int patata = 14;
        }
    }

   private void LoadResponsesJson(string level)
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

    private void Pathfinder(string firstDialogueName, List<int> dialogueList, HashSet<string> processedResponses,
        int accumulativeTension, int currCharacter)
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

        if (currDialogue.unlock != null)
        {
            Pathfinder(currDialogue.unlock, dialogueList, processedResponses, accumulativeTension, currCharacter);
        }
        else if (currDialogue.response == "None")
        {
            // Terminada ruta de personaje actual
            dialogueList.Add(-1);
            currCharacter++;
            while (!dialogueDictionary.ContainsKey("Inicial" + characters[currCharacter]) && currCharacter < characters.Count)
                currCharacter++;

            Pathfinder("Inicial" + characters[currCharacter], dialogueList, processedResponses, accumulativeTension, currCharacter);
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

                    Pathfinder(responses.Item1, tempDialogues, tempResponses, accumulativeTension + responses.Item2, currCharacter);
                }
            }
        }
    }
}
