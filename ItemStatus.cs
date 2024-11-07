using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ItemStatus : MonoBehaviour
{
    public GameObject popupText;

    [Header("Time and Position")]
    public Vector2 endPosition;
    public float fadeDragTime = 1;
    public float fadeInTime = 1;
    public float fadeOutTime = 0.5f;
    public float waitDuration = 1f;
    AudioSource pickupAudio;

    void Start()
    {
        pickupAudio = GameObject.FindGameObjectWithTag("PickupAudio").GetComponent<AudioSource>();
    }

    public void ShowItemPopup(string name, int amount)
    {
        GameObject popup = Instantiate(popupText, transform);
        TextMeshProUGUI text = popup.GetComponent<TextMeshProUGUI>();
        RectTransform rect = popup.GetComponent<RectTransform>();

        text.text = name + " x" + (amount == 0 ? 1 : amount).ToString();

        StartCoroutine(ShowPopupCoroutine(text, rect, popup));
        pickupAudio.Play();
    }

    IEnumerator ShowPopupCoroutine(TextMeshProUGUI text, RectTransform textRect, GameObject mainObject)
    {
        text.DOFade(1, fadeInTime);
        textRect.DOAnchorPos(endPosition, fadeDragTime).SetEase(Ease.OutQuint);
        yield return new WaitForSeconds(waitDuration);

        text.DOFade(0, fadeOutTime);
        yield return new WaitForSeconds(fadeOutTime + 0.1f);
        Destroy(mainObject);
    }
}