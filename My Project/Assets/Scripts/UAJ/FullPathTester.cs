using Defective.JSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class FullPathTester : MonoBehaviour
{
    private GameObject input;
    [SerializeField]
    float checkRouteProportion; //100% == all paths checked  

    [SerializeField]
    [Range(0.0f, 10.0f)]
    float maxTension = 10.0f;
    public static FullPathTester Instance { get; private set; }

    private ErrorHandler errorHandler = new ErrorHandler();

    private List<BotJSONParser.RouteInfo> currentLevelRoutes;
    private HashSet<int> unlockedLevels;
    private bool inConversation = false;
    private bool levelLoaded = false;
    private int currentLevel = 1;
    private int currentRoute = 0;
    private int currentCharacter = 0;
    private int currentResponse = 0;

    private float time = 0;

    private void Init()
    {
        if (Instance == null)
        {
            Instance = this;
            unlockedLevels = new HashSet<int>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Awake()
    {
        Init();
    }
    private void Start()
    {
        input = GameObject.FindGameObjectWithTag("Input");
        input.SetActive(false);
        ConversationManager.Instance.DisableInput();
    }

    void Update()
    {
        try
        {
            if (EventQueue.Instance().HasEvents())
            {
                HandleEvent(EventQueue.Instance().HandleEvent());
            }
            if (inConversation)
            {
                InputCommands.Instance.NextDialogue();
            }
        }
        catch(Exception e)
        {
            JSONObject error = new JSONObject();
            if (currentLevelRoutes!= null)
                errorHandler.ProccessError(e, GameManager.Instance.GetNameOnIndex(currentLevel), currentLevelRoutes[currentLevel]);
            if (currentLevelRoutes == null)
                errorHandler.ProccessError(e);
        }
        time += Time.deltaTime;
    }

    private void StartPlaying()
    {
        unlockedLevels.Add(1);
        InputCommands.Instance.StartGame();
    }

    private void SelectLevel()
    {
        InputCommands.Instance.SelectLevel(currentLevel);
    }
    private void StartLevel()
    {
        if (!levelLoaded)
        {
            input = GameObject.FindGameObjectWithTag("Input");
            input.SetActive(false);
            currentLevelRoutes = BotJSONParser.Instance().ParseLevel(GameManager.Instance.GetNameOnIndex(currentLevel), maxTension);

            levelLoaded = true;
        }
    }
    private bool StartConversation(string character)
    {
        return InputCommands.Instance.InteractWithCharacter(character);
    }

    private void HandleEvent(GameEvent evt)
    {
        switch (evt.GetEventType())
        {
            case EventType.GameStart:
                Debug.Log("GameStart");
                StartPlaying();
                break;
            case EventType.LevelsMenu:
                Debug.Log("LevelsMenu");
                if (currentLevel < SceneManager.sceneCountInBuildSettings)
                    SelectLevel();
                else 
                    Application.Quit();
                break;
            case EventType.LevelStart:
                Debug.Log("LevelStart" + evt.GetParameter<int>().ToString());
                StartLevel();
                break;
            case EventType.ConversationStarted:
                Debug.Log("ConversationStarted" + evt.GetParameter<string>());
                inConversation = true;
                break;
            case EventType.OnLevelLoaded:
                Debug.Log("OnLevelLoaded");
                currentCharacter = 0;
                currentResponse = 0;
                inConversation = true;
                break;
            case EventType.ResponseStarted:
                Debug.Log("ResponseStarted" + evt.GetParameter<string>());
                inConversation = false;
                if (currentResponse < currentLevelRoutes[currentRoute].Responses.Count)
                    if (!InputCommands.Instance.SelectOption(currentLevelRoutes[currentRoute].Responses[currentResponse++]))
                        throw new Exception("Error al seleccionar la respuesta: " + currentLevelRoutes[currentRoute].Responses[currentResponse - 1]);
                break;
            case EventType.SelectedResponse:
                Debug.Log("SelectedResponse: " + evt.GetParameter<int>());
                inConversation = true;
                break;
            case EventType.ConversationEnded:
                Debug.Log("ConversationEnded");
                inConversation = false;
                if (currentCharacter < currentLevelRoutes[currentRoute].Characters.Count)
                    if (!StartConversation(currentLevelRoutes[currentRoute].Characters[currentCharacter++]))
                        throw new Exception("Error en el personaje: " + currentLevelRoutes[currentRoute].Characters[currentCharacter - 1]);
                break;
            case EventType.LevelConclusion:
                Debug.Log("LevelConclusion");
                inConversation = true;
                break;
            case EventType.LevelEnd:
                Debug.Log("LevelEnd");
                inConversation = true;
                if ((currentRoute >= currentLevelRoutes.Count - 1) ||
                    (currentRoute >= (currentLevelRoutes.Count - 1) * checkRouteProportion / 100))
                {
                    Debug.Log("Tiempo de rutas de nivel " + currentLevel + ": " + time);
                    levelLoaded = false;
                    currentLevel++;
                    currentRoute = 0;
                    time = 0;
                }
                else
                {
                    currentRoute++;
                }
                break;
        }
    }
}
