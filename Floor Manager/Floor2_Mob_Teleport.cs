using UnityEngine;

public class Floor_2MobTeleport : MonoBehaviour
{
    public DoorManager doormanager;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (doormanager.doorIsOpened) return;
        col.transform.position = new Vector2(13.5f, 174.62f);
    }
}