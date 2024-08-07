using UnityEngine;

public class ItemController : MonoBehaviour
{
    GameManager gameManager;
    Camera mainCamera;
    GameObject bonine;
    public float attackDelay = 0.5f;
    [HideInInspector] public bool input = true;
    [HideInInspector] public float delay = 0;
    [HideInInspector] public Item item;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        bonine = GameObject.FindGameObjectWithTag("Bonine");
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (delay > 0) delay -= Time.deltaTime;
    }

    public void AddDelay() => delay = attackDelay;

    public Vector2 DirectionalUtility(bool hold = false)
    {
        if (gameManager == null) return Vector2.zero;
        input = true;

        if (!gameManager.UseUtility() || delay > 0)
        {
            input = false;
            return Vector2.zero;
        }

        hold = hold ? Input.GetMouseButton(item.mouseKey) : Input.GetMouseButtonDown(item.mouseKey);

        if (hold)
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - bonine.transform.position).normalized;

            return direction.normalized;
        }

        else
        {
            if (item.mouseKey != 0)
            {
                input = false;
                return Vector2.zero;
            }

            float horizontal = Input.GetAxis("RightStickHorizontal");
            float vertical = Input.GetAxis("RightStickVertical");

            if (horizontal == 0 && vertical == 0)
            {
                input = false;
                return Vector2.zero;
            }

            else return new Vector2(horizontal, vertical).normalized;
        }
    }

    public static float GetDegree(Vector2 direction)
    {
        float degree = Mathf.Atan2(direction.y, direction.x) * 57.295779f;
        if (degree < 0) degree = 360 - Mathf.Abs(degree);

        return degree;
    }
}
