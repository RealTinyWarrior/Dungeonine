using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
    ! Do not visit this file, it causes PTSD
    
    I am as confused as you are
    This pile of crap containing more than a thousand lines of code took more than a month to code
    I am never making an Inventory Manager ever again
    I'll copy this Inventory Manager on my future projects if needed
    Neither am I commenting this file T-T
    How did I even manage to write this code?
    I am sorry future Tiny :)
*/

public class InventoryManager : MonoBehaviour
{
    [HideInInspector] public Item[] Inventory;
    [HideInInspector] public bool hasStorage = true;
    [HideInInspector] public Item[] chest;
    [HideInInspector] public Item[] utility;
    public GameObject chestObject;
    public GameObject pausePanel;
    public GameObject itemPrefab;
    public RectTransform selectIcon;
    public Sprite emptyItem;
    public Image[] detailObject;
    public TextMeshProUGUI[] detailAmount;
    public Image[] detailImage;
    bool accessedDetail;
    Item[] detailItem;
    public GameObject topDetails;
    public GameObject bottomDetails;
    public TextMeshProUGUI[] detailsTexts;
    int selectedHotbar = 0;
    public Image[] inventorySlots;
    public TextMeshProUGUI[] amountTexts;
    public Image[] iconSlots;
    public Image[] utilitySlot;
    public TextMeshProUGUI[] utilityAmount;
    public Image[] utilityIcons;
    public Image[] chestSlots;
    public Image[] chestIcons;
    public TextMeshProUGUI[] chestAmount;
    public Image utilityIcon0;
    public Image utilityIcon1;
    public GameObject utilityIconContainer;
    ItemStatus itemStatus;
    GameManager gameManager;
    Image[] hotbar = new Image[5];
    List<int> appendedID = new();
    Transform bonineTransform;
    GameItems gameItems;
    int previousIndex = -1;
    [HideInInspector] public int accessedUtilitySlot = -1;
    [HideInInspector] public int previousChestIndex = -1;
    List<string> runningCoroutines = new List<string>();

    void Add(int id, int index, int amount, Item[] itemArray, Image[] iconArray, TextMeshProUGUI[] textArray)
    {
        Item item = gameItems.GetItem(id);
        item.itemReference = gameItems.items[id].itemReference;

        itemArray[index] = item;
        itemArray[index].amount = amount;
        iconArray[index].sprite = item.icon;

        if (item.stackable) textArray[index].text = item.amount.ToString();
        else textArray[index].text = "";
    }

    void Remove(int index, Item[] itemArray, Image[] iconArray, TextMeshProUGUI[] textArray)
    {
        Item emptyItem = gameItems.GetItem(0);

        itemArray[index] = emptyItem;
        iconArray[index].sprite = emptyItem.icon;
        textArray[index].text = "";
    }

    void Exchange(Item item01, int index01, Item[] itemArray01, Image[] iconArray01, TextMeshProUGUI[] textArray01, Item item02, int index02, Item[] itemArray02, Image[] iconArray02, TextMeshProUGUI[] textArray02)
    {
        Add(item02.id, index01, item02.amount, itemArray01, iconArray01, textArray01);
        Add(item01.id, index02, item01.amount, itemArray02, iconArray02, textArray02);
    }

    public void RemoveItem(int itemIndex) => Remove(itemIndex, Inventory, iconSlots, amountTexts);

    void DeselectSlot(int index, string type) => StartCoroutine(DeselectCoroutine(index, type));

    IEnumerator DeselectCoroutine(int index, string type)
    {
        runningCoroutines.Clear();
        string uid = GenerateUniqueId();
        runningCoroutines.Add(uid);

        yield return new WaitForSeconds(5.5f);

        if (runningCoroutines.Contains(uid))
        {
            if (type == "inventory")
            {
                previousIndex = -1;
                inventorySlots[index].DOColor(new Color(0.3608f, 0.6824f, 1f, 0.7f), 0.2f);
            }

            if (type == "chest")
            {
                previousChestIndex = -1;
                chestSlots[index].DOColor(new Color(1, 1, 1, 0.1765f), 0.2f);
            }

            if (type == "utility")
            {
                accessedUtilitySlot = -1;
                utilitySlot[index].DOColor(new Color(0.3608f, 0.6824f, 1f, 0.7f), 0.2f);
            }

            if (type == "details")
            {
                detailObject[index].DOColor(new Color(0.3608f, 0.6824f, 1f, 0.7f), 0.2f);
            }
        }
    }

