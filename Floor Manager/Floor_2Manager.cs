using UnityEngine;

public class Floor_2Manager : MonoBehaviour
{
    InventoryManager inventory;
    ChestManager chestManager;

    void Start()
    {
        inventory = GetComponent<InventoryManager>();
        chestManager = GetComponent<ChestManager>();

        inventory.LoadUserInventory();
    }
}
