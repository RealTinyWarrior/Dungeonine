using DG.Tweening;
using UnityEngine;

public class LightSource : MonoBehaviour
{
    public float blinkInterval = 2f;
    public float startPoint;
    public float endPoint;
    float timer = 0;
    bool increase = true;

    void Update()
    {
        if (increase) timer += Time.deltaTime;
        else timer -= Time.deltaTime;

        if (timer >= blinkInterval)
        {
            transform.DOScale(endPoint, blinkInterval);
            increase = false;
        }

        if (timer <= 0)
        {
            transform.DOScale(startPoint, blinkInterval);
            increase = true;
        }
    }
}
