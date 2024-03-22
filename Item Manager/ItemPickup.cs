using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemObject itemObject;
    InventoryManager inventory;
    bool hasCollided = false;

    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InventoryManager>();
    }

    void Update()
    {
        if (hasCollided && itemObject.isObtainable)
        {
            GameObject parent = transform.parent.gameObject;
            ItemObject item = parent.GetComponent<ItemObject>();
            if (item.timer > 0) return;

            inventory.AddItemInInventory(item.id, item.amount);
            Destroy(parent);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("BonineHitbox")) hasCollided = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("BonineHitbox")) hasCollided = false;
    }
}