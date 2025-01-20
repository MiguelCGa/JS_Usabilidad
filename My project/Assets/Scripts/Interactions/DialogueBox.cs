using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DialogueBox : MonoBehaviour
{
    [System.Serializable]
    public class CharacterSprite
    {
        public string name;
        public Sprite sprite;
    }

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

    [SerializeField]
    private Image image;

    [SerializeField]
    private GameObject fondo;

    [SerializeField]
    private GameObject fondoOscurecer;

    [SerializeField]
    private List<CharacterSprite> sprites;

    private string textToShow;
    private bool textComplete;
    private Coroutine typingCoroutine;
    private bool arrowControl;
    private float arrowTimer;


    private string preparedText = null;
    private string preparedName = null;

    public void prepareDialogue(string text, string name) {
        preparedText = text;
        preparedName = name;
    }

    public void dialogue(string text, string name)
    {
        textToShow = text;
        textComplete = false;

        nameText.enabled = true;
        nameBox.SetActive(true);
        dialogueBox.SetActive(true);
        dialogueText.enabled = true;
        image.enabled = true;
        fondo.SetActive(true);
        fondoOscurecer.SetActive(true);
        image.preserveAspect = true;
        arrow.SetActive(false);
        arrowControl = false;

        dialogueText.text = textToShow;
        dialogueText.enableAutoSizing = false;
        dialogueText.text = "";

        ChangeImage(name);

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
        textComplete = true;
        arrow.SetActive(true);
        arrowControl = true;
        arrowTimer = 0;
    }

    private void ChangeImage(string name)
    {
        var img = sprites.Find(s => s.name == name);
        if (img != null && image != null)
        {
            image.sprite = img.sprite;
            image.enabled = true;
        }
        else if (image != null)
        {
            image.enabled = false;
            fondo.SetActive(false);
        }
    }

    private void Awake() {
        ConversationManager.Instance.SetDialogBox(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        nameText.enabled = false;
        nameBox.SetActive(false);
        dialogueBox.SetActive(false);
        dialogueText.enabled = false;
        image.enabled = false;
        fondo.SetActive(false);
        fondoOscurecer.SetActive(false);
        textComplete = false;
        arrow.SetActive(false);
        arrowControl = false;
        arrowTimer = 0;

        // Activar el auto size
        dialogueText.enableAutoSizing = true;

        // Definir el tamaño mínimo y máximo de la fuente
        dialogueText.fontSizeMin = 10;
        dialogueText.fontSizeMax = 36;

        if (preparedText != null) {
            dialogue(preparedText, preparedName);
        }
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
        if (textComplete)
            return false;
        if (typingCoroutine != null) {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
            dialogueText.text = textToShow; // texto completo
            textComplete = true;
            arrow.SetActive(true);
            arrowControl = true;
            arrowTimer = 0;
            return true;
        }
        return false;
    }
}
