using UnityEngine;

public class SlashRotate : MonoBehaviour
{
    public float activeFor = 0.2f;
    public GameObject slashChild;
    public Transform maskTransform;
    public AudioSource slashAudio;
    ItemController itemController;
    RotateOnDegree rotateOnDegree;
    float activeTimer;
    Item item;

    void Start()
    {
        GameObject gameManagerObject = GameObject.FindGameObjectWithTag("GameManager");
        rotateOnDegree = gameManagerObject.GetComponent<RotateOnDegree>();
        maskTransform.localPosition = new Vector2(0.551f, -1.1f);
        activeTimer = activeFor;

        itemController = GetComponent<ItemController>();
        item = GetComponent<ItemParam>().item;
        itemController.item = item;
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

        Vector2 direction = itemController.DirectionalUtility();
        if (!itemController.input) return;

        maskTransform.localPosition = new Vector2(0.551f, -1.1f);
        float degree = ItemController.GetDegree(direction);

        rotateOnDegree.Rotate(degree, itemController.attackDelay);
        transform.rotation = Quaternion.Euler(0, 0, degree);
        slashChild.SetActive(true);
        slashAudio.Play();
        activeTimer = 0;

        itemController.AddDelay();
    }

    void OnDisable()
    {
        maskTransform.localPosition = new Vector2(0.551f, -1.1f);
        slashChild.SetActive(false);
        activeTimer = activeFor;
    }
}
