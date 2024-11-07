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


        if (PlayerPrefs.GetInt("LevelsUnlocked", 1) > 1)
        {
            bonine.transform.position = new Vector2(15.6f, 10.2f);
            movableObject.position = new Vector2(9.6f, -4.55f);
            bonineMovement.allowMovement = true;
            leverInteraction.onClick.Invoke(leverObject);
            leverInteraction.leverPulled = true;

            darkManager.SetActive(false);
            hasCompleted = true;

            doorManager.doorInteraction.SetActive(false);
            doorManager.doorIsOpened = true;
            doorManager.OpenDoor();
        }

        inventory.LoadUserInventory();
    }

    void Update()
    {
        if (hasCompleted) return;
        if (gameStartTimer < 4.8f && gameStartTimer >= 0) gameStartTimer += Time.deltaTime;

        else if (gameStartTimer != -1)
        {
            message.Edit("???", new string[] {
                "Hey there little one, you look lost.",

                "<icon>", "1",
                "Who am I you may ask? You can call me Master. I'm here to assist you.",

                "<name>", "Master",
                "This dungeon is filled with robots and... life.",

                "<icon>", "0",
                "Nevertheless, look at the wardrobe in the corner, you may find something useful there.",
            }, chatIcons);

            gameStartTimer = -1;
        }
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

        message.Edit("Master", new string[] {
            "A puzzle, huh. I'll let you figure that out on your own."
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
