using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class OnInteraction : MonoBehaviour
{
    public enum InteractionTypes
    {
        Message,
        Chest,
        Others
    }

    public UnityEvent<GameObject> onHoverEnter;
    public UnityEvent<GameObject> onHoverExit;
    public UnityEvent<GameObject> onClick;
    public OnObjectAccess accessManager;
    public string interactionText;
    public GameObject chestObject;
    public GameObject chatObject;
    public InteractionTypes interactionType;

    void Start() => accessManager = transform.GetChild(0).GetComponent<OnObjectAccess>();
    public void OnMouseDown() => StartCoroutine(OnClickManager());
    void Update()
    {
        if (!accessManager.canAccess) return;
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton0)) StartCoroutine(OnClickManager());
    }

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


    IEnumerator OnClickManager()
    {
        if (accessManager.canAccess)
        {
            bool doChange = true;
            if (interactionType == InteractionTypes.Chest) if (chestObject.activeSelf) doChange = false;
            if (interactionType == InteractionTypes.Message) if (chatObject.activeSelf) doChange = false;

            if (doChange)
            {
                if (interactionType == InteractionTypes.Chest) chestObject.SetActive(true);
                yield return null;

                onClick?.Invoke(gameObject);
            }
        }
    }
}
