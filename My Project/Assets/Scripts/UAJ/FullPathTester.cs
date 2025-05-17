using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullPathTester : GameListener
{
    public static FullPathTester Instance { get; private set; }

    private HashSet<int> unlockedLevels;

    private void Init()
    {

        if (Instance == null)
        {
            Instance = this;
            EventQueue.Instance().AddListener(Instance);

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

    public void StartPlaying()
    {
        Debug.Log("Yipeeee");
        InputCommands.Instance.StartGame();
        unlockedLevels.Add(0);
    }

    public override void RecieveEvent(GameEvent evt)  
    {
        if (evt.GetEventType()== EventType.GameStart)
        {

            StartPlaying();
            EventQueue.Instance().HandleEvent();
        }
    }

}
