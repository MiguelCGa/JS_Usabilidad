using Defective.JSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorHandler 
{
    public JSONObject ProccessError(Exception e, BotJSONParser.RouteInfo? routeInfo = null) {
        JSONObject globalErrorData = new JSONObject();

        globalErrorData.AddField("ErrorData", e.Message);

        if (routeInfo != null)
        {
            var data = routeInfo.Value;
            JSONObject routeData = new JSONObject();

            routeData.AddField("CharacterHistory", listToJSON(data.Characters));
            routeData.AddField("ResponseHistory", listToJSON(data.Responses));
            routeData.AddField("Tension", data.TotalTension);

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
