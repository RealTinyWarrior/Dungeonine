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
    List<GameObject> spawnedEntities = new();

    void Start()
    {
        GameObject gameManagerObject = GameObject.FindGameObjectWithTag("GameManager");
        GameObject bonine = GameObject.FindGameObjectWithTag("Bonine");
        bonineMovement = bonine.GetComponent<Movement>();
        bonineMovement.allowMovement = false;
        gameManager = gameManagerObject.GetComponent<GameManager>();
        inventoryManager = gameManagerObject.GetComponent<InventoryManager>();
        inventoryManager.LoadUserInventory();

        entityManager = GameObject.FindGameObjectWithTag("EntityManager").GetComponent<EntityManager>();
        messageManager = gameManagerObject.GetComponent<MessageManager>();
        StartCoroutine(StartConversation());
    }

    void Update()
    {
        if (startingDone) return;

        if (messageManager.GetResolved("Once you enter that door, there's no coming back, good luck!"))
        {
            tabletButton.SetActive(true);
            tabletObject.SetActive(true);
            startingDone = true;
        }
    }

    IEnumerator StartConversation()
    {
        yield return new WaitForSeconds(4.8f);

        messageManager.Edit("Master", new string[] {
            "Hey Bonine! Sorry I lost connection. Glad to see you safe!",
            "<icon>", "1",
            "Alright, listen up, this floor's gonna be a tough one.",
            "The buckets and the W0RMs, they went OVERCLOCKED!",
            "Using multiple utility with one hand might become difficult for you, here take this.",
            "This is a Tablet, it has many functionalities",
            "On the bottom right corner, you'll see two utility slots, where you can use utility on your off hand.",
            "Throughout your journey, you'll get many more applications for this tablet",
            "Once you enter that door, there's no coming back, good luck!"
        }, chatIcons);
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
            "Uhh",
            "<icon>", "1",
            "Bonine, you did it, congrats!",
            "I hope you utilized your tablet properly!"
        }, chatIcons);
    }
}
