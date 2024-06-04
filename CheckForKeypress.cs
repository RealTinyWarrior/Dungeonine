using UnityEngine;
using UnityEngine.Events;

public class CheckForKeyPress : MonoBehaviour
{
    public KeyCode[] keycodes;
    public UnityEvent onClick;

    void Update()
    {
        foreach (KeyCode keyCode in keycodes)
        {
            if (Input.GetKeyDown(keyCode))
            {
                onClick?.Invoke();
            }
        }
    }
}