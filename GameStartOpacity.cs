using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameStartOpacity : MonoBehaviour
{
    public float parentDelay = 2f;
    public float fadeTime = 4f;
    public float exist = 6f;
    float existTimer = 0;
    float startTimer = 0;
    public RectTransform title;
    GameManager gameManager;
    Image fadeImage;
    float parentTimer = 0f;
    bool isDone = false;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        fadeImage = GetComponent<Image>();
        gameManager.canPause = false;
    }

    void Update()
    {
        if (parentTimer < parentDelay) parentTimer += Time.deltaTime;
        else
        {
            if (!isDone)
            {
                isDone = true;
                title.DOAnchorPos(new Vector2(0, 188), 1f);
                title.DOScale(new Vector2(0.6f, 0.6f), 1f);
                fadeImage.DOFade(0f, fadeTime);
            }

            if (existTimer < exist) existTimer += Time.deltaTime;
            else gameObject.SetActive(false);

            if (startTimer < fadeTime) startTimer += Time.deltaTime;
            else
            {
                gameManager.canPause = true;
                gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 800), 1f);
            }
        }
    }
}
