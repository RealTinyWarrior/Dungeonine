using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using URPGlitch.Runtime.AnalogGlitch;

public class Floor_3Manager : MonoBehaviour
{
    public Sprite[] chatIcons;
    public Sprite emptySprite;
    public GameObject startConversationTrigger;
    public GameObject darkManager;
    public Image faceImage;
    public AudioSource dollMusic;
    public AudioSource heartBeat;
    public AudioSource jumpscareAudio;
    public GameObject dollObject;
    public Tilemap tilemap;
    public TileBase wall0;
    public TileBase wall1;
    public Animator domeAnimation;
    public GameObject dollInteraction;
    public Vector2[] spawnPoints;
    public GameObject sadLilyth;
    public GameObject happyLilyth;
    public Volume postProcessingVolume;
    public Sprite interactionIcon;
    AnalogGlitchVolume glitch;
    EntityManager entityManager;
    InventoryManager inventory;
    BonineHealth bonineHealth;
    MessageManager messageManager;
    GameObject bonine;
    GameManager gameManager;
    Movement bonineMovement;
    List<GameObject> spawnedEntities = new();
    bool startConversationDone = false;
    bool dollBattleDone = false;
    bool saidNo = false;
    bool domeConversationDone = false;
    bool foundOrb = false;
    bool firstConvoDone = false;

    void Start()
    {
        GameObject gameManagerObject = GameObject.FindGameObjectWithTag("GameManager");
        gameManager = gameManagerObject.GetComponent<GameManager>();

        bonine = GameObject.FindGameObjectWithTag("Bonine");
        messageManager = gameManagerObject.GetComponent<MessageManager>();
        bonineMovement = bonine.GetComponent<Movement>();

        bonineHealth = bonine.GetComponent<BonineHealth>();
        inventory = gameManagerObject.GetComponent<InventoryManager>();
        entityManager = GameObject.FindGameObjectWithTag("EntityManager").GetComponent<EntityManager>();
        inventory.LoadUserInventory();

        if (postProcessingVolume.profile.TryGet<AnalogGlitchVolume>(out var volume))
        {
            glitch = volume;
        }
    }

    void Update()
    {
        // * Lilyth's Toy Question Manager

        if (messageManager.GetResolved("Appreciate your help, my friend.. While you were away, I found this in the dungeon."))
        {
            inventory.AddItemOnBonine(ItemCode.SuspiciousCrystal, 1);
        }

        bool[] toyAnswer = messageManager.GetAnswer("Toy Question");

        if (toyAnswer[0])
        {
            startConversationDone = true;

            // Lilyth's dialogue when you accept her request
            if (toyAnswer[1])
            {
                saidNo = false;

                messageManager.Edit("Lilyth", new string[] {
                    "Thank you for your kind words! He must be somewhere in this.. maze.",
                    "I hope that he is alright.."
                }, chatIcons, 2, 1);

                startConversationTrigger.SetActive(false);
            }

            else StartCoroutine(JumpscareCoroutine());
        }

        if (messageManager.GetResolved("I hope that he is alright.."))
        {
            if (firstConvoDone) return;

            bonineMovement.allowMovement = true;
            darkManager.SetActive(false);
            firstConvoDone = true;

            SpawnEntities();
        }

        //* Dome's Diamond Question Manager

        bool[] domeAnswer = messageManager.GetAnswer("Dome Question");

        if (domeAnswer[0])
        {
            if (domeAnswer[1])
            {
                int inventoryIndex = inventory.ItemIndexOnInventory((int)ItemCode.SuspiciousCrystal);

                // Dome's dialogue when you lie about having the crystal
                if (inventoryIndex == -1)
                {
                    messageManager.Edit("Dome", new string[] {
                        "Your lies ain't workin' on me bud.",
                        "Go find it, then I'm allowing you to go in."
                    }, chatIcons, 4, 2);

                    bonine.transform.position = new Vector2(bonine.transform.position.x + 0.1f, bonine.transform.position.y);
                }

                // Dome's dialogue when you give him the crystal
                else
                {
                    StartCoroutine(DomeLaugh());

                    foundOrb = true;
                    messageManager.Edit("Dome", new string[] {
                        "Thanks a lot mate! Here, keep these Steel Energizers.",
                        "Good luck on your adventure my guy."
                    }, chatIcons, 4, 2);

                    inventory.RemoveItem(inventory.ItemIndexOnInventory((int)ItemCode.SuspiciousCrystal));
                    inventory.AddItemOnBonine(ItemCode.SteelEnergizers, 1);
                }
            }

            // When you try to bypass Dome without giving him the crystal
            else
            {
                bonine.transform.position = new Vector2(bonine.transform.position.x + 0.1f, bonine.transform.position.y);

                messageManager.Edit("Dome", new string[] {
                   "Go ahead and find it."
                }, chatIcons, 4, 2);
            }
        }
    }

