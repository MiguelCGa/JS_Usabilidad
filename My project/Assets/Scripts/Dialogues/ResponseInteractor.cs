using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResponseInteractor : MonoBehaviour
{
    [SerializeField]
    private TMP_Text myText = null;

    private string text = "";
    private bool isInteractable = true;

    private Button button = null;

    private int id;

    public void SetId(int responseId) {
        id = responseId;
    }
    public void SelectResponse() {
        ConversationManager.Instance.SelectResponse(id);
        button.interactable = false;
    }

    private void OnEnable() {
        if (myText != null) 
            myText.text = text;
        if (button != null)
            button.interactable = isInteractable;
    }

    public void SetResponse(string response) {
        text = response;
        if (myText != null)
            myText.text = text;
    }

    public void SetInteractable(bool interactable) {
        isInteractable = interactable;
    }

    public void ResetInteraction() {
        if (button != null)
            button.interactable = true;
    }

    // Start is called before the first frame update
    void Start() {
        button = GetComponent<Button>();
        if (myText != null)
        {
            myText.text = text;
            myText.enableAutoSizing = true;
        }
        button.interactable = isInteractable;
    }
}
