using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;

public class GetActor : MonoBehaviour
{
    private const string CREDITS_URL = "https://api.themoviedb.org/3/movie/{0}/credits?api_key=a70f3df7b5f68e1d198a0ab44a5dff54";

    void Start()
    {
        StartCoroutine(RequestActor(string.Format(CREDITS_URL, "123"), RequestAuthorCallback));
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

        var result = json["results"];

        GetComponent<Text>().text = result["cast"][0]["name"];
    }
}
