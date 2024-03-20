using UnityEngine;
using UnityEngine.Events;

public class OnInteraction : MonoBehaviour
{
    public UnityEvent<GameObject> onHoverEnter;
    public UnityEvent<GameObject> onHoverExit;
    public UnityEvent<GameObject> onClick;
    public OnObjectAccess accessManager;
    public string interactionText;

    void Start() => accessManager = transform.GetChild(0).GetComponent<OnObjectAccess>();

    void OnMouseEnter()
    {
        if (!accessManager.canAccess) return;
        onHoverEnter?.Invoke(gameObject);
    }

    public void OnMouseExit()
    {
        if (!accessManager.canAccess) return;
        onHoverExit?.Invoke(gameObject);
    }

    void OnMouseDown()
    {
        if (!accessManager.canAccess) return;
        onClick?.Invoke(gameObject);
    }
}
