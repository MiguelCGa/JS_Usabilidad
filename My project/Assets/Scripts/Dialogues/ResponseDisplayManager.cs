using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseDisplayManager : MonoBehaviour
{
    private ResponseGroup responses;

    [SerializeField]
    private Canvas twoResponses;
    [SerializeField]
    private Canvas threeResponses;
    [SerializeField]
    private Canvas fourResponses;

    void ActivateTwoResponses() {

    }
    void ActivateThreeResponses() {

    }
    void ActivateFourResponses() {

    }

    public void SetResponses(ResponseGroup responseGroup) {
        responses = responseGroup;
        switch (responseGroup.responses.Length) {
            case 2:
                ActivateTwoResponses();
                break;
            case 3:
                ActivateThreeResponses();
                break;
            case 4:
                ActivateFourResponses();
                break;
        }
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
