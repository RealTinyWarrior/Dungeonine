using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Floor_3Manager : MonoBehaviour
{
    public Sprite[] chatIcons;
    public Sprite emptySprite;
    public GameObject startConversationTrigger;
    public GameObject darkManager;
    public Image glitchImage;
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
    }

    void Update()
    {
        //* Lilyth's Toy

        bool[] toyAnswer = messageManager.GetAnswer("Toy Question");

        if (toyAnswer[0])
        {
            startConversationDone = true;

            if (toyAnswer[1])
            {
                saidNo = false;

                messageManager.Edit("Lilyth", new string[] {
                    "Thank you for your kind words!",
                    "He must be somewhere in this.. maze.",
                    "I hope that he is alright.."
                }, chatIcons);

                startConversationTrigger.SetActive(false);
            }

            else StartCoroutine(JumpscareCoroutine());
        }

        if (messageManager.GetResolved("I hope that he is alright.."))
        {
            bonineMovement.allowMovement = true;
            darkManager.SetActive(false);

            SpawnEntities();
        }

        //* Dome's Diamond

        bool[] domeAnswer = messageManager.GetAnswer("Dome Question");

        if (domeAnswer[0])
        {
            if (domeAnswer[1])
            {
                int inventoryIndex = inventory.ItemIndexOnInventory((int)ItemCode.SuspiciousCrystal);

                if (inventoryIndex == -1)
                {
                    messageManager.Edit("Dome", new string[] {
                        "Your lies ain't workin' on me bud.",
                        "go Find it, then I'm allowing you to go in."
                    }, chatIcons);

                    bonine.transform.position = new Vector2(bonine.transform.position.x + 0.1f, bonine.transform.position.y);
                }

                else
                {
                    StartCoroutine(DomeLaugh());

                    foundOrb = true;
                    messageManager.Edit("Dome", new string[] {
                        "Thanks a lot mate!",
                        "For this, Imma reward you with these Steel Energizers.",
                        "Good luck on your adventure my guy."
                    }, chatIcons);

                    inventory.RemoveItem(inventory.ItemIndexOnInventory((int)ItemCode.SuspiciousCrystal));
                    inventory.AddItemOnBonine(ItemCode.SteelEnergizers, 1);
                }
            }

            else
            {
                bonine.transform.position = new Vector2(bonine.transform.position.x + 0.1f, bonine.transform.position.y);

                messageManager.Edit("Dome", new string[] {
                   "Go ahead and find it lil' man."
                }, chatIcons);
            }
        }
    }

    public void SomethingBlocking()
    {
        if (startConversationDone && !saidNo) return;


        messageManager.Edit("Interact", new string[] {
            "Something is stopping you to go there."
        }, new Sprite[] { emptySprite });

        bonine.transform.position = new Vector2(bonine.transform.position.x, bonine.transform.position.y - 0.2f);
    }

    public void LilythConversation01()
    {
        if (saidNo)
        {
            messageManager.Edit("Lilyth", new string[] {
                "Cupcake, are you alright?",
                "<choice>",
                "Toy Question",
                "Can you help me find him?"
            }, chatIcons);

            return;
        }

        if (startConversationDone)
        {
            int itemIndex = inventory.ItemIndexOnInventory((int)ItemCode.Cupcake);

            if (itemIndex != -1)
            {
                messageManager.Edit("Lilyth", new string[] {
                    "Cupcake!! You are back!",
                    "Appreciate your help, my friend.. While you were away, I found this in the dungeon.",
                    "A crystal, you may find this useful."
                }, chatIcons);

                sadLilyth.SetActive(false);
                happyLilyth.SetActive(true);

                inventory.RemoveItem(itemIndex);
                inventory.AddItemOnBonine(ItemCode.SuspiciousCrystal, 1);
                dollObject.SetActive(true);
                dollObject.transform.position = new Vector2(11.12f, -2.1f);

                heartBeat.Stop();
                return;
            }

            messageManager.Edit("Lilyth", new string[] {
                "I hope that he is alright.."
            }, chatIcons);

            return;
        }


        messageManager.Edit("Lilyth", new string[] {
            "*Weeps*",
            "Where are you cupcake?",
            "...",
            "I lost my toy.. and I cannot find it...",
            "<choice>",
            "Toy Question",
            "Can you help me find him?"
        }, chatIcons);
    }

    public void TriggerHeartBeat()
    {
        if (!dollBattleDone) heartBeat.Play();
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

    public void DomeConversation1()
    {
        if (foundOrb) return;

        if (domeConversationDone)
        {
            bonine.transform.position = new Vector2(bonine.transform.position.x + 0.1f, bonine.transform.position.y);

            messageManager.Edit("Dome", new string[] {
                "Hoy!",
                "<choice>",
                "Dome Question",
                "Have you found the crystal?",
            }, chatIcons);

            return;
        }

        messageManager.Edit("Dome", new string[] {
            "Yo, whaddup lil' man.",
            "<choice>",
            "Dome Question",
            "Have you happened to see a crystal lying around here?",
        }, chatIcons);

        domeConversationDone = true;
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

    public void LastJumpscare() => StartCoroutine(LastJumpscareCoroutine());

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

    IEnumerator LastJumpscareCoroutine()
    {
        jumpscareAudio.Play();

        saidNo = true;
        glitchImage.DOFade(0.5f, 0.4f);

        yield return new WaitForSeconds(0.4f);
        bonine.transform.position = new Vector2(-123, 25);
        glitchImage.DOFade(0f, 0.4f);

        yield return new WaitForSeconds(0.18f);
        jumpscareAudio.Stop();
    }

    IEnumerator DomeLaugh()
    {
        domeAnimation.Play("Dome_Laugh");
        yield return new WaitForSeconds(1f);
        domeAnimation.Play("Dome__Breath");
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
        glitchImage.DOFade(0.2f, 0.5f);
        faceImage.DOFade(0.9f, 0.5f);

        yield return new WaitForSeconds(0.5f);
        glitchImage.DOFade(0f, 0.5f);
        faceImage.DOFade(0f, 0.5f);

        yield return new WaitForSeconds(0.3f);
        jumpscareAudio.Stop();
    }
}
