using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;
using Vuforia;

public class GetActor : DefaultTrackableEventHandler
{
    private const string CREDITS_URL = "https://api.themoviedb.org/3/movie/{0}/credits?api_key=a70f3df7b5f68e1d198a0ab44a5dff54";

    
    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();
        Debug.Log(string.Format(CREDITS_URL, transform.GetComponent<ImageTargetBehaviour>().ImageTarget.Name));
        StartCoroutine(RequestActor(string.Format(CREDITS_URL, transform.GetComponent<ImageTargetBehaviour>().ImageTarget.Name), RequestAuthorCallback));
    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();
    }

    private IEnumerator RequestActor(string url, Action<string> callback)
    {
        var request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.isHttpError || request.isNetworkError)
        {
            Debug.Log("Error");
        }
        else
        {
            callback(request.downloadHandler.text);
        }
    }

    private void RequestAuthorCallback(string data)
    {
        var json = JSON.Parse(data);

        Debug.Log(json["cast"][0]["name"]);

        transform.Find("Canvas").Find("Text").GetComponent<Text>().text = json["cast"][0]["name"];
    }
}
