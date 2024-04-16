using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Floor_2Manager : MonoBehaviour
{
    public Sprite[] chatIcons;
    public DoorManager doormanager;
    public Vector2[] moreBuckets;
    public AudioSource music;
    MessageManager messageManager;
    EntityManager entityManager;
    Movement movement;
    InventoryManager inventory;

    void Update()
    {
        if (messageManager.GetResolved("Move on, more mysteries await!") || messageManager.GetResolved("Anyways.. let's move on!")) movement.allowMovement = true;

        if (messageManager.GetResolved("Quick RUN!! Close the gates by flicking the lever, run!"))
        {
            music.Play();

            for (int i = 0; i < 25; i++)
            {
                Bucket bucket = entityManager.Spawn(EntityCode.Bucket, new Vector2(6.5f, 2)).GetComponent<Bucket>();
                bucket.viewDistance = 1000;
                bucket.speed = 10.5f;
            }

            for (int i = 0; i < 25; i++)
            {
                Bucket bucket = entityManager.Spawn(EntityCode.Bucket, new Vector2(-5.2f, 2)).GetComponent<Bucket>();
                bucket.viewDistance = 1000;
                bucket.speed = 10.5f;
            }

            movement.speed = 6;
            movement.allowMovement = true;
        }
    }

    void Start()
    {
        entityManager = GameObject.FindGameObjectWithTag("EntityManager").GetComponent<EntityManager>();
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        messageManager = gameManager.GetComponent<MessageManager>();
        inventory = gameManager.GetComponent<InventoryManager>();
        movement = GameObject.FindGameObjectWithTag("Bonine").GetComponent<Movement>();
        StartCoroutine(Conversation());

        inventory.LoadUserInventory();
    }

    IEnumerator Conversation()
    {
        movement.allowMovement = false;
        yield return new WaitForSeconds(6.2f);

        messageManager.Edit("Master", new string[] {
            "Look who's back!",
            "Congrats for finding your way out of there.",
            "Move on, more mysteries await!"
        }, chatIcons);
    }

    public void LastMomentConvo()
    {
        movement.allowMovement = false;
        messageManager.Edit("Master", new string[] {
            "Good job!",
            "Sorry. It was my bad that you had to face this",
            "Anyways.. let's move on!"
        }, chatIcons);
    }

    public void TouchTrigger()
    {
        movement.allowMovement = false;
        messageManager.Edit("Master", new string[] {
            "Uh oh...",
            "Uhh.. I mistakely let them free.. the buckets..",
            "You uhh.. have to RUN!",
            "Listen to the blue arrows and DO NOT listen to the red arrows.",
            "Quick RUN!! Close the gates by flicking the lever, run!"
        }, chatIcons);
    }

    public void SpawnMoreBuckets(int index)
    {
        for (int i = 0; i < 25; i++)
        {
            Bucket bucket = entityManager.Spawn(EntityCode.Bucket, moreBuckets[index]).GetComponent<Bucket>();
            bucket.viewDistance = 1000;
            bucket.speed = 10f;
        }
    }

    public void SetDoorToClosed() => doormanager.doorIsOpened = true;

    public void SaveGame()
    {
        PlayerPrefs.SetInt("LevelsUnlocked", 2);

        inventory.SaveUserInventory();
        SceneManager.LoadScene("MainMenu");
    }
}
