using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResponseInteractor : MonoBehaviour
{
    [SerializeField]
    private TMP_Text myText = null;

    private int id;
    public void SetId(int responseId) {
        id = responseId;
    }
    public void SelectResponse() {
        ConversationManager.Instance.SelectResponse(id);
    }

    public void SetResponse(string response) {
        if (myText != null)
            myText.text = response;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
