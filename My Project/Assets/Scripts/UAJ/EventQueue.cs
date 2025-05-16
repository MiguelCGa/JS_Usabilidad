using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct GameEvent
{
    
}
enum EventType { GameStart, LevelStart, StartingTension, ModifiedTension, FinalTension, SelectedResponse, ConversationStarted, ResponseStarted }
public class EventQueue 
{
   private Queue<GameEvent> queue;
   public void Init() {

   }
}
