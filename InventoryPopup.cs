using UnityEngine;
using DG.Tweening;

public class InventoryPopup : MonoBehaviour
{
    public float top;
    public float bottom;
    bool isActive = false;
    RectTransform rectTransform;
    public float popupSpeed = 0.5f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Popup()
    {
        if (isActive)
        {
            isActive = false;
            rectTransform.DOAnchorPosY(bottom, popupSpeed);
        }

        else
        {
            isActive = true;
            rectTransform.DOAnchorPosY(top, popupSpeed);
        }
    }
}
