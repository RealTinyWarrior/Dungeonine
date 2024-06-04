using UnityEngine;

public class SlashRotate : MonoBehaviour
{
    public float activeFor = 0.2f;
    public GameObject slashChild;
    public Transform maskTransform;
    public AudioSource slashAudio;
    public int minEnergyCost = 2;
    public int maxEnergyCost = 5;
    BonineEnergy bonineEnergy;
    InventoryManager inventory;
    GameManager gameManager;
    RotateOnDegree rotateOnDegree;
    GameObject bonine;
    Camera mainCamera;
    float activeTimer;
    Item item;

    void Start()
    {
        GameObject gameManagerObject = GameObject.FindGameObjectWithTag("GameManager");
        item = GetComponent<ItemParam>().item;

        rotateOnDegree = gameManagerObject.GetComponent<RotateOnDegree>();
        inventory = gameManagerObject.GetComponent<InventoryManager>();
        gameManager = gameManagerObject.GetComponent<GameManager>();
        maskTransform.localPosition = new Vector2(0.551f, -1.1f);
        bonine = GameObject.FindGameObjectWithTag("Bonine");
        bonineEnergy = bonine.GetComponent<BonineEnergy>();
        mainCamera = Camera.main;
        activeTimer = activeFor;
    }

    void Update()
    {
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

        if (Input.GetMouseButtonDown(item.mouseKey))
        {
            if (gameManager.UseUtility())
            {
                slashAudio.Play();

                Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector2 direction = (mousePosition - bonine.transform.position).normalized;
                maskTransform.localPosition = new Vector2(0.551f, -1.1f);

                float degree = Mathf.Atan2(direction.y, direction.x) * 57.295779f;
                if (degree < 0) degree = 360 - Mathf.Abs(degree);

                rotateOnDegree.Rotate(degree, inventory.attackDelay);

                transform.rotation = Quaternion.Euler(0, 0, degree);
                slashChild.SetActive(true);
                activeTimer = 0;

                bonineEnergy.DecreaseEnergy(Random.Range(minEnergyCost, maxEnergyCost + 1));
                inventory.AddGlobalDelay();
            }
        }

        else if (item.mouseKey == 0)
        {
            if (gameManager.UseUtility())
            {
                float horizontal = Input.GetAxis("RightStickHorizontal");
                float vertical = Input.GetAxis("RightStickVertical");
                if (horizontal == 0 && vertical == 0) return;

                slashAudio.Play();
                Vector2 direction = new(horizontal, vertical);
                maskTransform.localPosition = new Vector2(0.551f, -1.1f);

                float degree = Mathf.Atan2(direction.y, direction.x) * 57.295779f;
                if (degree < 0) degree = 360 - Mathf.Abs(degree);

                rotateOnDegree.Rotate(degree, inventory.attackDelay);

                transform.rotation = Quaternion.Euler(0, 0, degree);
                slashChild.SetActive(true);
                activeTimer = 0;

                bonineEnergy.DecreaseEnergy(Random.Range(minEnergyCost, maxEnergyCost + 1));
                inventory.AddGlobalDelay();
            }
        }
    }
}
