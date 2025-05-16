using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;


public struct GameEvent
{
    EventType type;
    object parameter;
    public GameEvent(EventType t, string s)
    {
        type = t;
        parameter = s;
    }
    public GameEvent(EventType t, int i)
    {
        type = t;
        parameter = i;
    }
    public GameEvent(EventType t, float f)
    {
        type = t;
        parameter = f;
    }
    public GameEvent(EventType t)
    {
        type = t;
        parameter = null;
    }
}
public enum EventType { GameStart, LevelStart, StartingTension, ModifiedTension, FinalTension, SelectedResponse, 
    ConversationStarted, ConversationEnded, ResponseStarted,  CharacterUnlocked}

public class EventQueue 
{
   public static EventQueue Instance { get; private set; }
   private Queue<GameEvent> queue;
   public static void Init() {
        if (Instance == null)
        {
            Instance = new EventQueue();
        } 
   }

    public void AddEvent(GameEvent gEvent)
    {
        queue.Enqueue(gEvent);
    }

    public GameEvent HandleEvent()
    {
        return queue.Dequeue();
    }
}
