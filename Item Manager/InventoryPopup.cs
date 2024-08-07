using UnityEngine;
using DG.Tweening;

public class InventoryPopup : MonoBehaviour
{
    public float startValue;
    public float endValue;
    bool isActive = false;
    RectTransform rectTransform;
    public float popupSpeed = 0.5f;
    GameObject pausePanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)) Popup();
    }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        pausePanel = GameObject.FindGameObjectWithTag("PauseScreen");
    }

    public void Popup()
    {
        if (pausePanel == null)
        {
            pausePanel = GameObject.FindGameObjectWithTag("PauseScreen");
            if (pausePanel == null) DoPopup();
        }

        else if (!pausePanel.activeSelf)
        {
            DoPopup();
        }
    }

    private void DoPopup()
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
