using DG.Tweening;
using UnityEngine;

public class AnchorPos : MonoBehaviour
{
    public Vector2 endVector;
    public float duration;
    public float start;
    float startTimer;
    bool isDone = false;

    void Update()
    {
        if (startTimer < start) startTimer += Time.deltaTime;
        else if (!isDone)
        {
            isDone = true;
            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.DOAnchorPos(endVector, duration);
        }
    }
}