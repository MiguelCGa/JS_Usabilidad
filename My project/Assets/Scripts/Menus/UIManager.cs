using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