    // Pushes Bonine back when he tries to escape without having a conversation
    public void SomethingBlocking()
    {
        if (startConversationDone && !saidNo) return;

        messageManager.Edit("interact", new string[] {
            "Something is stopping you from going there."
        }, new Sprite[] { interactionIcon });

        bonine.transform.position = new Vector2(bonine.transform.position.x, bonine.transform.position.y - 0.2f);
    }

    // * Lilyth's Dialogue

    public void LilythConversation01()
    {
        if (happyLilyth.activeInHierarchy) bonineMovement.LookAt(happyLilyth.transform.position);
        else if (sadLilyth.activeInHierarchy) bonineMovement.LookAt(sadLilyth.transform.position);

        if (saidNo)
        {
            // Lilyth's dialogue after the first time you reject her request
            messageManager.Edit("Lilyth", new string[] {
                "Cupcake, are you alright?",
                "<choice>",
                "Toy Question",
                "Can you help me find him?"
            }, chatIcons, 2, 1);

            return;
        }

        if (startConversationDone)
        {
            int itemIndex = inventory.ItemIndexOnInventory((int)ItemCode.Cupcake);

            if (itemIndex != -1)
            {
                // Lilyth's dialogue when you find cupcake
                messageManager.Edit("Lilyth", new string[] {
                    "Cupcake!! You are back!",
                    "Appreciate your help, my friend.. While you were away, I found this in the dungeon.",
                    "A crystal, you may find this useful."
                }, chatIcons, 3, 1);

                sadLilyth.SetActive(false);
                happyLilyth.SetActive(true);

                inventory.RemoveItem(itemIndex);
                dollObject.SetActive(true);
                dollObject.transform.position = new Vector2(11.12f, -2.1f);

                heartBeat.Stop();
                return;
            }

            messageManager.Edit("Lilyth", new string[] {
                "I hope that he is alright.."
            }, chatIcons, 2, 1);

            return;
        }


        // Lilyth's first conversation with Bonine
        messageManager.Edit("Lilyth", new string[] {
            "*Weeps*",
            "Where are you cupcake?",
            "...",
            "I lost my toy.. and I cannot find it...",
            "<choice>",
            "Toy Question",
            "Can you help me find him?"
        }, chatIcons, 1, 1);
    }

    public void OnDollPickup()
    {
        dollInteraction.SetActive(false);
        dollMusic.Play();
        dollObject.SetActive(false);
        inventory.AddItemOnBonine(ItemCode.Cupcake, 1);

        tilemap.SetTile(new Vector3Int(105, 35, 0), wall1);
        tilemap.SetTile(new Vector3Int(105, 34, 0), wall0);

        StartCoroutine(SpawnShowdown());
        gameManager.bossFightOngoing = true;
    }

    public void TriggerHeartBeat()
    {
        if (!dollBattleDone) heartBeat.Play();
    }

