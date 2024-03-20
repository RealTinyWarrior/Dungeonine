using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoverManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public UnityEvent onClick;
    public UnityEvent onHoverEnter;
    public UnityEvent onHoverExit;
    public AudioSource hoverAudio;
    public AudioSource clickSound;
    public Texture2D pointerCursor;
    public Texture2D defaultCursor;

    public void OnPointerEnter(PointerEventData eventData)
    {
        onHoverEnter?.Invoke();
        if (pointerCursor != null) Cursor.SetCursor(pointerCursor, Vector2.zero, CursorMode.Auto);
        if (hoverAudio != null) hoverAudio.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onHoverExit.Invoke();
        if (defaultCursor != null) Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickSound != null) clickSound.Play();
        onClick?.Invoke();
    }
}
