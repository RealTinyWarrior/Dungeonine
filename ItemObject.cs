using System;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public int id;
    public int amount;
    public float speed = 1;
    public float timer = 0;
    public float itemSlideT = 0.4f;
    public bool isObtainable = false;
    bool isColliding = false;
    Rigidbody2D rb;
    Transform bonineTransform;
    InventoryManager inventoryManager;
    GameObject bonine;
    GameItems gameItems;
    float checkTimer = 0f;

    void Start()
    {
        GameObject gameManagerObject = GameObject.FindGameObjectWithTag("GameManager");
        inventoryManager = gameManagerObject.GetComponent<InventoryManager>();
        bonine = GameObject.FindGameObjectWithTag("Bonine");
        bonineTransform = bonine.GetComponent<Transform>();
        gameItems = gameManagerObject.GetComponent<GameItems>();
        rb = GetComponent<Rigidbody2D>();

        if (inventoryManager.ItemIndexOnInventory(id) != -1) isObtainable = true;
        else isObtainable = inventoryManager.hasStorage;

        Physics2D.IgnoreCollision(bonine.GetComponent<BoxCollider2D>(), gameObject.GetComponent<BoxCollider2D>());
    }

    void Update()
    {
        if (timer > 0) timer -= Time.deltaTime;
        if (itemSlideT > 0) itemSlideT -= Time.deltaTime;
        else rb.velocity = Vector2.zero;

        if (checkTimer < 0.2f) checkTimer += Time.deltaTime;

        else
        {
            if (inventoryManager.ItemIndexOnInventory(id) != -1) isObtainable = true;
            else isObtainable = inventoryManager.hasStorage;

            if (!isObtainable) rb.velocity = Vector2.zero;
            if (!isObtainable && timer <= 0) return;

            if (Vector2.Distance(bonineTransform.position, transform.position) < 2.5f) isColliding = true;
            else isColliding = false;
            checkTimer = 0f;
        }

        if (isColliding && isObtainable && timer <= 0)
        {
            Vector2 distance = bonineTransform.position - transform.position;
            double degree = Mathf.Atan2(distance.y, distance.x) * 57.295779;

            if (degree < 0) degree = 360 - Math.Abs(degree);
            float xVelocity = Convert.ToSingle(Math.Cos(degree * 0.017453));
            float yVelocity = Convert.ToSingle(Math.Sin(degree * 0.017453));

            rb.velocity = new Vector2(xVelocity, yVelocity) * speed;
        }
    }
}