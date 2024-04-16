using UnityEngine;
using UnityEngine.UI;

[SerializeField]
public class ChestManager : MonoBehaviour
{
    InventoryManager inventory;
    GameItems gameItems;
    bool doLoadChest = true;
    public GameObject chestObject;
    public Image chestBackground;
    public int floor;
    public Chest[] chest;
    int openedChestID = -1;
    Movement movement;

    void Start()
    {
        movement = GameObject.FindGameObjectWithTag("Bonine").GetComponent<Movement>();
        inventory = GetComponent<InventoryManager>();
        gameItems = GetComponent<GameItems>();

        for (int i = 0; i < chest.Length; i++)
        {
            for (int j = 0; j < chest[i].items.Length; j++)
            {
                if (chest[i].items[j].rangeAmount)
                {
                    chest[i].items[j].amount = Random.Range(chest[i].items[j].firstAmount, chest[i].items[j].lastAmount);
                }
            }
        }
    }

    public void OpenChestID(int id)
    {
        movement.allowMovement = false;
        openedChestID = id;

        if (doLoadChest)
        {
            LoadChestData();
            doLoadChest = false;
        }

        for (int i = 0; i < chest.Length; i++)
        {
            if (chest[i].id == id)
            {
                ItemID[] itemIDs = chest[i].items;

                for (int j = 0; j < itemIDs.Length; j++)
                {
                    if (itemIDs[j].id == 0 && (!itemIDs[j].spawnable)) inventory.chest[j] = new Item();

                    else
                    {
                        Item item = gameItems.GetItem(itemIDs[j].id);
                        inventory.chestIcons[j].sprite = item.icon;
                        item.amount = itemIDs[j].amount;
                        inventory.chest[j] = item;

                        if (item.stackable) inventory.chestAmount[j].text = item.amount.ToString();
                    }
                }

                break;
            }
        }

        chestObject.SetActive(true);
    }

    public void ExitChestID()
    {
        movement.allowMovement = true;
        int id = openedChestID;
        chestObject.SetActive(false);

        for (int i = 0; i < chest.Length; i++)
        {
            if (chest[i].id == id)
            {
                for (int j = 0; j < chest[i].items.Length; j++)
                {
                    chest[i].items[j].id = inventory.chest[j].id;
                    chest[i].items[j].amount = inventory.chest[j].amount;
                }

                break;
            }
        }

        openedChestID = -1;
    }

    public void SaveChestData()
    {
        for (int i = 0; i < chest.Length; i++)
        {
            for (int j = 0; j < chest[i].items.Length; j++)
            {
                Item item = gameItems.items[chest[i].items[j].id];
                PlayerPrefs.SetInt("ID_" + floor + "_" + chest[i].id + "_" + j, chest[i].items[j].id);

                if (item.stackable) PlayerPrefs.SetInt("Amount_" + floor + "_" + chest[i].id + "_" + j, chest[i].items[j].amount);
                else PlayerPrefs.SetInt("Amount_" + floor + "_" + chest[i].id + "_" + j, -1);
            }
        }

        PlayerPrefs.Save();
    }

    public void LoadChestData()
    {
        for (int i = 0; i < chest.Length; i++)
        {
            for (int j = 0; j < chest[i].items.Length; j++)
            {
                int itemID = PlayerPrefs.GetInt("ID_" + floor + "_" + chest[i].id + "_" + j, chest[i].items[j].id);

                if (itemID > 0)
                {
                    chest[i].items[j].id = itemID;
                    int itemAmount = PlayerPrefs.GetInt("Amount_" + floor + "_" + chest[i].id + "_" + j, chest[i].items[j].amount);

                    if (itemAmount != -1 && chest[i].items[j].id != 0 && gameItems.items[chest[i].items[j].id].stackable)
                    {
                        chest[i].items[j].amount = itemAmount;
                    }
                }

                else if (!chest[i].items[j].spawnable) chest[i].items[j] = new ItemID();
            }
        }
    }

    public void ChangeChestSprite(Sprite sprite)
    {
        chestBackground.sprite = sprite;
    }
}
