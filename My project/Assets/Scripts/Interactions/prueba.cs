using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prueba : MonoBehaviour
{
    private DialogueBox dialogue;
    // Start is called before the first frame update
    void Start()
    {
        dialogue = GetComponent<DialogueBox>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            dialogue.dialogue("Juegos seriossssssss aaaaaa", "Sofía");
        }
    }
}
