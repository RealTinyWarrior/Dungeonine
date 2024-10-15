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
    Movement bonineMovement;
    GameObject pausePanel;

    void Start()
    {
        accessManager = transform.GetChild(0).GetComponent<OnObjectAccess>();
        pausePanel = GameObject.FindGameObjectWithTag("PauseScreen");
        bonineMovement = GameObject.FindGameObjectWithTag("Bonine").GetComponent<Movement>();
    }

    public void OnMouseDown() => StartCoroutine(OnClickManager());
    void Update()
    {
        if (!accessManager.canAccess) return;

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if (pausePanel == null)
            {
                pausePanel = GameObject.FindGameObjectWithTag("PauseScreen");
                if (pausePanel == null) StartCoroutine(OnClickManager());
            }

            else if (!pausePanel.activeSelf)
            {
                StartCoroutine(OnClickManager());
            }
        }
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

    // Runs the specified interaction function when all of the conditions are met
    IEnumerator OnClickManager()
    {
        if (accessManager.canAccess)
        {
            bool doChange = true;
            if (interactionType == InteractionTypes.Chest) if (chestObject.activeSelf) doChange = false;
            if (interactionType == InteractionTypes.Message) if (chatObject.activeSelf) doChange = false;

            if (doChange)
            {
                bonineMovement.LookAt(transform.position);

                if (interactionType == InteractionTypes.Chest) chestObject.SetActive(true);
                yield return null;

                onClick?.Invoke(gameObject);
            }

            yield return new WaitForSeconds(0.2f);
            bonineMovement.ChangeAnimationState(bonineMovement.idleState);
        }
    }
}
