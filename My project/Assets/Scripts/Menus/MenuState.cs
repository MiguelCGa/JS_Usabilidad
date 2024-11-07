using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuState : MonoBehaviour
{
    [SerializeField]
    private MenuStateID menuStateID;
    [SerializeField]
    private UnityEvent OnEnter, OnExit;

    public MenuStateID getID() { 
        return menuStateID;
    }
    public void Enter() {
        OnEnter.Invoke(); 
    }
    public void Exit() {
        OnExit.Invoke();
    }
}
