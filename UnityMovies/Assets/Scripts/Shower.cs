using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Shower : MonoBehaviour
{
    private Loader loader;
    private float width;
    private int totalMovies = 0;
    private float currentMovie = 0f;
    private Vector3 velocity = Vector3.zero;
    private float buttonTarget = 0f;
    private bool buttonPressed = false;
    private bool needMore = true;
    private float smoothTime = .02f;

    public GameObject moviePanel;
    public GameObject parentPanel;

    private string URL = "https://www.themoviedb.org/movie/";

    void Start()
    {
        loader = GetComponent<Loader>();
        width = parentPanel.GetComponent<HorizontalLayoutGroup>().spacing + moviePanel.GetComponent<RectTransform>().rect.width;
        transform.Find("Buttons").Find("Next").GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(ButtonClick(currentMovie + 1f)); });
        transform.Find("Buttons").Find("Previous").GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(ButtonClick(currentMovie - 1f)); });
    }

    void Update()
    {
        currentMovie = Mathf.Abs(Mathf.Round(parentPanel.GetComponent<RectTransform>().anchoredPosition.x / width));
        if (currentMovie == 0)
        {
            transform.Find("Buttons").Find("Previous").GetComponent<Button>().interactable = false;
        } else
        {
            transform.Find("Buttons").Find("Previous").GetComponent<Button>().interactable = true;
        }

        if (totalMovies - currentMovie < 5f && needMore)
        {
            StartCoroutine(RequestLoad());
        }
    }

    void LateUpdate()
    {
        MovePanel();
    }

    private IEnumerator ButtonClick(float target)
    {
        buttonTarget = target;
        buttonPressed = true;
        yield return new WaitForSeconds(smoothTime);
        buttonPressed = false;
    }

    private IEnumerator RequestLoad()
    {
        needMore = false;
        loader.Load();
        yield return new WaitForSeconds(1f);
        needMore = true;
    }

    public void Show(Movie movie)
    {
        totalMovies++;

        Transform currentPanel = Instantiate(moviePanel, parentPanel.transform).transform;

        currentPanel.Find("Title").GetComponent<Text>().text = movie.title;

        Transform innerPanel = currentPanel.Find("InnerPanel").transform;
        innerPanel.Find("Rating").GetComponent<Image>().fillAmount = movie.voteAverage / 100f;
        innerPanel.Find("Date").GetComponent<Text>().text = movie.releaseDate.Replace("-", ".");
        innerPanel.Find("RatingForeground").Find("RatingText").GetComponent<Text>().text = movie.voteAverage.ToString();
        innerPanel.Find("More").GetComponent<Button>().onClick.AddListener(delegate { Application.OpenURL(URL + movie.id); });

        RawImage poster = innerPanel.Find("Poster").GetComponent<RawImage>();
        StartCoroutine(loader.RequestImage(movie.posterPath, poster, ApplyImage));

        currentPanel.Find("ScrollRect").Find("Description").GetComponent<Text>().text = movie.overview;
    }

    private void ApplyImage(RawImage image, Texture2D texture)
    {
        image.texture = texture;
        var color = image.color;
        color.a = 1f;
        image.color = color;
    }

    private void MovePanel()
    {
        var pos = parentPanel.GetComponent<RectTransform>().anchoredPosition;
        Vector3 targetPos;

        if (buttonPressed)
        {
            targetPos = new Vector3(-(buttonTarget) * width, 0f, 0f);
        }
        else
        {
            targetPos = new Vector3(-(currentMovie) * width, 0f, 0f);
        }

        if (Input.touchCount == 1)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            return;
        }

        if (Mathf.Abs(targetPos.x - pos.x) > 100f)
        {
            parentPanel.GetComponent<RectTransform>().anchoredPosition = Vector3.SmoothDamp(pos, targetPos, ref velocity, smoothTime);
        } else
        {
            parentPanel.GetComponent<RectTransform>().anchoredPosition = targetPos;
        }
    }
}
