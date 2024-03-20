using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody2D rb;
    public Vector2 movement;
    Animator animator;
    public float speed = 2;
    public bool allowMovement = true;

    [HideInInspector]
    public enum Animation
    {
        Bonine_Idle,
        Bonine_Idle_Left,
        Bonine_Idle_Right,
        Bonine_Idle_Back,
        Bonine_Walk_Left,
        Bonine_Walk_Right,
        Bonine_Walk_Front,
        Bonine_Walk_Back,
    }

    Animation currentState = Animation.Bonine_Idle;
    public Animation idleState = Animation.Bonine_Idle;
    [HideInInspector] public float rotationTimer = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!allowMovement)
        {
            ChangeAnimationState(idleState);
            movement = Vector2.zero;
            return;
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (rotationTimer > 0)
        {
            rotationTimer -= Time.deltaTime;
            return;
        }

        if (movement.x != 0 || movement.y != 0)
        {
            if (movement.x > 0)
            {
                ChangeAnimationState(Animation.Bonine_Walk_Right);
                idleState = Animation.Bonine_Idle_Right;
            }
            else if (movement.x < 0)
            {
                ChangeAnimationState(Animation.Bonine_Walk_Left);
                idleState = Animation.Bonine_Idle_Left;
            }
            else if (movement.y > 0)
            {
                ChangeAnimationState(Animation.Bonine_Walk_Back);
                idleState = Animation.Bonine_Idle_Back;
            }
            else
            {
                ChangeAnimationState(Animation.Bonine_Walk_Front);
                idleState = Animation.Bonine_Idle;
            }
        }

        else ChangeAnimationState(idleState);
        movement.Normalize();
    }

    void FixedUpdate()
    {
        if (!allowMovement) rb.velocity = Vector2.zero;

        else rb.velocity = speed * movement;
    }

    public void ChangeAnimationState(Animation newState)
    {
        if (newState == currentState) return;

        animator.Play(newState.ToString());
        currentState = newState;
    }
}
