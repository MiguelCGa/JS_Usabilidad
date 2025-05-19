using Defective.JSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorHandler 
{
    public JSONObject ProccessError(Exception e, BotJSONParser.DialogueInfo? dialogueInfo, BotJSONParser.ResponseInfo? responseInfo, BotJSONParser.RouteInfo? routeInfo) {
        JSONObject globalErrorData = new JSONObject();

        globalErrorData.AddField("ErrorData", e.Message);


        if (dialogueInfo != null)
        {
            var currentDialogue = dialogueInfo.Value;

            JSONObject dialogueData = new JSONObject();

            dialogueData.AddField("CharacterInteracting", currentDialogue.character);
            dialogueData.AddField("Response", currentDialogue.response);
            dialogueData.AddField("Unlock", currentDialogue.unlock);

            globalErrorData.AddField("DialogueInfo", dialogueData);
        }

        if (responseInfo != null)
        {
            var currentResponse = responseInfo.Value;

            JSONObject responseData = new JSONObject();

            responseData.AddField("NextDialogueGroup", currentResponse.NextDialogueGroup);
            responseData.AddField("Tension", currentResponse.Tension);
            responseData.AddField("ResponseID", currentResponse.ResponseID);

            globalErrorData.AddField("ResponseInfo", responseData);

        }

        if (routeInfo != null)
        {
            var currentRoute = routeInfo.Value;

            JSONObject routeData = new JSONObject();

            routeData.AddField("CharacterHistory", listToJSON(currentRoute.Characters));
            routeData.AddField("ResponseHistory", listToJSON(currentRoute.Responses));
            routeData.AddField("Tension", currentRoute.TotalTension);

            globalErrorData.AddField("RouteInfo", routeData);

        }

        return globalErrorData;
    }

    private JSONObject listToJSON<T>(List<T> list)
    {
        JSONObject jsonObject = new JSONObject();

        foreach (T item in list)
        {
            jsonObject.Add(item.ToString());
        }

        return jsonObject;
    }
}