    IEnumerator SpawnShowdown()
    {
        int spawned = 0;
        bonineMovement.speed = 4;

        while (spawned < 16)
        {
            if (Random.Range(0, 3) != 0)
            {
                GameObject bucket = entityManager.Spawn(EntityCode.Bucket, Random.Range(0, 2) == 0 ? new Vector2(101, 42) : new Vector2(110, 42));
                spawnedEntities.Add(bucket);
            }

            else
            {
                GameObject worm = entityManager.Spawn(Random.Range(0, 2) == 0 ? EntityCode.W0RM_B : EntityCode.W0RM_A, Random.Range(0, 2) == 0 ? new Vector2(96, 37) : new Vector2(114, 37));
                spawnedEntities.Add(worm);
            }

            spawned++;
            yield return new WaitForSeconds(Random.Range(0.4f, 2));
        }

        StartCoroutine(CheckForBattleEnd());
    }

    void SpawnEntities()
    {
        foreach (Vector2 position in spawnPoints)
        {
            entityManager.Spawn(
                Random.Range(0, 2) == 0 ? EntityCode.Bucket : Random.Range(0, 2) == 0 ? EntityCode.W0RM_A : EntityCode.W0RM_B,
                position
            );
        }
    }

    public void LastJumpscare() => StartCoroutine(LastJumpscareCoroutine());

    IEnumerator LastJumpscareCoroutine()
    {
        jumpscareAudio.Play();

        saidNo = true;
        glitch.scanLineJitter.value = 0.76f;
        glitch.colorDrift.value = 1f;
        glitch.verticalJump.value = 0.3f;
        glitch.horizontalShake.value = 0.2f;

        yield return new WaitForSeconds(0.4f);
        bonine.transform.position = new Vector2(-123, 25);

        glitch.scanLineJitter.value = 0f;
        glitch.colorDrift.value = 0f;
        glitch.verticalJump.value = 0f;
        glitch.horizontalShake.value = 0f;

        yield return new WaitForSeconds(0.18f);
        jumpscareAudio.Stop();
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

        tilemap.SetTile(new Vector3Int(105, 35, 0), null);
        tilemap.SetTile(new Vector3Int(105, 34, 0), null);
        bonineMovement.speed = 2.6f;
        dollMusic.Stop();
        gameManager.bossFightOngoing = false;
    }

    IEnumerator JumpscareCoroutine()
    {
        bonine.transform.position = new Vector2(1.2f, -1.2f);
        bonineHealth.Damage(Random.Range(57, 68), 0.0005f);
        jumpscareAudio.Play();

        saidNo = true;
        glitch.scanLineJitter.value = 0.76f;
        glitch.colorDrift.value = 1f;
        glitch.verticalJump.value = 0.3f;
        glitch.horizontalShake.value = 0.2f;
        faceImage.DOFade(0.9f, 0.5f);

        yield return new WaitForSeconds(0.5f);
        glitch.scanLineJitter.value = 0f;
        glitch.colorDrift.value = 0f;
        glitch.verticalJump.value = 0f;
        glitch.horizontalShake.value = 0f;
        faceImage.DOFade(0f, 0.5f);

        yield return new WaitForSeconds(0.3f);
        jumpscareAudio.Stop();
    }

    // * Dome's Dialogue

    public void DomeConversation1()
    {
        bonineMovement.LookAt(new Vector2(-56.28f, 21.39f));
        if (foundOrb) return;

        if (domeConversationDone)
        {
            bonine.transform.position = new Vector2(bonine.transform.position.x + 0.1f, bonine.transform.position.y);

            // Dome when you talk with him after 'the crystal search'
            messageManager.Edit("Dome", new string[] {
                "Hoy!",
                "<choice>",
                "Dome Question",
                "Have you found the crystal?",
            }, chatIcons, 4, 2);

            return;
        }

        // Dome's first conversation
        messageManager.Edit("Dome", new string[] {
            "Yo, whaddup lil' guy.",
            "<choice>",
            "Dome Question",
            "Have you happened to see a crystal lying around here?",
        }, chatIcons, 4, 2);

        domeConversationDone = true;
    }

    IEnumerator DomeLaugh()
    {
        domeAnimation.Play("Dome_Laugh");
        yield return new WaitForSeconds(1f);
        domeAnimation.Play("Dome__Breath");
    }
}
