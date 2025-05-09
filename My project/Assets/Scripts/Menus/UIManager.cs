using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void ShowMenu(MenuController menu) {
        if (menu == null)
            return;
        menu.gameObject.SetActive(true);
    }

    public void HideMenu(MenuController menu) {
        if (menu == null)
            return;
        menu.gameObject.SetActive(false);
    }
}
