using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseInteractor : MonoBehaviour
{
    private int id;
    public void SetId(int responseId) {
        id = responseId;
    }
    public void SelectResponse() {
        ConversationManager.Instance.SelectResponse(id);
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
