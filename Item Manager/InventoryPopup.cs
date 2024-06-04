using UnityEngine;
using DG.Tweening;

public class InventoryPopup : MonoBehaviour
{
    public float startValue;
    public float endValue;
    bool isActive = false;
    RectTransform rectTransform;
    public float popupSpeed = 0.5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)) Popup();
    }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Popup()
    {
        if (isActive)
        {
            isActive = false;
            rectTransform.DOAnchorPosY(startValue, popupSpeed);
        }

        else
        {
            isActive = true;
            rectTransform.DOAnchorPosY(endValue, popupSpeed);
        }
    }
}
