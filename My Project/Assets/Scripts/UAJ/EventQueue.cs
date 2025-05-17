using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;

public   class GameListener :MonoBehaviour
{

    public virtual void RecieveEvent(GameEvent evt)
    { 

    }
      

}
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

    public EventType GetEventType() { return type; }
    public T GetParameter<T>()
    {
        if (parameter is T value) { return (T)parameter; }
        throw new System.InvalidCastException();
    }
}

public enum EventType
{
    GameStart, LevelStart, StartingTension, ModifiedTension, FinalTension, SelectedResponse,
    ConversationStarted, ConversationEnded, ResponseStarted, CharacterUnlocked
}

public class EventQueue
{
    private static EventQueue instance;
    private Queue<GameEvent> queue;
    private HashSet<GameListener> listeners;
    public static EventQueue Instance()
    {
        if (instance == null)
        {
            instance = new EventQueue();
        }

        return instance;
    }
    private EventQueue()
    {
        queue = new Queue<GameEvent>();
        listeners = new HashSet<GameListener>();
    }

    public void AddEvent(GameEvent gEvent)
    {
        queue.Enqueue(gEvent);
        foreach (GameListener lt in listeners)
        {

            lt.RecieveEvent(gEvent);
        }
    }

    public GameEvent HandleEvent()
    {
        return queue.Dequeue();
    }

    public void AddListener(GameListener listener)
    {
        listeners.Add(listener);
    }
}
