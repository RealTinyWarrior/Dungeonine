using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [HideInInspector] public bool isUsedByKnockback;
    [HideInInspector] public Vector2 movement;
    Rigidbody2D rb;
    GlowManager glowManager;
    RotateOnDegree rotateOnDegree;
    Animator animator;
    public float speed = 2;
    public float animationSpeed = 1;
    public bool allowMovement = true;
    public bool isDead = false;
    public AudioSource walkingAudio;
    bool isPlayingAudio = false;

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
        StartCoroutine(BonineIdleStart());
        glowManager = GetComponent<GlowManager>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        rotateOnDegree = GameObject.FindGameObjectWithTag("GameManager").GetComponent<RotateOnDegree>();
        animator.speed = animationSpeed;
    }

    void Update()
    {
        // Doesn't allow Bonine to move if He's dead or inactive
        if (!allowMovement || isDead)
        {
            if (isPlayingAudio) walkingAudio.Stop();
            isPlayingAudio = false;

            ChangeAnimationState(idleState);
            movement = Vector2.zero;
            return;
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();

        if (rotationTimer > 0)
        {
            if (isPlayingAudio) walkingAudio.Stop();
            isPlayingAudio = false;

            rotationTimer -= Time.deltaTime;
            return;
        }

        // * Manages Movement Animation

        if (movement.x != 0 || movement.y != 0)
        {
            if (!isPlayingAudio) walkingAudio.Play();
            isPlayingAudio = true;

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

        else
        {
            ChangeAnimationState(idleState);
            if (isPlayingAudio) walkingAudio.Stop();

            isPlayingAudio = false;
        }
    }

    IEnumerator BonineIdleStart()
    {
        yield return new WaitForSeconds(0.1f);
        ChangeAnimationState(Animation.Bonine_Idle);
    }

    void FixedUpdate()
    {
        if (isUsedByKnockback) return;
        if (!allowMovement || isDead) rb.velocity = Vector2.zero;

        else rb.velocity = speed * movement;
    }

    public void ForceToIdleState()
    {
        if (isDead) return;

        animator.Play(idleState.ToString());
        if (glowManager != null) glowManager.ApplyChangeInGlow((int)currentState);

        currentState = idleState;
    }

    public void ChangeAnimationState(Animation newState)
    {
        if (isDead) return;
        if (newState == currentState) return;

        animator.Play(newState.ToString());
        currentState = newState;

        if (glowManager != null) glowManager.ApplyChangeInGlow((int)currentState);
    }

    public void LookAt(Vector2 position)
    {
        float degree = Vector2.SignedAngle(Vector2.right, position - (Vector2)transform.position);

        if (degree < 0)
        {
            degree = 360 + degree;
        }

        rotateOnDegree.Rotate(degree, 0.25f);

        StartCoroutine(MakeBonineIdle());
    }

    IEnumerator MakeBonineIdle()
    {
        yield return new WaitForSeconds(0.75f);
        ForceToIdleState();
    }
}
