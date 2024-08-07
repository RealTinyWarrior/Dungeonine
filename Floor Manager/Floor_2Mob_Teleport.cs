using UnityEngine;
using UnityEngine.AI;

public class Floor_2MobTeleport : MonoBehaviour
{
    public DoorManager doormanager;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (doormanager.doorIsOpened) return;

        if (col.TryGetComponent<NavMeshAgent>(out var agent)) agent.enabled = false;
        col.transform.position = new Vector2(13.5f, 174.62f);
        if (agent != null) agent.enabled = true;
    }
}