using Defective.JSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorHandler 
{
    public JSONObject ProccessError(Exception e, BotJSONParser.RouteInfo routeInfo) {
        JSONObject globalErrorData = new JSONObject();

        globalErrorData.AddField("ErrorData", e.Message);
        
        JSONObject routeData = new JSONObject();

        routeData.AddField("CharacterHistory", listToJSON(routeInfo.Characters));
        routeData.AddField("ResponseHistory", listToJSON(routeInfo.Responses));
        routeData.AddField("Tension", routeInfo.TotalTension);

        globalErrorData.AddField("RouteInfo", routeData);

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
