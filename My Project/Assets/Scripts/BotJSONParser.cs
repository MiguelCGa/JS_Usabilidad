using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Defective.JSON;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BotJSONParser
{
    public struct DialogueInfo
    {
        public string character; // persoaje con el que se interactua
        public string response;  // respuesta que se le da al personaje
        public string unlock;    // dialogo que se desbloquea al terminar la conversacion

        public DialogueInfo(string c, string r, string u)
        {
            character = c;
            response = r;
            unlock = u;
        }
    }

    public struct RouteInfo
    {
        public List<int> Responses;     // respuestas que se escogen en la ruta
        public List<string> Characters; // personajes que con los que se interactuan en la ruta
        public float TotalTension;        // tension total de la ruta

        public RouteInfo(List<int> r, List<string> c, float t)
        {
            Responses = r;
            Characters = c;
            TotalTension = t;
        }
    }

    public struct ResponseInfo
    {
        public string NextDialogueGroup; // siguiente grupo de dialogo a mostrar
        public float Tension;              // tension que se modifica al escoger la respuesta
        public int ResponseID;           // ID de la respuesta escogida

        public ResponseInfo(string d, float t, int r)
        {
            NextDialogueGroup = d;
            Tension = t;
            ResponseID = r;
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

    /* diccionario de respuestas
     * clave: nombre de la respuesta
     * valor: posibles respuestas seleccionables
     */
    private Dictionary<string, List<ResponseInfo>> responseDictionary;
    /* diccionario de dialogos
     * clave: nombre del dialogo
     * valor: Informacion del dialogo
     */
    private Dictionary<string, DialogueInfo> dialogueDictionary;
    // lista de rutas posibles
    private List<RouteInfo> Routes;
    // lista de personajes que hay en un nivel
    private List<string> characters;
    // instancia de la clase
    private static BotJSONParser instance;

    // parámetro de tensión máxima
    private float maxTension;

    private ErrorHandler errorHandler;

    static public BotJSONParser Instance()
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

    // Metodo para leer un nivel
    // @param level: nombre del nivel a leer
    // @return: lista de las rutas posibles
    public List<RouteInfo> ParseLevel(string level, float tension)
    {
        maxTension = tension;
        Routes = new List<RouteInfo>();
        characters = new List<string>();
        try
        {
            LoadDialogueJson(Application.dataPath + "/Scripts/Dialogues/" + level + "/Dialogues" + level + ".json", out float initialTension);
            LoadResponsesJson(Application.dataPath + "/Scripts/Dialogues/" + level + "/Responses" + level + ".json");
            List<string> auxCharacters = new List<string>() { "Inicial" + characters[0] };
            Pathfinder("Inicial" + characters[0], new List<int>(), new HashSet<string>(), auxCharacters, initialTension, 0);
        }
        catch (Exception e)
        {
            errorHandler.ProccessError(e, level);
        }
        return Routes;
    }

    // Metodo para leer un json de dialogos
    // @param level: nombre del nivel a leer
    private void LoadDialogueJson(string level, out float initialTension)
    {
        dialogueDictionary = new Dictionary<string, DialogueInfo>(); ;

        using (StreamReader r = new StreamReader(level))
        {
            // Se lee el json
            string json = r.ReadToEnd();

            // Se crea un objeto json a partir del string leido
            JSONObject levelJsonObject = new JSONObject(json);

            if (!levelJsonObject)
                throw new Exception("Error en la carga del archivo .json de dialogos " + level);

            // Se obtiene el primer objeto json de la lista, que equivale a los dialogos del nivel
            JSONObject dialogueJsonObject = levelJsonObject.list[0];
            if(!dialogueJsonObject)
                throw new Exception("Error en la obtencion de los dialogos del nivel del archivo .json de dialogos " + level);

            // Se obtiene la tensión inicial del nivle
            JSONObject initialTens = levelJsonObject.GetField("InitialTension");
            if(!initialTens)
                throw new Exception("No hay tensión definida en el nivel del archivo .json de dialogos " + level);
            initialTension = initialTens.floatValue;


            string character;
            string lastResponse;

            // Se recorre la lista de dialogos
            for (int i = 0; i < dialogueJsonObject.list.Count; ++i)
            {
                JSONObject dialogues = dialogueJsonObject.list[i];
                if(!dialogues)
                    throw new Exception("Error en la obtencion del dialogo " + i + " del archivo .json de dialogos " + level);

                // Se obtiene el nombre de los personajes con los que se interactuan
                foreach (var dialogue in dialogues)
                {
                    var auxCharacterObject = dialogue.GetField("character");
                    if (auxCharacterObject == null || !auxCharacterObject.isString)
                        throw new Exception("Error en la obtencion del nombre del personaje en el dialogo " + i + " del archivo .json de dialogos " + level);
                    string auxCharacter = auxCharacterObject.stringValue;

                    if (!characters.Contains(auxCharacter) && auxCharacter != "Yo")
                    {
                        // Se guardan los personajes en una lista
                        characters.Add(auxCharacter);
                    }
                }

                // Se obtiene el nombre del ultimo personaje con el que se interactua
                var characterObject = dialogues.list[dialogues.list.Count - 1].GetField("character");
                if (characterObject == null || !characterObject.isString)
                    throw new Exception("Error en la obtencion del nombre del ultimo personaje en el ultimo dialogo del archivo .json de dialogos " + level);
                character = characterObject.stringValue;
                // Se obtiene la respuesta que se le da al personaje
                var lastResponseObject = dialogues.list[dialogues.list.Count - 1].GetField("Responses");
                if (lastResponseObject == null || !lastResponseObject.isString)
                    throw new Exception("Error en la obtencion de la respuesta al ultimo personaje en el ultimo dialogo del archivo .json de dialogos " + level);
                lastResponse = lastResponseObject.stringValue;
                // Se obtiene el nombre del dialogo que se desbloquea al terminar la conversacion
                JSONObject unlockJSONObject = dialogues.list[dialogues.list.Count - 1].GetField("unlock");
                
                // Se guarda la informacion del dialogo en el diccionario
                dialogueDictionary.Add(dialogueJsonObject.keys[i], new DialogueInfo(character, lastResponse, unlockJSONObject?.stringValue));
            }
        }
    }
    // Metodo para leer un json de respuestas
    // @param level: nombre del nivel a leer
    private void LoadResponsesJson(string level)
    {
        responseDictionary = new Dictionary<string, List<ResponseInfo>>();

        using (StreamReader r = new StreamReader(level))
        {
            // Se lee el json
            string json = r.ReadToEnd();

            // Se crea un objeto json a partir del string leido
            JSONObject levelJsonObject = new JSONObject(json);
            if (!levelJsonObject)
                throw new Exception("Error en la carga del archivo .json de respuestas " + level);
            // Se obtiene el primer objeto json de la lista, que equivale a las respuestas del nivel
            JSONObject responseJsonObject = levelJsonObject.list[0];
            if (!responseJsonObject)
                throw new Exception("Error en la obtencion de las respuestas del nivel del archivo .json de respuestas " + level);

            // Se recorre la lista de respuestas
            for (int i = 0; i < responseJsonObject.list.Count; ++i)
            {
                // Se obtiene el nombre de la respuesta y se guarda en el diccionario
                responseDictionary.Add(responseJsonObject.keys[i], new List<ResponseInfo>());
                for (int j = 0; j < responseJsonObject.list[i].count; ++j)
                {
                    // Se obtiene el nombre del dialogo
                    JSONObject dialogues = responseJsonObject.list[i];
                    if (!dialogues)
                        throw new Exception("Error en la obtencion de la respuesta " + i + " del archivo .json de respuestas " + level);
                    // Se obtiene el parametro que indica si la respuesta es seleccionable
                    JSONObject interactable = dialogues.list[j].GetField("interactable");
                    // En caso de que no lo sea, se salta la respuesta
                    if (interactable != null && !interactable.boolValue) continue;

                    // Se obtiene el nombre del siguiente dialogo
                    var nextDialogueGroupObject = dialogues.list[j].GetField("nextDialogueGroup");
                    if (nextDialogueGroupObject == null || !nextDialogueGroupObject.isString)
                        throw new Exception("Error en la obtencion del siguiente dialogo a una respuesta del archivo .json de respuestas " + level);
                    string nextDialogueGroup = nextDialogueGroupObject.stringValue;
                    // Se obtiene la tension que se modifica al seleccionar esa respuesta
                    var tensionObject = dialogues.list[j].GetField("tension");
                    if (tensionObject == null || !tensionObject.isInteger)
                        throw new Exception("Error en la obtencion de la tensión de una respuesta del archivo .json de respuestas " + level);
                    int tension = tensionObject.intValue;

                    // Se guarda la informacion de la respuesta en el diccionario
                    responseDictionary[responseJsonObject.keys[i]].Add(new ResponseInfo(nextDialogueGroup, tension, j));
                }
            }
        }
    }

    // Metodo recursivo para recorrer el arbol de dialogos
    // @param firstDialogueName: nombre del primer dialogo a recorrer
    // @param responseIDs: lista de IDs de respuestas escogidas
    // @param processedResponses: lista de respuestas que ya han sido procesadas
    // @param characterOrder: lista de personajes que se han recorrido en orden temporal
    // @param accumulativeTension: tension acumulada hasta el momento
    // @param currCharacter: personaje con el que se interactua en ese momento
    private void Pathfinder(string firstDialogueName, List<int> responseIDs, HashSet<string> processedResponses,
        List<string> characterOrder, float accumulativeTension, int currCharacter)
    {
        // Se obtiene el dialogo actual
        DialogueInfo currDialogue = dialogueDictionary[firstDialogueName];

        // Si no se desbloquea ningun dialogo y no se puede responder al personaje, se termina la ruta
        if (currDialogue.unlock == null && currDialogue.response == null)
        {
            return;
        }

        // Se ha llegado al final de la ruta
        if (currDialogue.response == "EndLevel")
        {
            // Se guarda la ruta en la lista 
            if(accumulativeTension <= maxTension)
                Routes.Add(new RouteInfo(responseIDs, characterOrder, accumulativeTension));
            return;
        }

        // Se ha terminado de hablar con el personaje y se desbloquea uno nuevo
        if (currDialogue.unlock != null)
        {
            // Si no se habia interactuado antes con el personaje, se guarda en la lista
            if (!characterOrder.Contains(currDialogue.unlock))
                characterOrder.Add(currDialogue.unlock);
            // Comienza a recorrer el dialogo que se desbloquea
            Pathfinder(currDialogue.unlock, responseIDs, processedResponses, characterOrder, accumulativeTension, currCharacter);
        }
        // Terminada ruta de personaje actual
        else if (currDialogue.response == "None")
        {
            // Se pasa a hablar con el siguiente personaje con el que no se ha interactuado
            currCharacter++;
            while (!dialogueDictionary.ContainsKey("Inicial" + characters[currCharacter]) && currCharacter < characters.Count)
                currCharacter++;
            // Si no se habia interactuado antes con ese personaje, se aniade a la lista
            if (!characterOrder.Contains("Inicial" + characters[currCharacter]))
                characterOrder.Add("Inicial" + characters[currCharacter]);
            // Comienza a recorrer el dialogo inicial del personaje
            Pathfinder("Inicial" + characters[currCharacter], responseIDs, processedResponses, characterOrder, accumulativeTension, currCharacter);
        }
        else
        {
            // Se recorren las posibles respuestas de ese dialogo
            for (int i = 0; i < responseDictionary[currDialogue.response].Count; ++i)
            {
                // Se obtiene la respuesta seleccionada
                ResponseInfo responses = responseDictionary[currDialogue.response][i];
                // Si ya se habia seleccionado esa respuesta se salta
                if (processedResponses.Contains(currDialogue.response + i.ToString()))
                    continue;
                else
                {
                    // Set que contiene las respuestas seleccionadas hasta el momento
                    HashSet<string> tempResponses = new HashSet<string>(processedResponses)
                    {
                        currDialogue.response + i.ToString()
                    };
                    List<int> tempResponseIDs = new List<int>(responseIDs)
                    {
                        responses.ResponseID
                    };

                    // Se recorre el arbol seleccionando esa respuesta
                    Pathfinder(responses.NextDialogueGroup, tempResponseIDs, tempResponses, characterOrder, accumulativeTension + responses.Tension, currCharacter);
                }
            }
        }
    }
}
