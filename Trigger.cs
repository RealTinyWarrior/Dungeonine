using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public UnityEvent<Collider2D> onTriggerEnter;
    public UnityEvent<Collider2D> onTriggerExit;
    [HideInInspector] public bool isActive;
    public string target;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(target))
        {
            onTriggerEnter?.Invoke(col);
            isActive = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag(target))
        {
            onTriggerExit?.Invoke(col);
            isActive = true;
        }
    }
}
