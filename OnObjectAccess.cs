using UnityEngine;

public class OnObjectAccess : MonoBehaviour
{
    [HideInInspector] public bool canAccess = false;
    public GameObject interactItem;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bonine"))
        {
            canAccess = true;
            if (interactItem != null) interactItem.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Bonine"))
        {
            canAccess = false;
            if (interactItem != null) interactItem.SetActive(false);
        }
    }
}
