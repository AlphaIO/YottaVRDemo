using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WebApiClient : MonoBehaviour {
    const string ApiKeyHeader = "x-api-key";

    public string apiKey = "uw4t4P7ACh6fOxXOXaPiY1XmWdDxAXVJ3fr3lO5y";
    public string baseUrl = "https://rv7u3mbgn5.execute-api.us-east-1.amazonaws.com/prod";
    string sessionId;
    JSONObject states;
    void Start() {
        //SendEvent ("Test");
        states = new JSONObject();
    }

    public void SendEvent(string eventName) {
        StartCoroutine(SendEventCoroutine(eventName));
    }

    IEnumerator StartSessionCoroutine() {
        var jsonRequest = new JSONObject();

        yield return StartCoroutine(HttpRequest(baseUrl + "/session", jsonRequest, Headers("POST"), () => {
            Debug.Log("Failed to start session");
        },
        (jsonResponse) => {
            sessionId = jsonResponse.GetString("SessionId");
            Debug.Log("Session started");
        }));
    }

    System.Int64 TimeStamp {
        get
        {
            var origin = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            var diff = System.DateTime.UtcNow - origin;
            return ( (System.Int64) diff.TotalSeconds ) * 1000;
        }
    }

    public void AddState(string Face) {

        states.Add("" + TimeStamp, Face);
    }


    IEnumerator SendEventCoroutine(string eventName) {
        if (string.IsNullOrEmpty(sessionId)) {
            yield return StartCoroutine(StartSessionCoroutine());
        }
        if (string.IsNullOrEmpty(sessionId)) {
            Debug.Log("Not logged in");
            yield break;
        }


        var metadata = new JSONObject();
        metadata.Add("event", eventName);

        var jsonRequest = new JSONObject();
        jsonRequest.Add("timestamp", TimeStamp);
        jsonRequest.Add("screenshot", "");
        jsonRequest.Add("states", states);
        jsonRequest.Add("metadata", metadata);

        yield return StartCoroutine(HttpRequest(baseUrl + "/session/" + sessionId + "/event", jsonRequest, Headers("PUT"), () => {
            Debug.Log("Failed to post event");
        },
        (jsonResponse) => {
            Debug.Log("Event posted");
        }));
    }

    Dictionary<string, string> Headers(string method) {
        return new Dictionary<string, string>()
        {
            { "Content-Type", "application/json" },
            { ApiKeyHeader, apiKey },
            { "X-HTTP-Method-Override", method },
        };
    }

    IEnumerator HttpRequest(string url, JSONObject request, Dictionary<string, string> headers, System.Action onFailure, System.Action<JSONObject> onSuccess) {
        var body = request != null ? System.Text.Encoding.UTF8.GetBytes(request.ToString()) : null;
        WWW www = new WWW(url, body, headers);
        yield return www;

        var headersStr = "";
        foreach (var h in headers) {
            headersStr += "    " + h.Key + " = " + h.Value + "\n";
        }
        var bodyStr = System.Text.Encoding.UTF8.GetString(body);

        Debug.Log("Request: <color=blue>" + url + "</color>\n\n" +
            "Headers:\n" + headersStr + "\n" +
            "Request Body:\n" + bodyStr + "\n\n" +
            "Response Error: <color=red>" + www.error + "</color>\n" +
            "Response Body:\n" + www.text + "\n\n");

        if (!string.IsNullOrEmpty(www.error)) {
            Debug.LogWarning("Request Failed: " + www.error);
            onFailure();
            yield break;
        }

        onSuccess(JSONObject.Parse(www.text));
        yield break;
    }
}
