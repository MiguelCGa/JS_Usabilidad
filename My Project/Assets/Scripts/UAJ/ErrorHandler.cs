using Defective.JSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ErrorHandler 
{
    //Creamos y escribimos un archivo JSON con los datos enviados
    public void ProccessError(Exception e, string level = null, BotJSONParser.RouteInfo? routeInfo = null) {
        JSONObject globalErrorData = new JSONObject();

        globalErrorData.AddField("ErrorData", e.Message);

        if (level != null)
            globalErrorData.AddField("Level", level);

        if (routeInfo != null)
        {
            var data = routeInfo.Value;
            JSONObject routeData = new JSONObject();

            routeData.AddField("CharacterHistory", listToJSON(data.Characters));
            routeData.AddField("ResponseHistory", listToJSON(data.Responses));
            routeData.AddField("Tension", data.TotalTension);

            globalErrorData.AddField("RouteInfo", routeData);
        }

        var file = File.CreateText(Application.persistentDataPath + "/" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + "error.json");
        file.Write(globalErrorData.ToString());
        file.Close();

        Application.Quit();
    }

    //Parseo de lista a un objeto de tipo JSON
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
