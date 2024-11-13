using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResponseInteractor : MonoBehaviour
{
    [SerializeField]
    private Text myText;

    private int id;
    public void SetId(int responseId) {
        id = responseId;
    }
    public void SelectResponse() {
        ConversationManager.Instance.SelectResponse(id);
    }

    public void SetResponse(string response) {
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
