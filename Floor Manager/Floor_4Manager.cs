using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Floor_4Manager : MonoBehaviour
{
    public Sprite[] chatIcons;
    public GameObject tabletButton;
    public GameObject tabletObject;
    public AudioSource showdownAudio;
    public Vector2[] spawnPoints;
    public Vector2[] bucketPoints;
    public Vector2[] W0RMPoints;
    public GameObject overclockedBucket;
    public GameObject overclockedW0RM;
    public Tilemap tilemap;
    GameManager gameManager;
    EntityManager entityManager;
    InventoryManager inventoryManager;
    MessageManager messageManager;
    Movement bonineMovement;
    bool overclockedEntity;
    bool startingDone = false;
    ItemStatus itemStatus;
    List<GameObject> spawnedEntities = new();

    void Start()
    {
        GameObject gameManagerObject = GameObject.FindGameObjectWithTag("GameManager");
        GameObject bonine = GameObject.FindGameObjectWithTag("Bonine");
        bonineMovement = bonine.GetComponent<Movement>();
        gameManager = gameManagerObject.GetComponent<GameManager>();
        inventoryManager = gameManagerObject.GetComponent<InventoryManager>();

        entityManager = GameObject.FindGameObjectWithTag("EntityManager").GetComponent<EntityManager>();
        messageManager = gameManagerObject.GetComponent<MessageManager>();
        inventoryManager.LoadUserInventory();

        itemStatus = GameObject.FindGameObjectWithTag("ItemStatus").GetComponent<ItemStatus>();
    }

    void Update()
    {
        if (startingDone) return;

        if (messageManager.GetResolved("You can't encounter them without proper equipment, here take this."))
        {
            tabletButton.SetActive(true);
            tabletObject.SetActive(true);
            startingDone = true;

            itemStatus.ShowItemPopup("Tablet", 1);
        }
    }

    public void StartConversation()
    {
        messageManager.Edit("Master", new string[] {
            "Sorry, I'm facing connection issues.. glad to see you safe Bonine!",
            "The robots seem to have become overclocked!",
            "<icon>", "1",
            "You can't encounter them without proper equipment, here take this.",
            "This is a tablet which allows you to manage your utilities and much more.",
            "Give it a go, apply your utilities in the bottom right corner to use both hands during combat."
        }, chatIcons);

        gameManager.hasTablet = true;
    }

    public void Showdown()
    {
        bonineMovement.speed = 4;
        showdownAudio.Play();
        gameManager.bossFightOngoing = true;

        overclockedEntity = Random.Range(0, 2) == 0;

        if (overclockedEntity) Instantiate(overclockedBucket, spawnPoints[Random.Range(0, spawnPoints.Length)], Quaternion.identity);
        else Instantiate(overclockedW0RM, spawnPoints[Random.Range(0, spawnPoints.Length)], Quaternion.identity);

        StartCoroutine(OverClockedShowdown());
    }

    IEnumerator OverClockedShowdown()
    {
        int spawned = 0;
        bonineMovement.speed = 4;

        while (spawned < 33)
        {
            if (spawned == 25)
            {
                if (!overclockedEntity) Instantiate(overclockedBucket, spawnPoints[Random.Range(0, spawnPoints.Length)], Quaternion.identity);
                else Instantiate(overclockedW0RM, spawnPoints[Random.Range(0, spawnPoints.Length)], Quaternion.identity);
            }

            else if (Random.Range(0, 3) != 0)
            {
                GameObject bucket = entityManager.Spawn(EntityCode.Bucket, bucketPoints[Random.Range(0, bucketPoints.Length)]);
                spawnedEntities.Add(bucket);
            }

            else
            {
                GameObject worm = entityManager.Spawn(Random.Range(0, 2) == 0 ? EntityCode.W0RM_B : EntityCode.W0RM_A, W0RMPoints[Random.Range(0, W0RMPoints.Length)]);
                spawnedEntities.Add(worm);
            }

            spawned++;
            yield return new WaitForSeconds(Random.Range(0.6f, 2.2f));
        }

        StartCoroutine(CheckForBattleEnd());
    }

    IEnumerator CheckForBattleEnd()
    {
        bool hasEnded = false;

        while (!hasEnded)
        {
            hasEnded = true;

            foreach (GameObject entity in spawnedEntities)
            {
                if (entity != null)
                {
                    hasEnded = false;
                }
            }

            yield return new WaitForSeconds(2);
        }

        tilemap.SetTile(new Vector3Int(25, 25, 0), null);
        tilemap.SetTile(new Vector3Int(25, 24, 0), null);
        tilemap.SetTile(new Vector3Int(26, 25, 0), null);
        tilemap.SetTile(new Vector3Int(26, 24, 0), null);
        bonineMovement.speed = 2.6f;
        showdownAudio.Stop();

        messageManager.Edit("Master", new string[] {
            "Bonine, you did it, congrats!",
            "I hope you utilized your tablet properly!"
        }, chatIcons, 2);
    }
}
