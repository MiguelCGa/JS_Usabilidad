using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuNavigationManager : MonoBehaviour
{
    [SerializeField]
    MenuController[] initialMenus = null;

    static int initialMenuIndex = 0;

    MenuController currentMenu = null;
    Stack<MenuController> previous = new Stack<MenuController>();
    Stack<MenuController> next = new Stack<MenuController>();

    public static void SetIntialMenuIndex(int index) { 
        initialMenuIndex = index;
    }

    private void Start() {
        if (initialMenus != null) {
            for (int i = 0; i < initialMenuIndex + 1; i++) {
                if (initialMenuIndex < initialMenus.Length) 
                    GoTo(initialMenus[i]);
            }
        }
    }

    public void GoTo(MenuController menu) {
        if (currentMenu != null) {
            currentMenu.Exit();
            previous.Push(currentMenu);
            next.Clear();
        }
        currentMenu = menu;
        currentMenu.Enter();
    }


    public void GoTo(int index)
    { 
        MenuController menu = initialMenus[index];
        if (currentMenu != null)
        {
            currentMenu.Exit();
            previous.Push(currentMenu);
            next.Clear();
        }
        currentMenu = menu;
        currentMenu.Enter();
    }

    public void GoBack() {
        if (currentMenu != null) {
             currentMenu.Exit();
            next.Push(currentMenu);
        }
        currentMenu = previous.Pop();
        currentMenu.Enter();
    }
    public void GoForward() {
        if (currentMenu != null) {
            currentMenu.Exit();
            previous.Push(currentMenu);
        }
        currentMenu = next.Pop();
        currentMenu.Enter();
    }
}
