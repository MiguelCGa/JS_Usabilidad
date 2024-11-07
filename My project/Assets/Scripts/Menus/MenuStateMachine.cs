using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuStateMachine : MonoBehaviour
{
    [SerializeField]
    List<MenuState> states;

    Dictionary<MenuStateID, MenuState> statesDict;
    MenuState currentState;

    private void Start() {
        currentState = states.First();
        foreach (MenuState state in states) { 
            statesDict.Add(state.getID(), state);
        }
        currentState.Enter();
    }

    public void ChangeState(MenuStateID into) { 
        currentState.Exit();
        currentState = statesDict[into];
        currentState.Enter();
    }
}
