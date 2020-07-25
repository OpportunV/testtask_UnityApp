using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movie
{
    public string posterPath;
    public string title;
    public float voteAverage;
    public string overview;
    public string releaseDate;
    public string id;

    public Movie(string posterPath, string title, string voteAverage, string overview, string releaseDate, string id)
    {
        this.id = id;
        this.posterPath = posterPath;
        this.title = title;
        this.voteAverage = float.Parse(voteAverage, System.Globalization.CultureInfo.InvariantCulture.NumberFormat) * 10f;
        this.overview = overview;
        this.releaseDate = releaseDate;
    }
}
