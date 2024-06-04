using UnityEngine;
using UnityEngine.EventSystems;

public class CursorObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum CursorType
    {
        UI,
        GameObject
    }

    public int cursorCode;
    public CursorType cursorType;
    CursorManager cursorManager;

    void Start() => cursorManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CursorManager>();

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (cursorType == CursorType.UI) ApplyCursor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (cursorType == CursorType.UI) ApplyDefaultCursor();
    }

    void OnMouseEnter()
    {
        if (cursorType == CursorType.GameObject) ApplyCursor();
    }

    void OnMouseExit()
    {
        if (cursorType == CursorType.GameObject) ApplyDefaultCursor();
    }

    void ApplyCursor()
    {
        GameCursor cursor = cursorManager.cursors[cursorCode];
        Cursor.SetCursor(cursor.icon, cursor.offset, CursorMode.Auto);
    }

    void ApplyDefaultCursor()
    {
        if (cursorManager.defaultCursor == 0)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            return;
        }

        GameCursor cursor = cursorManager.cursors[cursorManager.defaultCursor];
        Cursor.SetCursor(cursor.icon, cursor.offset, CursorMode.Auto);
    }
}