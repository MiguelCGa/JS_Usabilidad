using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IMainActions
{
    public struct MousePos
    {
        public float x, y;
    }

    Controls controls;
    public static InputReader Instance { get; private set; }
    public Action onUse { get; set; }

    public MousePos mousePos;

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
            mousePos.x = Mouse.current.position.ReadValue().x;
            mousePos.y = Mouse.current.position.ReadValue().y;
        }
    }
}
