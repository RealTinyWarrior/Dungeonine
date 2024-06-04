using UnityEngine;

public class Floor_1Manager : MonoBehaviour
{
    public Sprite[] chatIcons;
    public Vector2[] spawnPoints;
    public GameObject darkManager;
    public Transform movableObject;
    public DoorManager doorManager;
    public OnInteraction leverInteraction;
    public GameObject leverObject;
    InventoryManager inventory;
    MessageManager message;
    float gameStartTimer = 0f;
    Movement bonineMovement;
    EntityManager entityManager;
    bool hasSpawned;
    bool bucketsSpawned;
    bool hasCompleted = false;

    void Start()
    {
        entityManager = GameObject.FindGameObjectWithTag("EntityManager").GetComponent<EntityManager>();
        GameObject bonine = GameObject.FindGameObjectWithTag("Bonine");
        bonineMovement = bonine.GetComponent<Movement>();
        inventory = GetComponent<InventoryManager>();
        message = GetComponent<MessageManager>();
        bonineMovement.allowMovement = false;

        inventory.LoadUserInventory();

        if (PlayerPrefs.GetInt("LevelsUnlocked", 1) > 1)
        {
            bonine.transform.position = new Vector2(15.6f, 10.2f);
            movableObject.position = new Vector2(9.6f, -4.55f);
            bonineMovement.allowMovement = true;
            leverInteraction.onClick.Invoke(leverObject);
            darkManager.SetActive(false);
            hasCompleted = true;

            doorManager.doorInteraction.SetActive(false);
            doorManager.doorIsOpened = true;
            doorManager.OpenDoor();
        }
    }

    void Update()
    {
        if (hasCompleted) return;
        if (gameStartTimer < 6.5f && gameStartTimer >= 0) gameStartTimer += Time.deltaTime;

        else if (gameStartTimer != -1)
        {
            bonineMovement.allowMovement = false;

            message.Edit("???", new string[] {
                "Hello, there little one. You look lost.",

                "<icon>", "1",
                "Oh me? You can call me Master.",

                "<name>", "Master",
                "This is not a safe place, but I can assist you to... survive.",

                "<icon>", "0",
                "Look at the wardrobe at the corner, you might find something useful there.",
                "There are many.. evil robots here, so be safe, you'd need a weapon to destroy them."
            }, chatIcons);

            gameStartTimer = -1;
        }

        if (message.GetResolved("There are many.. evil robots here, so be safe, you'd need a weapon to destroy them.")) bonineMovement.allowMovement = true;
        if (message.GetResolved("I'll let you figure that out on your own.")) bonineMovement.allowMovement = true;
        if (message.GetResolved("Good luck on the next floor.")) bonineMovement.allowMovement = true;
    }

    public void SpawnW0RMOnCarpet()
    {
        if (hasSpawned || hasCompleted) return;

        foreach (Vector2 point in spawnPoints)
        {
            GameObject W0RM;

            if (Random.Range(0, 2) == 0) W0RM = entityManager.Spawn(EntityCode.W0RM_A, point);
            else W0RM = entityManager.Spawn(EntityCode.W0RM_B, point);

            W0RM.GetComponent<W0RM>().viewRange = 11;
        }

        hasSpawned = true;
    }

    public void MasterTalk02()
    {
        if (hasCompleted) return;
        bonineMovement.allowMovement = false;

        message.Edit("Master", new string[] {
            "A puzzle, huh.",
            "I'll let you figure that out on your own."
        }, chatIcons);
    }

    public void MasterTalk03()
    {
        if (hasCompleted) return;
        bonineMovement.allowMovement = false;

        message.Edit("Master", new string[] {
            "Well done buddy!",
            "Good luck on the next floor."
        }, chatIcons);
    }

    public void SpawnBuckets()
    {
        if (hasCompleted) return;
        if (bucketsSpawned) return;
        GameObject bucket01 = entityManager.Spawn(EntityCode.Bucket, new Vector2(-7.2f, 11.5f));
        GameObject bucket02 = entityManager.Spawn(EntityCode.Bucket, new Vector2(-5f, 9));

        bucket01.GetComponent<Bucket>().viewDistance = 5;
        bucket02.GetComponent<Bucket>().viewDistance = 5;
        bucketsSpawned = true;
    }
}
