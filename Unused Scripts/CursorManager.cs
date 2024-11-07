using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public int defaultCursor = 0;
    public GameCursor[] cursors;

    void Start()
    {
        if (defaultCursor != 0)
            Cursor.SetCursor(cursors[defaultCursor].icon, cursors[defaultCursor].offset, CursorMode.Auto);
    }

    public void SetDefaultCursor(int cursorNum)
    {
        defaultCursor = cursorNum;

        if (cursorNum == 0) Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        else Cursor.SetCursor(cursors[cursorNum].icon, cursors[cursorNum].offset, CursorMode.Auto);
    }
}

[System.Serializable]
public class GameCursor
{
    public string name;
    public Texture2D icon;
    public Vector2 offset;
}