using UnityEngine;

public class Floor_1Manager : MonoBehaviour
{
    public Sprite[] chatIcons;
    InventoryManager inventory;
    MessageManager message;
    // float gameStartTimer = 0f;
    Movement bonineMovement;
    float timer;
    bool isDone = false;
    ChestManager chestManager;

    void Start()
    {
        GameObject bonine = GameObject.FindGameObjectWithTag("Bonine");

        bonineMovement = bonine.GetComponent<Movement>();
        inventory = GetComponent<InventoryManager>();
        message = GetComponent<MessageManager>();
        chestManager = GetComponent<ChestManager>();
        //bonineMovement.allowMovement = false;

        inventory.LoadUserInventory();
    }

    void Update()
    {
        if (timer < 1) timer += Time.deltaTime;
        else if (!isDone)
        {
            chestManager.OpenChestID(0);
            isDone = true;
        }
    }

    // void Update()
    // {
    //     if (gameStartTimer < 6.5f && gameStartTimer >= 0) gameStartTimer += Time.deltaTime;

    //     else if (gameStartTimer != -1)
    //     {
    //         bonineMovement.allowMovement = false;

    //         message.Edit("???", new string[] {
    //             "Hello, there little one. You look lost.",

    //             "<icon>", "1",
    //             "Oh me? You can call me Master.",

    //             "<name>", "Master",
    //             "This is not a safe place, but I can assist you to... survive.",

    //             "<icon>", "0",
    //             "Look at the wardrobe on the corner, you might find something useful there."
    //         }, chatIcons);

    //         gameStartTimer = -1;
    //     }

    //     if (message.GetResolved("Look at the wardrobe on the corner, you might find something useful there.")) bonineMovement.allowMovement = true;
    // }
}
