using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    [SerializeField]
    private GameObject dialogueBox;

    [SerializeField]
    private Text dialogueText;  

    private string textToShow;
    private bool showText;
    private Coroutine typingCoroutine;

    public void dialogue(string text)
    {
        textToShow = text;
        showText = true;

        dialogueBox.SetActive(true);

        // por si ya había alguna activa se para
        if (typingCoroutine != null) StopCoroutine(typingCoroutine); 

        typingCoroutine = StartCoroutine(TypeText(textToShow));
    }

    // Corrutina para escribir el texto caracter a caracter
    private IEnumerator TypeText(string text)
    {
        dialogueText.text = "";
        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.05f);// velocidad de aparición de los caracteres
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        dialogueBox.SetActive(false);
        showText = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Si se hace click se completa el texto (y para la corrutina)
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("space"))
        {
            if (!showText)
            {
                dialogueText.enabled = false;
                dialogueBox.SetActive(false);
            }
            else if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
                dialogueText.text = textToShow; // texto completo
                showText = false;
            }
        }
    }
}
