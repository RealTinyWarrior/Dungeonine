using TMPro;
using UnityEngine;
using UnityEngine.UI;

// This is the only code of Dungeonine that looks dope lmao

[SerializeField]
public class ChestManager : MonoBehaviour
{
    InventoryManager inventory;
    GameItems gameItems;
    GameManager gameManager;
    bool doLoadChest = true;
    public GameObject chestObject;
    public TextMeshProUGUI chestName;
    public Image chestBackground;
    public int floor;
    public Chest[] chest;
    public AudioSource chestOpen;
    public AudioSource chestClose;
    int openedChestID = -1;
    Movement movement;
    [HideInInspector] public bool hasOpened = false;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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

    // Loads the corresponding chest from `chest[]` into the UI
    public void OpenChestID(int id)
    {
        ChangeChestName(chest[id].name);
        movement.allowMovement = false;
        gameManager.canPause = false;
        openedChestID = id;
        hasOpened = true;

        chestOpen.Play();

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

    // Exits the opened chest
    public void ExitChestID()
    {
        if (inventory.accessedUtilitySlot != -1) inventory.chestSlots[inventory.accessedUtilitySlot].color = new Color(1, 1, 1, 0.1765f);
        inventory.accessedUtilitySlot = -1;
        hasOpened = false;
        chestClose.Play();

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
        gameManager.canPause = true;
    }

    // Stores the items of the chest in PlayerPrefs
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

    // Loads chesf from PlayerPrefs into `chest[]`
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

    void ChangeChestSprite(Sprite sprite) => chestBackground.sprite = sprite;
    void ChangeChestName(string name) => chestName.text = name;
}
