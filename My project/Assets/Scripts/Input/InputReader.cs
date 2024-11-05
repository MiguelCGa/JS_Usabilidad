using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IMainActions
{
    Controls controls;
    public static InputReader Instance { get; private set; }
    public Action onUse { get; set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        controls = new Controls();
        controls.Main.Enable();
        controls.Main.SetCallbacks(this);
    }

    public void OnUse(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            onUse?.Invoke();
        }
    }
}
