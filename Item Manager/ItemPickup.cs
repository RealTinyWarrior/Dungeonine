using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public AudioSource pickupAudio;
    public ItemObject itemObject;
    InventoryManager inventory;
    bool hasCollided = false;

    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InventoryManager>();
        pickupAudio = GameObject.FindGameObjectWithTag("PickupAudio").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (hasCollided && itemObject.isObtainable)
        {
            GameObject parent = transform.parent.gameObject;
            ItemObject item = parent.GetComponent<ItemObject>();
            if (item.timer > 0) return;

            inventory.AddItemInInventory(item.id, item.amount);
            pickupAudio.Play();
            Destroy(parent);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bonine")) hasCollided = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Bonine")) hasCollided = false;
    }
}