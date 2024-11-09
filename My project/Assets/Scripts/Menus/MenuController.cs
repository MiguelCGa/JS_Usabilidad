using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnEnter, OnExit;

    public void Enter() {
        OnEnter.Invoke(); 
    }
    public void Exit() {
        OnExit.Invoke();
    }
}
