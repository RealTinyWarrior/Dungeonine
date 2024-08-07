using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class Floor_2Manager : MonoBehaviour
{
    public Sprite[] chatIcons;
    public DoorManager doormanager;
    public Vector2[] moreBuckets;
    public AudioSource music;
    public bool hasPulledLever;
    public GameObject darkManager01;
    public GameObject startLadder;
    public AudioSource backgroundMusic;
    MessageManager messageManager;
    EntityManager entityManager;
    Movement movement;
    InventoryManager inventory;
    GameManager pauseManager;
    bool hasCompletedLevel = false;

    void Update()
    {
        if (messageManager.GetResolved("Quick RUN!! Close the gates by flicking the lever, run!") || messageManager.GetResolved("You know the drill, run!"))
        {
            music.Play();
            backgroundMusic.Stop();
            pauseManager.bossFightOngoing = true;
            darkManager01.SetActive(false);
            StartCoroutine(SlowlySpawnBuckets());

            movement.speed = 6;
            movement.allowMovement = true;
            startLadder.SetActive(false);
        }
    }

    void Start()
    {
        entityManager = GameObject.FindGameObjectWithTag("EntityManager").GetComponent<EntityManager>();
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        pauseManager = gameManager.GetComponent<GameManager>();
        messageManager = gameManager.GetComponent<MessageManager>();
        inventory = gameManager.GetComponent<InventoryManager>();
        movement = GameObject.FindGameObjectWithTag("Bonine").GetComponent<Movement>();
        StartCoroutine(Conversation());

        if (PlayerPrefs.GetInt("LevelsUnlocked", 2) > 2)
        {
            hasCompletedLevel = true;
        }

        inventory.LoadUserInventory();
    }

    IEnumerator Conversation()
    {
        movement.allowMovement = false;
        yield return new WaitForSeconds(4.8f);

        if (!hasCompletedLevel)
        {
            messageManager.Edit("Master", new string[] {
                "Look who's back!",
                "Congrats for finding your way out of there.",
                "Move on, more mysteries await!"
            }, chatIcons);
        }

        else
        {
            messageManager.Edit("Master", new string[] {
                "So you are back again on this floor.",
                "Remember, the buckets are still out here, so be careful."
            }, chatIcons);
        }
    }

    public void LastMomentConvo()
    {
        if (hasPulledLever) return;

        hasPulledLever = true;
        messageManager.Edit("Master", new string[] {
            "Good job!",
            "This floor is dangerous, be careful..",
            "Anyways.. move on!"
        }, chatIcons);
    }

    public void TouchTrigger()
    {
        if (!hasCompletedLevel)
        {
            messageManager.Edit("Master", new string[] {
                "Uh oh...",
                "The buckets, they escaped...",
                "You uhh.. have to RUN!",
                "Listen to the blue arrows and DO NOT listen to the red arrows.",
                "Quick RUN!! Close the gates by flicking the lever, run!"
            }, chatIcons);
        }

        else
        {
            messageManager.Edit("Master", new string[] {
                "Uhmm..",
                "They escaped again.",
                "You know the drill, run!"
            }, chatIcons);
        }
    }

    public void SpawnMoreBuckets(int index) => StartCoroutine(SpawnBucket(moreBuckets[index]));
    public void SetDoorToClosed() => doormanager.doorIsOpened = true;

    public void SaveGame()
    {
        PlayerPrefs.SetInt("LevelsUnlocked", 2);
        PlayerPrefs.Save();

        inventory.SaveUserInventory();
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator SlowlySpawnBuckets()
    {
        for (int i = 1; i <= 60; i++)
        {
            Bucket bucket = entityManager.Spawn(EntityCode.Bucket_No_Glow, i % 2 == 0 ? new Vector2(6.5f, 3.5f) : new Vector2(-5.2f, 3.5f)).GetComponent<Bucket>();
            bucket.willDespawn = true;
            bucket.viewDistance = 1000;
            bucket.despawnTimer = 50;
            bucket.speed = 10.5f;

            Health bucketHealth = bucket.GetComponent<Health>();
            bucket.AddComponent<DestroyObject>().timer = 50;
            bucketHealth.maxHealth = 100;
            bucketHealth.health = 100;

            yield return new WaitForSeconds(0.0005f);
        }
    }

    IEnumerator SpawnBucket(Vector2 position)
    {
        for (int i = 1; i <= 22; i++)
        {
            Bucket bucket = entityManager.Spawn(EntityCode.Bucket_No_Glow, position).GetComponent<Bucket>();
            bucket.viewDistance = 1000;
            bucket.speed = 10.5f;

            Health bucketHealth = bucket.GetComponent<Health>();
            bucket.AddComponent<DestroyObject>().timer = 50;
            bucketHealth.maxHealth = 100;
            bucketHealth.health = 100;

            yield return new WaitForSeconds(0.003f);
        }
    }
}
