using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

public class AirtableManager : MonoBehaviour
{

    [Header("Scripts")]
    // Reference to the AirtableSceneController script
    public AirtableSceneController airtableSceneController;     //project specific

    [Header("Airtable Elements")]                           //neede for all projects
    // Airtable API endpoint and authentication details
    public string airtableEndpoint = "https://api.airtable.com/v0/";
    public string accessToken = "YOUR_ACCESS_TOKEN";
    public string baseId = "YOUR_BASE_ID";
    public string tableName = "YOUR_TABLE_NAME";
    private string dataToParse;

    [Header("Data For Airtable")]                   //different variables per project
    // Data fields for recording information
    public string dateTime;
    public string Question1;
    public string Question2;
    public string Question3;
    public string Question4;
    public string Question5;
    public string Question6;
    public string Question7;
    public string Question8;
    //public string Question9;

    // Method to create a new record in Airtable
    public void CreateRecord()
    {
        // Reset dataToLoad if it is not null           //used to check before reading from airtable
        //if (dataToLoad != null)         
        //{
        //    dataToLoad = null;
        //}

        // Get the current date and time
        dateTime = DateTime.Now.ToString("dd.MM.yyyy HH.mm");

        // Create the URL for the API request
        string url = airtableEndpoint + baseId + "/" + tableName;       //points to the table

        // Create JSON data for the API request
        string jsonFields = "{\"fields\": {" +
                                    "\"dateTime\":\"" + dateTime + "\", " +
                                    "\"Question1\":\"" + Question1 + "\", " +
                                    "\"Question2\":\"" + Question2 + "\", " +
                                    "\"Question3\":\"" + Question3 + "\", " +
                                    "\"Question4\":\"" + Question4 + "\", " +
                                    "\"Question5\":\"" + Question5 + "\", " +
                                    "\"Question6\":\"" + Question6 + "\", " +
                                    "\"Question7\":\"" + Question7 + "\", " +
                                    "\"Question8\":\"" + Question8 + "\", " +
                                    "\"Question9\":\"" + "Question9" + "\"" +
                                    "}}";

        // Start the coroutine to send the API request
        StartCoroutine(SendRequest(url, "POST", response =>
        {
            // Log the response from the API
            Debug.Log("Record created: " + response);

            // Store the response for parsing
            dataToParse = response;

            // Parse the JSON response
            JSONParse();
        }, jsonFields));
    }

    // Coroutine to make API requests
    private IEnumerator SendRequest(string url, string method, Action<string> callback, string jsonData = "")
    {
        // Create a HTTP web request
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = method;
        request.ContentType = "application/json";
        request.Headers["Authorization"] = "Bearer " + accessToken;

        // Include JSON data in the request if provided
        if (!string.IsNullOrEmpty(jsonData))
        {
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(jsonData);
            }
        }

        // Get the response from the API
        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        {
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string jsonResponse = reader.ReadToEnd();
                // Invoke the callback with the response
                if (callback != null)
                {
                    callback(jsonResponse);
                }
            }
        }

        // Yield to the next frame
        yield return null;
    }


    // Method to parse the JSON response from Airtable
    public void JSONParse()
    {
        // Get the JSON source data
        string source = dataToParse;

        // Parse the JSON using Newtonsoft.Json
        dynamic data = JObject.Parse(source);

    }
}
