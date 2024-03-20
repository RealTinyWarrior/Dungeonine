using UnityEngine;

public class IronSword : MonoBehaviour
{
    public float activeFor = 0.2f;
    public GameObject slashChild;
    public Transform maskTransform;
    InventoryManager inventory;
    Movement bonineMovement;
    RotateOnDegree rotateOnDegree;
    GameObject bonine;
    Camera mainCamera;
    float activeTimer;
    Item item;

    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InventoryManager>();
        maskTransform.localPosition = new Vector2(0.551f, -1.1f);
        bonine = GameObject.FindGameObjectWithTag("Bonine");
        rotateOnDegree = GameObject.FindGameObjectWithTag("GameManager").GetComponent<RotateOnDegree>();
        mainCamera = Camera.main;
        activeTimer = activeFor;

        bonineMovement = GameObject.FindGameObjectWithTag("Bonine").GetComponent<Movement>();
        item = GetComponent<ItemParam>().item;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(item.mouseKey) && inventory.delay <= 0)
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - bonine.transform.position).normalized;
            maskTransform.localPosition = new Vector2(0.551f, -1.1f);

            float degree = Mathf.Atan2(direction.y, direction.x) * 57.295779f;
            if (degree < 0) degree = 360 - Mathf.Abs(degree);

            rotateOnDegree.Rotate(degree, inventory.attackDelay);

            transform.rotation = Quaternion.Euler(0, 0, degree);
            slashChild.SetActive(true);
            activeTimer = 0;

            inventory.AddGlobalDelay();
        }

        if (activeTimer < activeFor)
        {
            activeTimer += Time.deltaTime;
            maskTransform.localPosition = new Vector2(0.551f, maskTransform.localPosition.y + Time.deltaTime * 10);

            if (activeTimer >= activeFor)
            {
                slashChild.SetActive(false);
                maskTransform.localPosition = new Vector2(0.551f, -1.1f);
            }
        }
    }
}