using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseController : MonoBehaviour
{
    [SerializeField]
    ResponseInteractor[] interactors;
    
    public void SetResponses(ResponseGroup responseGroup) {
        int i = 0;
        foreach (var interactor in interactors) {
            interactor.SetResponse(responseGroup.responses[i].text);
            ++i;
        }
    }

    // Start is called before the first frame update
    void Start() {
        int i = 0;
        foreach (var interactor in interactors) {
            interactor.SetId(i);
            ++i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
