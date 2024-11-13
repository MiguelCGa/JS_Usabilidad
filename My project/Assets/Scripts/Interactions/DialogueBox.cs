using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    [SerializeField]
    private GameObject nameBox;

    [SerializeField]
    private TMP_Text nameText;

    [SerializeField]
    private GameObject dialogueBox;

    [SerializeField]
    private TMP_Text dialogueText;

    [SerializeField]
    private GameObject arrow;

    private string textToShow;
    private bool showText;
    private Coroutine typingCoroutine;
    private bool arrowControl;
    private float arrowTimer;

    public void dialogue(string text, string name)
    {
        textToShow = text;
        showText = true;

        nameText.enabled = true;
        nameBox.SetActive(true);
        dialogueBox.SetActive(true);
        dialogueText.enabled = true;

        // por si ya había alguna activa se para
        if (typingCoroutine != null) StopCoroutine(typingCoroutine); 

        typingCoroutine = StartCoroutine(TypeText(textToShow, name));
    }

    // Corrutina para escribir el texto caracter a caracter
    private IEnumerator TypeText(string text, string name)
    {
        nameText.text = name;
        dialogueText.text = "";
        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.05f);// velocidad de aparición de los caracteres
        }
        showText = false;
        arrow.SetActive(true);
        arrowControl = true;
        arrowTimer = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        nameText.enabled = false;
        nameBox.SetActive(false);
        dialogueBox.SetActive(false);
        dialogueText.enabled = false;
        showText = false;
        arrow.SetActive(false);
        arrowControl = false;
        arrowTimer = 0;

        // Activar el auto size
        dialogueText.enableAutoSizing = true;

        // Definir el tamaño mínimo y máximo de la fuente
        dialogueText.fontSizeMin = 10;
        dialogueText.fontSizeMax = 36;

        ConversationManager.Instance.SetDialogBox(this);
    }

    // Update is called once per frame
    void Update()
    {
        arrowTimer += Time.deltaTime;
        if (arrowControl && arrowTimer >= 0.4)
        {
            arrow.SetActive(!arrow.gameObject.activeSelf);
            arrowTimer = 0;
        }
    }

    public bool Next() {
        if (!showText) {
            nameText.enabled = false;
            dialogueText.enabled = false;
            nameBox.SetActive(false);
            dialogueBox.SetActive(false);
            arrow.SetActive(false);
            arrowControl = false;
        }
        else if (typingCoroutine != null) {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
            dialogueText.text = textToShow; // texto completo
            showText = false;
            arrow.SetActive(true);
            arrowControl = true;
            arrowTimer = 0;
        }
        else return false;
        return true;
    }
}
