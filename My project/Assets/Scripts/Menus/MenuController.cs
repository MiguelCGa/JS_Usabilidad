using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private MenuControllerID id;
    [SerializeField]
    private UnityEvent OnEnter, OnExit;

    public MenuControllerID getID() { 
        return id;
    }
    public void Enter() {
        OnEnter.Invoke(); 
    }
    public void Exit() {
        OnExit.Invoke();
    }
}