    void ResetSelection()
    {
        if (accessedUtilitySlot != -1)
        {
            utilitySlot[accessedUtilitySlot].color = new Color(0.3608f, 0.6824f, 1f, 0.7f);
            accessedUtilitySlot = -1;
        }

        if (previousChestIndex != -1)
        {
            chestSlots[previousChestIndex].color = new Color(1, 1, 1, 0.1765f);
            previousChestIndex = -1;
        }

        if (previousIndex != -1)
        {
            inventorySlots[previousIndex].color = new Color(0.3608f, 0.6824f, 1f, 0.7f);
            previousIndex = -1;
        }

        if (accessedDetail)
        {
            detailObject[0].color = new Color(0.3608f, 0.6824f, 1f, 0.7f);
            accessedDetail = false;
        }
    }

    string GenerateUniqueId()
    {
        string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        string randomString = "";

        for (int i = 0; i < 6; i++)
        {
            randomString += alphabet[Random.Range(0, alphabet.Length)];
        }

        return randomString;
    }

    void Awake()
    {
        bonineTransform = GameObject.FindGameObjectWithTag("Bonine").GetComponent<Transform>();
        itemStatus = GameObject.FindGameObjectWithTag("ItemStatus").GetComponent<ItemStatus>();
        gameManager = GetComponent<GameManager>();
        Inventory = new Item[inventorySlots.Length];
        detailItem = new Item[1];
        detailItem[0] = new Item();
        LoadEmptyInventory();
        SetStorage();

        gameItems = GetComponent<GameItems>();
        utility = new Item[2];
        chest = new Item[16];

        utility[0] = new Item();
        utility[1] = new Item();
        StartCoroutine(CheckForDisplayUtility());

        for (int i = 0; i < hotbar.Length; i++) hotbar[i] = inventorySlots[i];
        for (int i = 0; i < chest.Length; i++) chest[i] = new Item();
    }

