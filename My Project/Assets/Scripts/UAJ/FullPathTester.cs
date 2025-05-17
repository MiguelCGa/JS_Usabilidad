using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullPathTester : GameListener
{
    [SerializeField]
    float checkRouteProportion; //100% == all paths checked  
    public static FullPathTester Instance { get; private set; }

    private  List<BotJSONParser.RouteInfo> currentLevelRoutes;
    private HashSet<int> unlockedLevels;
    int currentLevel = 0;

    private void Init()
    {

        if (Instance == null)
        {
            Instance = this;
            EventQueue.Instance().AddListener(Instance);
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
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartPlaying()
    {
        
        unlockedLevels.Add(1);
        currentLevel = 1;
        InputCommands.Instance.StartGame();
    }

    private void SelectLevel(int l)
    {

        InputCommands.Instance.SelectLevel(l);
    
    }
    private void StartLevel(int l )
    {

        currentLevelRoutes = BotJSONParser.Instance().ParseLevel(GameManager.Instance.GetNameOnIndex(l));
        InputCommands.Instance.NextDialogue();

    
    }
    public override void RecieveEvent(GameEvent evt)  
    {

        switch (evt.GetEventType()) {
            case EventType.GameStart:
                Debug.Log("GameStart");

                StartPlaying();
                EventQueue.Instance().HandleEvent();
                break;
            case EventType.LevelsMenu:
                Debug.Log("LevelsMenu");
                SelectLevel(currentLevel);
                EventQueue.Instance().HandleEvent();
                break;
            case EventType.LevelStart:
                Debug.Log("LevelStart" + evt.GetParameter<int>().ToString());
                StartLevel(1);
                break;
            case EventType.ConversationStarted:
                Debug.Log("ConversationStarted" + evt.GetParameter<string>().ToString());
                InputCommands.Instance.NextDialogue();
                break;
            case EventType.FinalTension: //chequear si se corresponde con la tension calculada por la ruta
                break;
        }

            
        
    }

}
