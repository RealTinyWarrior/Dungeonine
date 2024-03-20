using DG.Tweening;
using UnityEngine;

public class DoScale : MonoBehaviour
{
    public Vector2 endVector;
    public float duration;
    public float start;
    float startTimer = 0f;
    bool isDone = false;

    void Start()
    {
        if (startTimer < start) startTimer += Time.deltaTime;
        else if (!isDone)
        {
            isDone = true;
            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.DOScale(endVector, duration);
        }
    }
}