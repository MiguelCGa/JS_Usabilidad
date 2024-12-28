using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseDisplayManager : MonoBehaviour
{
    private ResponseGroup responses = null;

    [SerializeField]
    private ResponseController twoResponses;
    [SerializeField]
    private ResponseController threeResponses;
    [SerializeField]
    private ResponseController fourResponses;
    [SerializeField]
    private ResponseController emotionsResponses;

    void ActivateResponses(ResponseGroup newResponses, ResponseController controller) {
        controller.gameObject.SetActive(true);
        if (newResponses != responses) {
            controller.ResetResponsesInteraction();
            responses = newResponses;
            controller.SetResponses(responses);
        }
    }

    public void SetResponses(ResponseGroup responseGroup, bool emotions) {
        if (emotions) {
            ActivateResponses(responseGroup, emotionsResponses);
            return;
        }

        switch (responseGroup.responses.Length) {
            case 2:
                ActivateResponses(responseGroup, twoResponses);
                break;
            case 3:
                ActivateResponses(responseGroup, threeResponses);
                break;
            case 4:
                ActivateResponses(responseGroup, fourResponses);
                break;
        }
    }

    public void HideResponses() {
        twoResponses.gameObject.SetActive(false);
        threeResponses.gameObject.SetActive(false);
        fourResponses.gameObject.SetActive(false);
        emotionsResponses.gameObject.SetActive(false);
    }
}
