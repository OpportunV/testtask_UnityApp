using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    private int page = 1;

    private Shower shower;

    private const string IMAGE_URL = "https://image.tmdb.org/t/p/w500";
    private const string URL = "https://api.themoviedb.org/3/discover/movie?api_key=a70f3df7b5f68e1d198a0ab44a5dff54&primary_release_year=2019&sort_by=popularity.desc&page=";
    void Start() 
    {
        Load();
        shower = this.GetComponent<Shower>();
        
    }

    private IEnumerator Request(string url, Action<string> callback)
    {
        var request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.isHttpError || request.isNetworkError)
        {
            Debug.Log("Error");
        } else
        {
            callback(request.downloadHandler.text);
        }
    }

    public IEnumerator RequestImage(string posterPath, RawImage image, Action<RawImage, Texture2D> callback)
    {
        var request = UnityWebRequestTexture.GetTexture(IMAGE_URL + posterPath);
        yield return request.SendWebRequest();

        if (request.isHttpError || request.isNetworkError)
        {
            Debug.Log("Error");
        }
        else
        {
            callback(image, DownloadHandlerTexture.GetContent(request));
        }
    }

    private void RequestCallback(string data)
    {
        var json = JSON.Parse(data);

        page++;
        
        var results = json["results"];

        foreach (var result in results)
        {
            Movie movie = new Movie(result.Value["poster_path"], result.Value["title"], result.Value["vote_average"], result.Value["overview"], result.Value["release_date"], result.Value["id"]);
            shower.Show(movie);
        }
    }

    public void Load()
    {
        StartCoroutine(Request(URL + page.ToString(), RequestCallback));
    }
}
