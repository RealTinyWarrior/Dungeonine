using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    [HideInInspector]
    public enum FadeTypes
    {
        Text,
        Image
    }

    public float fadeTime;
    public float startTimer;
    public FadeTypes fadeType;
    public float endValue;
    float timer;
    bool isDone = false;

    void Update()
    {
        if (timer < startTimer) timer += Time.deltaTime;
        else if (!isDone)
        {
            isDone = true;
            switch (fadeType)
            {
                case FadeTypes.Image:
                    GetComponent<Image>().DOFade(endValue, fadeTime);
                    break;

                case FadeTypes.Text:
                    GetComponent<TextMeshProUGUI>().DOFade(endValue, fadeTime);
                    break;

            }
        }
    }
}