    void Update()
    {
        if (pausePanel.activeSelf) return;

        bool l2 = Input.GetKeyDown(KeyCode.JoystickButton4);
        bool r2 = Input.GetKeyDown(KeyCode.JoystickButton5);
        int oldSelectedPosition = selectedHotbar;
        float mouseData = Input.mouseScrollDelta.y;

        if (l2 == false && r2 == false)
        {
            if (mouseData > 0) selectedHotbar += 1;
            else if (mouseData < 0) selectedHotbar -= 1;
        }

        else
        {
            if (l2) selectedHotbar -= 1;
            else if (r2) selectedHotbar += 1;
        }

        if (selectedHotbar > 4) selectedHotbar = 0;
        else if (selectedHotbar < 0) selectedHotbar = 4;

        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedHotbar = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) selectedHotbar = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) selectedHotbar = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) selectedHotbar = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5)) selectedHotbar = 4;

        if (selectedHotbar != oldSelectedPosition)
        {
            if (utility[1].id != 0) utility[1].itemReference.GetComponent<ItemParam>().item.mouseKey = 1;
            selectIcon.localPosition = new Vector2(120 * (selectedHotbar + 1) + 20, 20);

            if (Inventory[oldSelectedPosition].id != 0 && Inventory[oldSelectedPosition].itemReference != null && Inventory[selectedHotbar].id != utility[0].id && Inventory[selectedHotbar].id != utility[1].id) Inventory[oldSelectedPosition].itemReference.SetActive(false);
            if (Inventory[selectedHotbar].id != 0 && Inventory[selectedHotbar].itemReference != null && Inventory[selectedHotbar].id != utility[0].id)
            {
                Inventory[selectedHotbar].itemReference.SetActive(true);
                if (utility[0].itemReference != null) utility[0].itemReference.SetActive(false);
            }

            else if (utility[0].id != 0) utility[0].itemReference.SetActive(true);

            if (utility[1].id != 0)
            {
                utility[1].itemReference.GetComponent<ItemParam>().item.mouseKey = 1;
                if (utility[1].id != 0) utility[1].itemReference.SetActive(true);
            }

            if (utility[0].id != 0)
            {
                utility[0].itemReference.GetComponent<ItemParam>().item.mouseKey = 0;
                if (utility[0].id != 0) utility[0].itemReference.SetActive(true);
            }

            if (Inventory[selectedHotbar].id == utility[1].id && Inventory[selectedHotbar].itemType == Item.ItemTypes.Utility && Inventory[selectedHotbar].itemReference != null)
            {
                Inventory[selectedHotbar].itemReference.GetComponent<ItemParam>().item.mouseKey = 0;
                Inventory[selectedHotbar].itemReference.SetActive(true);
            }

            if (Inventory[selectedHotbar].itemType != Item.ItemTypes.Utility && utility[1].id != 0)
            {
                utility[1].itemReference.GetComponent<ItemParam>().item.mouseKey = 1;
            }

            if (Inventory[oldSelectedPosition].itemType == Item.ItemTypes.Utility && Inventory[oldSelectedPosition].id != utility[0].id && Inventory[oldSelectedPosition].id != utility[1].id && Inventory[oldSelectedPosition].itemReference != null) Inventory[oldSelectedPosition].itemReference.SetActive(false);
            CheckForActiveUtility();
        }
    }

    IEnumerator CheckForDisplayUtility()
    {
        while (true)
        {
            bool trigger = false;

            if (utility[1].id != 0)
            {
                utilityIcon1.color = new Color(1, 1, 1, 1);
                utilityIcon1.sprite = utility[1].icon;
                trigger = true;
            }

            else utilityIcon1.color = new Color(1, 1, 1, 0);

            if (Inventory[selectedHotbar].itemType == Item.ItemTypes.Utility)
            {
                utilityIcon0.color = new Color(1, 1, 1, 1);
                utilityIcon0.sprite = Inventory[selectedHotbar].icon;
                trigger = true;
            }

            else if (utility[0].id != 0)
            {
                utilityIcon0.color = new Color(1, 1, 1, 1);
                utilityIcon0.sprite = utility[0].icon;
                trigger = true;
            }

            else utilityIcon0.color = new Color(1, 1, 1, 0);

            if (!trigger) utilityIconContainer.SetActive(false);
            else utilityIconContainer.SetActive(true);
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void LoadEmptyInventory()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            Inventory[i] = new Item
            {
                icon = emptyItem
            };
        }
    }

    public void LoadUserInventory()
    {
        for (int i = 0; i < Inventory.Length; i++)
        {
            int itemID = PlayerPrefs.GetInt("Inventory_" + i, 0);

            if (itemID > 0)
            {
                Item item = gameItems.GetItem(itemID);
                iconSlots[i].sprite = item.icon;
                Inventory[i] = item;

                int itemAmount = PlayerPrefs.GetInt("Amount_Inventory_" + i, -1);
                CreateItemInstance(i, true);

                if (itemAmount != -1)
                {
                    Inventory[i].amount = itemAmount;
                    amountTexts[i].text = Inventory[i].amount.ToString();
                }
            }

            else Inventory[i] = new Item();
        }

        StartCoroutine(ExchangeItemOnLoad());
        CheckForActiveUtility();
    }

    IEnumerator ExchangeItemOnLoad()
    {
        int utility0 = PlayerPrefs.GetInt("Utility_0", 0);
        int utility1 = PlayerPrefs.GetInt("Utility_1", 0);

        if (utility0 != 0)
        {
            Item tempItem = Inventory[0];
            Remove(0, Inventory, iconSlots, amountTexts);
            yield return null;

            AddItemOnBonine((ItemCode)utility0, 1);
            yield return null;

            ExchangeItem(0);
            yield return null;

            ExchangeItemOnUtility(0);
            yield return null;

            AddItemOnBonine((ItemCode)tempItem.id, tempItem.amount);
            yield return null;
        }

        if (utility1 != 0)
        {
            Item tempItem = Inventory[0];
            Remove(0, Inventory, iconSlots, amountTexts);
            yield return null;

            AddItemOnBonine((ItemCode)utility1, 1);
            yield return null;

            ExchangeItem(0);
            yield return null;

            ExchangeItemOnUtility(1);
            yield return null;

            AddItemOnBonine((ItemCode)tempItem.id, tempItem.amount);
            yield return null;
        }

        if (utility[0].id == utility[1].id && utility0 != 0 && utility1 != 0) utility[0].itemReference.GetComponent<ItemParam>().item.mouseKey = 1;
    }

    public void SaveUserInventory()
    {
        for (int i = 0; i < Inventory.Length; i++)
        {
            PlayerPrefs.SetInt("Inventory_" + i, Inventory[i].id);

            if (Inventory[i].stackable) PlayerPrefs.SetInt("Amount_Inventory_" + i, Inventory[i].amount);
            else PlayerPrefs.SetInt("Amount_Inventory_" + i, -1);
        }

        PlayerPrefs.SetInt("Utility_0", utility[0].id);
        PlayerPrefs.SetInt("Utility_1", utility[1].id);
        PlayerPrefs.Save();
    }

    public void AddItemOnBonine(ItemCode itemID, int amount) => AddItemExtended((int)itemID, amount, 0, Vector2.zero, bonineTransform.position);
    public void AddItem(ItemCode itemID, int amount, Vector2 position) => AddItemExtended((int)itemID, amount, 0, Vector2.zero, position);
    public void AddItemExtended(int itemID, int amount, float timer, Vector2 initialForce, Vector2 position, bool dropSound = false)
    {
        if (!gameManager.IsPointNavigable(position))
        {
            Vector2 topPosition = new(position.x, position.y + 0.5f);

            if (gameManager.IsPointNavigable(topPosition))
            {
                position = topPosition;
            }

            else
            {
                position = new Vector2(position.x, position.y - 0.5f);
            }
        }

        GameObject createdItem = Instantiate(itemPrefab, position, Quaternion.Euler(0, 0, 0));
        ItemObject itemObject = createdItem.GetComponent<ItemObject>();
        SpriteRenderer itemImage = createdItem.GetComponent<SpriteRenderer>();

        Item item = gameItems.GetItem(itemID);
        itemImage.sprite = item.icon;
        itemObject.amount = amount;
        itemObject.timer = timer;
        itemObject.id = itemID;

        if (dropSound) createdItem.GetComponent<AudioSource>().Play();
        if (initialForce.x != 0 && initialForce.y != 0) createdItem.GetComponent<Knockback>().ApplyKnockback(initialForce, 1.4f, 0.6f, true);
    }

    public void ExchangeItem(int currentIndex)
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (chestObject.activeSelf)
            {
                bool hasRemovedObject = false;

                for (int i = 0; i < chest.Length; i++)
                {
                    if (chest[i].id == Inventory[currentIndex].id && Inventory[currentIndex].stackable)
                    {
                        if (!hasRemovedObject && Inventory[currentIndex].itemReference != null)
                        {
                            Inventory[currentIndex].itemReference.SetActive(false);
                            hasRemovedObject = true;
                        }

                        chestAmount[i].text = (chest[i].amount + Inventory[currentIndex].amount).ToString();
                        chest[i].amount = chest[i].amount + Inventory[currentIndex].amount;
                        Remove(currentIndex, Inventory, iconSlots, amountTexts);

                        break;
                    }
                }

                for (int i = 0; i < chest.Length; i++)
                {
                    if (chest[i].id == 0)
                    {
                        if (!hasRemovedObject && Inventory[currentIndex].itemReference != null)
                        {
                            Inventory[currentIndex].itemReference.SetActive(false);
                            hasRemovedObject = true;
                        }

                        Add(Inventory[currentIndex].id, i, Inventory[currentIndex].amount, chest, chestIcons, chestAmount);
                        Remove(currentIndex, Inventory, iconSlots, amountTexts);

                        break;
                    }
                }
            }

            ResetSelection();
            return;
        }

        if (previousIndex == -1)
        {
            if (previousChestIndex != -1)
            {
                Item currentItem = Inventory[currentIndex];
                Item currentChest = chest[previousChestIndex];
                int itemIndex = ItemIndexOnInventory(chest[previousChestIndex].id);

                if (currentItem.itemReference != null) currentItem.itemReference.SetActive(false);
                if (currentChest.itemReference != null && previousIndex == selectedHotbar) currentChest.itemReference.SetActive(true);

                if (itemIndex != -1 && Inventory[itemIndex].stackable)
                {
                    AddItemInInventory(currentChest.id, currentChest.amount);
                    Remove(previousChestIndex, chest, chestIcons, chestAmount);
                }

                else Exchange(
                    currentItem, currentIndex, Inventory, iconSlots, amountTexts,
                    currentChest, previousChestIndex, chest, chestIcons, chestAmount
                );
                CreateItemInstance(currentIndex);

                chestSlots[previousChestIndex].color = new Color(1, 1, 1, 0.1765f);
                previousChestIndex = -1;

            }

            else if (accessedUtilitySlot == 0 || accessedUtilitySlot == 1)
            {
                Item currentItem = Inventory[currentIndex];
                Item currentUtility = utility[accessedUtilitySlot];
                utilitySlot[accessedUtilitySlot].color = new Color(0.3608f, 0.6824f, 1f, 0.7f);

                if (currentItem.itemType != Item.ItemTypes.Utility && currentItem.id != 0)
                {
                    accessedUtilitySlot = -1;
                    return;
                }

                Exchange(
                    currentItem, currentIndex, Inventory, iconSlots, amountTexts,
                    currentUtility, accessedUtilitySlot, utility, utilityIcons, utilityAmount
                );
                CreateItemInstance(currentIndex);

                accessedUtilitySlot = -1;

                if (Inventory[currentIndex].id != 0 && Inventory[currentIndex].itemType == Item.ItemTypes.Utility) Inventory[currentIndex].itemReference.GetComponent<ItemParam>().item.mouseKey = 0;
                if (Inventory[currentIndex].itemReference != null && currentIndex != selectedHotbar) Inventory[currentIndex].itemReference.SetActive(false);
                CheckForActiveUtility();

                return;
            }

            //* Add more conditions here if necessary

            else if (accessedDetail)
            {
                if (Inventory[currentIndex].itemReference != null) Inventory[previousIndex].itemReference.SetActive(false);

                Exchange(
                    detailItem[0], 0, detailItem, detailImage, detailAmount,
                    Inventory[currentIndex], currentIndex, Inventory, iconSlots, amountTexts
                );

                CreateItemInstance(currentIndex);

                accessedDetail = false;
                SetupDetails(detailItem[0]);
                CheckForActiveUtility();
                detailObject[0].color = new Color(0.3608f, 0.6824f, 1f, 0.7f);
            }

            else
            {
                DeselectSlot(currentIndex, "inventory");
                inventorySlots[currentIndex].color = new Color(1, 1, 1);
                previousIndex = currentIndex;
            }
        }

        else
        {
            if (previousIndex == currentIndex)
            {
                if (chestObject.activeSelf && Inventory[currentIndex].stackable)
                {
                    for (int i = 0; i < chest.Length; i++)
                    {
                        if (chest[i].id == Inventory[currentIndex].id)
                        {
                            Inventory[currentIndex].amount += chest[i].amount;
                            Remove(i, chest, chestIcons, chestAmount);
                        }

                        amountTexts[currentIndex].text = Inventory[currentIndex].amount.ToString();
                    }
                }

                inventorySlots[previousIndex].color = new Color(0.3608f, 0.6824f, 1f, 0.7f);
                previousIndex = -1;

                return;
            }

            //* Bug fix: deselect change
            if (previousIndex == selectedHotbar)
            {
                if (Inventory[currentIndex].itemReference != null) Inventory[currentIndex].itemReference.SetActive(true);
                if (Inventory[previousIndex].itemReference != null) Inventory[previousIndex].itemReference.SetActive(false);
            }

            else if (currentIndex == selectedHotbar)
            {
                if (Inventory[currentIndex].itemReference != null) Inventory[currentIndex].itemReference.SetActive(false);
                if (Inventory[previousIndex].itemReference != null) Inventory[previousIndex].itemReference.SetActive(true);
            }

            Item previousItem = Inventory[previousIndex];
            Item currentItem = Inventory[currentIndex];

            Exchange(
                previousItem, previousIndex, Inventory, iconSlots, amountTexts,
                currentItem, currentIndex, Inventory, iconSlots, amountTexts
            );

            CreateItemInstance(currentIndex, true);

            //* Resetting to default
            inventorySlots[previousIndex].color = new Color(0.3608f, 0.6824f, 1f, 0.7f);
            previousIndex = -1;
        }

        CheckForActiveUtility();
    }

    public void AccessChest(int slot)
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (hasStorage)
            {
                AddItemOnBonine((ItemCode)chest[slot].id, chest[slot].amount);
                Remove(slot, chest, chestIcons, chestAmount);
            }

            ResetSelection();
            return;
        }

        if (previousChestIndex != -1)
        {
            if (slot == previousChestIndex)
            {
                if (chest[previousChestIndex].stackable)
                {
                    for (int i = 0; i < chest.Length; i++)
                    {
                        if (chest[i].id == chest[previousChestIndex].id && i != previousChestIndex)
                        {
                            chest[previousChestIndex].amount += chest[i].amount;
                            Remove(i, chest, chestIcons, chestAmount);
                        }
                    }

                    for (int i = 0; i < Inventory.Length; i++)
                    {
                        if (Inventory[i].id == chest[previousChestIndex].id)
                        {
                            chest[previousChestIndex].amount += Inventory[i].amount;
                            Remove(i, Inventory, iconSlots, amountTexts);
                        }
                    }

                    chestAmount[previousChestIndex].text = chest[previousChestIndex].amount.ToString();
                }

                chestSlots[previousChestIndex].color = new Color(1, 1, 1, 0.1765f);
                previousChestIndex = -1;
                return;
            }

            if ((chest[previousChestIndex].id == chest[slot].id) && (chest[previousChestIndex].id != 0 || chest[slot].id != 0) && chest[slot].stackable)
            {
                chestAmount[slot].text = (chest[slot].amount + chest[previousChestIndex].amount).ToString();
                chest[slot].amount = chest[slot].amount + chest[previousChestIndex].amount;

                chest[previousChestIndex] = new Item();
                chestIcons[previousChestIndex].sprite = emptyItem;
                chestAmount[previousChestIndex].text = "";
            }

            else Exchange(
                chest[previousChestIndex], previousChestIndex, chest, chestIcons, chestAmount,
                chest[slot], slot, chest, chestIcons, chestAmount
            );

            chestSlots[previousChestIndex].color = new Color(1, 1, 1, 0.1765f);
            previousChestIndex = -1;
        }

        else if (previousIndex != -1)
        {
            if ((chest[slot].id == Inventory[previousIndex].id) && (chest[slot].id != 0 || Inventory[previousIndex].id != 0) && chest[slot].stackable)
            {
                chestAmount[slot].text = (chest[slot].amount + Inventory[previousIndex].amount).ToString();
                chest[slot].amount = chest[slot].amount + Inventory[previousIndex].amount;

                Remove(previousIndex, Inventory, iconSlots, amountTexts);
            }

            else
            {
                if (Inventory[previousIndex].itemReference != null) Inventory[previousIndex].itemReference.SetActive(false);
                if (chest[slot].itemReference != null && previousIndex == selectedHotbar) chest[slot].itemReference.SetActive(true);

                Exchange(
                    Inventory[previousIndex], previousIndex, Inventory, iconSlots, amountTexts,
                    chest[slot], slot, chest, chestIcons, chestAmount
                );

                CreateItemInstance(previousIndex);
            }

            inventorySlots[previousIndex].color = new Color(0.3608f, 0.6824f, 1f, 0.7f);
            previousIndex = -1;
        }

        else
        {
            DeselectSlot(slot, "chest");
            previousChestIndex = slot;
            chestSlots[slot].color = new Color(1, 1, 1, 1);
        }

        SetStorage();
    }

    public void ThrowItem()
    {
        if (previousChestIndex != -1)
        {
            Item chestItem = chest[previousChestIndex];
            Remove(chest[previousChestIndex].id, chest, chestIcons, chestAmount);

            int chestDegree = Random.Range(0, 360);

            Vector2 chestVelocity = new(
                Mathf.Cos(chestDegree * 0.017453f),
                Mathf.Sin(chestDegree * 0.017453f)
            );

            AddItemExtended(chestItem.id, chestItem.amount, 1.5f, chestVelocity * 5.5f, bonineTransform.position, true);

            //? Resetting to default
            chestSlots[previousChestIndex].color = new Color(1, 1, 1, 0.1765f);
            accessedUtilitySlot = -1;
            previousIndex = -1;
            previousChestIndex = -1;

            return;
        }

        if (previousIndex == -1 || Inventory[previousIndex].id == 0) return;

        if ((Inventory[selectedHotbar].id == Inventory[previousIndex].id || selectedHotbar != previousIndex) && Inventory[previousIndex].itemReference != null)
            Inventory[previousIndex].itemReference.SetActive(false);

        Item previousItem = Inventory[previousIndex];
        Remove(previousIndex, Inventory, iconSlots, amountTexts);

        int degree = Random.Range(0, 360);

        Vector2 initialVelocity = new(
            Mathf.Cos(degree * 0.017453f),
            Mathf.Sin(degree * 0.017453f)
        );

        AddItemExtended(previousItem.id, previousItem.amount, 1.5f, initialVelocity * 5.5f, bonineTransform.position, true);

        //? Resetting to default
        inventorySlots[previousIndex].color = new Color(0.3608f, 0.6824f, 1f, 0.7f);
        previousIndex = -1;
        CheckForActiveUtility();
    }

    public void ExchangeItemOnUtility(int slotNumber)
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (hasStorage && utility[slotNumber].id != 0)
            {
                utility[slotNumber].itemReference.GetComponent<ItemParam>().item.mouseKey = 0;
                utility[slotNumber].itemReference.SetActive(false);

                AddItemOnBonine((ItemCode)utility[slotNumber].id, 1);
                Remove(slotNumber, utility, utilityIcons, utilityAmount);
                CheckForActiveUtility();

                if (Inventory[selectedHotbar].itemReference != null) Inventory[selectedHotbar].itemReference.SetActive(true);
            }

            return;
        }

        if (previousIndex != -1)
        {
            if (Inventory[previousIndex].id != 0 && Inventory[previousIndex].itemType != Item.ItemTypes.Utility) return;

            Exchange(
                utility[slotNumber], slotNumber, utility, utilityIcons, utilityAmount,
                Inventory[previousIndex], previousIndex, Inventory, iconSlots, amountTexts
            );

            CreateItemInstance(previousIndex);

            if (Inventory[previousIndex].id != 0) Inventory[previousIndex].itemReference.GetComponent<ItemParam>().item.mouseKey = 0;
            if (Inventory[previousIndex].itemReference != null && previousIndex != selectedHotbar) Inventory[previousIndex].itemReference.SetActive(false);

            inventorySlots[previousIndex].color = new Color(0.3608f, 0.6824f, 1f, 0.7f);
            accessedUtilitySlot = -1;
            previousIndex = -1;
        }

        else
        {
            if (accessedUtilitySlot == 0 || accessedUtilitySlot == 1)
            {
                if (accessedUtilitySlot != slotNumber)
                {
                    if (utility[0].id == 0 && utility[1].id == 0) return;

                    Exchange(
                        utility[0], 0, utility, utilityIcons, utilityAmount,
                        utility[1], 1, utility, utilityIcons, utilityAmount
                    );

                    utility[0].mouseKey = 0;
                    utility[1].mouseKey = 1;
                }

                utilitySlot[0].color = new Color(0.3608f, 0.6824f, 1f, 0.7f);
                utilitySlot[1].color = new Color(0.3608f, 0.6824f, 1f, 0.7f);
                accessedUtilitySlot = -1;
            }

            else
            {
                DeselectSlot(slotNumber, "utility");
                accessedUtilitySlot = slotNumber;

                previousIndex = -1;
                utilitySlot[slotNumber].color = new Color(1, 1, 1);
            }
        }

        CheckForActiveUtility();
    }

    public void RemoveDetailItemOnExit()
    {
        AddItemOnBonine((ItemCode)detailItem[0].id, detailItem[0].amount);
        Remove(0, detailItem, detailImage, detailAmount);
    }

    public void AdjustItemInDetails()
    {
        if (previousIndex != -1)
        {
            if (Inventory[previousIndex].itemReference != null) Inventory[previousIndex].itemReference.SetActive(false);

            Exchange(
                detailItem[0], 0, detailItem, detailImage, detailAmount,
                Inventory[previousIndex], previousIndex, Inventory, iconSlots, amountTexts
            );

            CreateItemInstance(previousIndex);

            inventorySlots[previousIndex].color = new Color(0.3608f, 0.6824f, 1f, 0.7f);

            previousIndex = -1;
            CheckForActiveUtility();
            SetupDetails(detailItem[0]);
        }

        else if (accessedDetail)
        {
            detailObject[0].color = new Color(0.3608f, 0.6824f, 1f, 0.7f);
            accessedDetail = false;
        }

        else
        {
            DeselectSlot(0, "details");

            detailObject[0].color = new Color(1, 1, 1, 1);
            accessedDetail = true;
        }
    }

    void SetupDetails(Item item)
    {
        if (item.id != 0)
        {
            detailsTexts[0].text = "Name: " + item.name;
            detailsTexts[1].text = "Type: " + item.itemType.ToString();
            detailsTexts[2].text = "ID: " + item.id.ToString();
            detailsTexts[3].text = "Stackable: " + (item.stackable ? "Yes" : "No");

            detailsTexts[4].text = gameItems.items[item.id].about;
            topDetails.SetActive(true);
            bottomDetails.SetActive(true);
        }

        else
        {
            topDetails.SetActive(false);
            bottomDetails.SetActive(false);
        }
    }

    public int ItemIndexOnInventory(int id)
    {
        for (int i = 0; i < Inventory.Length; i++)
            if (id == Inventory[i].id) return i;

        return -1;
    }

    //!!Practically Private Functions:

    public void AddItemInInventory(int id, int amount)
    {
        bool itemExists = false;

        for (int j = 0; j < Inventory.Length; j++)
            if (Inventory[j].id == id && Inventory[j].stackable) itemExists = true;

        for (int j = 0; j < Inventory.Length; j++)
        {
            if (Inventory[j].id == id && Inventory[j].stackable)
            {
                Inventory[j].amount += amount;
                amountTexts[j].text = Inventory[j].amount.ToString();

                itemStatus.ShowItemPopup(Inventory[j].name, amount);

                if (Inventory[j].amount < 0) Remove(j, Inventory, iconSlots, amountTexts);
                break;
            }

            else if (Inventory[j].id == 0 && (!itemExists))
            {
                if (amount < 0)
                {
                    SetStorage();
                    return;
                }

                Item item = gameItems.GetItem(id);

                Inventory[j] = item;
                iconSlots[j].sprite = item.icon;

                if (Inventory[j].stackable)
                {
                    Inventory[j].amount = amount;
                    amountTexts[j].text = Inventory[j].amount.ToString();
                }

                CreateItemInstance(j);
                break;
            }
        }

        CheckForActiveUtility();
        SetStorage();
    }

    void CreateItemInstance(int index, bool onLoad = false)
    {
        if (Inventory[index].id != 0 && !onLoad)
        {
            itemStatus.ShowItemPopup(Inventory[index].name, Inventory[index].amount);
        }

        if ((!appendedID.Contains(Inventory[index].id)) && Inventory[index].itemObject != null)
        {
            Item item = gameItems.items[Inventory[index].id];

            item.itemObject.SetActive(false);
            item.itemReference = Instantiate(item.itemObject, bonineTransform);
            ItemParam itemParam = item.itemReference.AddComponent<ItemParam>();

            Inventory[index].itemReference = item.itemReference;
            itemParam.item = Inventory[index];
            appendedID.Add(Inventory[index].id);

            if (index != selectedHotbar) item.itemReference.SetActive(false);
            else item.itemReference.SetActive(true);
        }
    }

    void SetStorage()
    {
        for (int i = 0; i < Inventory.Length; i++)
        {
            if (Inventory[i].id == 0)
            {
                hasStorage = true;
                return;
            }
        }

        hasStorage = false;
    }

    void CheckForActiveUtility()
    {
        if (Inventory[selectedHotbar].id != 0 && Inventory[selectedHotbar].itemReference != null)
        {
            Inventory[selectedHotbar].itemReference.SetActive(true);
            if (utility[0].id != 0 && Inventory[selectedHotbar].id != utility[0].id) utility[0].itemReference.SetActive(false);

            if (utility[1].id == Inventory[selectedHotbar].id) utility[1].itemReference.SetActive(true);
        }

        else
        {
            if (utility[0].id != 0)
            {
                utility[0].itemReference.SetActive(true);
                utility[0].mouseKey = 0;

                utility[0].itemReference.GetComponent<ItemParam>().item.mouseKey = 0;
            }

            if (utility[1].id != 0)
            {
                utility[1].itemReference.SetActive(true);
                utility[1].mouseKey = 1;

                utility[1].itemReference.GetComponent<ItemParam>().item.mouseKey = 1;
            }
        }

        if (utility[1].id != 0 && Inventory[selectedHotbar].itemType == Item.ItemTypes.Utility)
        {
            utility[1].itemReference.SetActive(true);
            utility[1].itemReference.GetComponent<ItemParam>().item.mouseKey = 1;
        }

        if (Inventory[selectedHotbar].id == utility[1].id && utility[1].id != 0) utility[1].itemReference.GetComponent<ItemParam>().item.mouseKey = 0;
    }
}