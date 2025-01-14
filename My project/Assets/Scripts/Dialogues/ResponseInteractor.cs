using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResponseInteractor : MonoBehaviour
{
    [SerializeField]
    private TMP_Text myText = null;

    private Button button = null;

    private int id;

    public void SetId(int responseId) {
        id = responseId;
    }
    public void SelectResponse() {
        ConversationManager.Instance.SelectResponse(id);
        button.interactable = false;
    }

    public void SetResponse(string response) {
        if (myText != null)
            myText.text = response;
    }

    public void SetInteractable(bool interactable) {
        button.interactable = interactable;
    }

    public void ResetInteraction() {
        if (button != null)
            button.interactable = true;
    }

    // Start is called before the first frame update
    void Start() {
        button = GetComponent<Button>();
    }
}
