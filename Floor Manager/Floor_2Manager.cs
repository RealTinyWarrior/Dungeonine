using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using URPGlitch.Runtime.AnalogGlitch;
using UnityEngine.Rendering.Universal;

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
    public Light2D globalLight;
    MessageManager messageManager;
    EntityManager entityManager;
    Movement movement;
    InventoryManager inventory;
    GameManager pauseManager;
    bool hasCompletedLevel = false;

    [Header("Glitch")]
    public Volume postProcessingVolume;
    AnalogGlitchVolume glitch;

    [Range(0, 1)]
    public float jitter;
    [Range(0, 1)]
    public float verticalJump;
    [Range(0, 1)]
    public float horizontalJump;
    [Range(0, 1)]
    public float colorDrift;

    public float minDirectionBlink;
    public float maxDirectionBlink;

    [Header("Blink Color")]
    public Color leftColor;
    public Color rightColor;

    void Start()
    {
        entityManager = GameObject.FindGameObjectWithTag("EntityManager").GetComponent<EntityManager>();
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        pauseManager = gameManager.GetComponent<GameManager>();
        messageManager = gameManager.GetComponent<MessageManager>();
        inventory = gameManager.GetComponent<InventoryManager>();
        movement = GameObject.FindGameObjectWithTag("Bonine").GetComponent<Movement>();

        if (PlayerPrefs.GetInt("LevelsUnlocked", 2) > 2)
        {
            hasCompletedLevel = true;
        }

        if (postProcessingVolume.profile.TryGet<AnalogGlitchVolume>(out var volume))
        {
            glitch = volume;
        }

        inventory.LoadUserInventory();
    }

    void Update()
    {
        if (messageManager.GetResolved("Close the gates by flicking the lever, RUN!!") || messageManager.GetResolved("You know the drill, run!"))
        {
            music.Play();
            backgroundMusic.Stop();
            pauseManager.bossFightOngoing = true;
            darkManager01.SetActive(false);
            StartCoroutine(SlowlySpawnBuckets());

            movement.speed = 6;
            movement.allowMovement = true;
            startLadder.SetActive(false);

            // Applying glitch effect
            glitch.scanLineJitter.value = jitter;
            glitch.verticalJump.value = verticalJump;
            glitch.horizontalShake.value = horizontalJump;
            glitch.colorDrift.value = colorDrift;
        }
    }

    public void TouchTrigger()
    {
        if (!hasCompletedLevel)
        {
            messageManager.Edit("Master", new string[] {
                "Uh oh.. The buckets, they escaped...",
                "I think you have to, run..",
                "When the blue light flashes, turn left. If the red light flashes, turn right.",
                "Close the gates by flicking the lever, RUN!!"
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

    public void ShowDirection(bool left)
    {
        StartCoroutine(ShowDirectionLighting(left));
    }

    IEnumerator ShowDirectionLighting(bool isLeft)
    {
        for (int i = 0; i < UnityEngine.Random.Range(7, 10); i++)
        {
            globalLight.color = isLeft ? leftColor : rightColor;
            yield return new WaitForSeconds(UnityEngine.Random.Range(minDirectionBlink, maxDirectionBlink));
            globalLight.color = Color.white;
            yield return new WaitForSeconds(UnityEngine.Random.Range(minDirectionBlink, maxDirectionBlink));
        }
    }

    public void SpawnMoreBuckets(int index) => StartCoroutine(SpawnBucket(moreBuckets[index], index > 3));
    public void SetDoorToClosed() => doormanager.doorIsOpened = true;

    // Slowly spawns bucket at the start of the match
    IEnumerator SlowlySpawnBuckets()
    {
        for (int i = 1; i <= 55; i++)
        {
            Bucket bucket = entityManager.Spawn(EntityCode.Bucket_No_Glow, i % 2 == 0 ? new Vector2(6.5f, 3.5f) : new Vector2(-5.2f, 3.5f)).GetComponent<Bucket>();
            bucket.willDespawn = true;
            bucket.despawnTimer = 30;

            bucket.viewDistance = 1000;
            bucket.speed = 10.5f;

            Health bucketHealth = bucket.GetComponent<Health>();
            bucketHealth.maxHealth = 100;
            bucketHealth.health = 100;

            yield return new WaitForSeconds(0.0005f);
        }
    }

    IEnumerator SpawnBucket(Vector2 position, bool finalBuckets)
    {
        for (int i = 1; i <= 17; i++)
        {
            Bucket bucket = entityManager.Spawn(EntityCode.Bucket_No_Glow, position).GetComponent<Bucket>();
            bucket.willDespawn = true;
            bucket.despawnTimer = finalBuckets ? 40 : 20;

            bucket.viewDistance = 1000;
            bucket.speed = 10.5f;

            Health bucketHealth = bucket.GetComponent<Health>();
            bucketHealth.maxHealth = 100;
            bucketHealth.health = 100;

            yield return new WaitForSeconds(0.003f);
        }
    }
}
