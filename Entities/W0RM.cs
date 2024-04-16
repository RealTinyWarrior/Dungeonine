using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class W0RM : MonoBehaviour
{
    public string worm;
    public float speed = 3f;
    public float movementTime = 0.5f;
    public float standTime = 1f;
    public float viewRange = 7f;
    public float damageTick = 0.8f;
    public int startDamage;
    public int endDamage;
    [Space(10)]
    [Header("Digging Mechanism")]
    public GameObject knockbackObject;
    public float digDelay = 6f;
    public float digRange = 4f;
    public float digSpeed = 3f;
    public float digCooldown = 1.5f;
    Animator animator;
    NavMeshAgent agent;
    Health health;
    BonineHealth bonineHealth;
    Movement movement;
    EffectManager effect;
    HitEffect bonineHit;
    SpriteRenderer spriteRenderer;
    GameObject bonine;
    Transform bonineTransform;
    Vector2 digGizmos;
    Vector2 moveGizmos;
    bool isActive = true;
    bool doWalk = false;
    bool isHitting = false;
    float damageTimer = 0;
    float digTimer;

    enum Animation
    {
        Idle_Left,
        Idle_Right,
        Walk_Start_Left,
        Walk_End_Left,
        Walk_Start_Right,
        Walk_End_Right,
        Dig_Left,
        Dig_Right,
        Dig_Up_Left,
        Dig_Up_Right
    }

    Animation currentState = Animation.Idle_Right;
    Animation idleAnimation = Animation.Idle_Right;

    void Start()
    {
        bonineTransform = GameObject.FindGameObjectWithTag("BonineHitbox").GetComponent<Transform>();
        effect = GameObject.FindGameObjectWithTag("EffectManager").GetComponent<EffectManager>();
        bonine = GameObject.FindGameObjectWithTag("Bonine");
        movement = bonine.GetComponent<Movement>();
        bonineHealth = bonine.GetComponent<BonineHealth>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bonineHit = bonine.GetComponent<HitEffect>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;
        digTimer = digDelay;

        StartCoroutine(Movement());
    }

    void Update()
    {
        if (damageTimer >= 0) damageTimer -= Time.deltaTime;

        else if (isHitting)
        {
            bonineHealth.Damage(Random.Range(startDamage, endDamage));
            damageTimer = damageTick;
        }

        if (digTimer <= digDelay) digTimer += Time.deltaTime;
    }

    IEnumerator Movement()
    {
        while (true)
        {
            RaycastHit2D bonineRay = Physics2D.Raycast(transform.position, (bonineTransform.position - transform.position).normalized);
            float distance = Vector2.Distance(bonineTransform.position, transform.position);
            bool isInView = distance <= viewRange;
            digGizmos = bonineRay.point;

            if (bonineRay.distance <= digRange && digTimer >= digDelay && (bonineRay.collider.CompareTag("Bonine") || bonineRay.collider.CompareTag("BonineHitbox")) && !movement.isDead)
            {
                animator.speed = 1.2f / digSpeed;
                string direction = (transform.InverseTransformPoint(bonineTransform.position).x < 0) ? "Left" : "Right";
                Vector2 staticPosition = new Vector2(bonineTransform.position.x, bonineTransform.position.y);
                PlayDigAnimation(direction);

                digTimer = 0f;
                isActive = false;
                agent.isStopped = true;
                yield return new WaitForSeconds(digSpeed);
                spriteRenderer.color = new Color(1, 1, 1, 0);

                transform.position = staticPosition;
                spriteRenderer.color = new Color(1, 1, 1, 1);
                isActive = true;

                PlayDigOutAnimation(direction);

                Instantiate(knockbackObject, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(digCooldown);
                animator.speed = 1;
                agent.isStopped = false;

                StartCoroutine(StopW0RM());
                ChangeAnimationState(idleAnimation);
            }

            else if (doWalk && isInView && isActive)
            {
                string direction = (transform.InverseTransformPoint(bonineTransform.position).x < 0) ? "Left" : "Right";
                StartCoroutine(PlayStartAnimation(direction));

                yield return new WaitForSeconds(standTime + 0.1f * standTime);
                agent.SetDestination(bonineTransform.position);
                PlayEndAnimation(direction);

                yield return new WaitForSeconds(movementTime);
                agent.SetDestination(transform.position);
            }

            else if (!isInView && doWalk && isActive)
            {
                int willMove = Random.Range(0, 4);

                if (willMove == 0)
                {
                    Vector2 randomDirection = Random.insideUnitCircle.normalized;
                    RaycastHit2D ray = Physics2D.Raycast(transform.position, randomDirection);

                    if (ray.collider != null)
                    {
                        string direction = (transform.InverseTransformPoint(ray.point).x < 0) ? "Left" : "Right";
                        StartCoroutine(PlayStartAnimation(direction));

                        yield return new WaitForSeconds(standTime + 0.1f * standTime);

                        moveGizmos = ray.point;
                        agent.isStopped = false;
                        PlayEndAnimation(direction);
                        agent.SetDestination(ray.point);

                        yield return new WaitForSeconds(movementTime);
                        agent.SetDestination(transform.position);
                    }
                }

                else
                {
                    animator.speed = 1;
                    ChangeAnimationState(idleAnimation);
                    yield return new WaitForSeconds(1.5f);
                }
            }

            doWalk = doWalk != true;
        }
    }

    IEnumerator PlayStartAnimation(string direction)
    {
        yield return new WaitForSeconds(0.3f * standTime);
        animator.speed = 1 / standTime;

        if (direction == "Left")
        {
            ChangeAnimationState(Animation.Walk_Start_Left);
            idleAnimation = Animation.Idle_Left;
        }
        else
        {
            ChangeAnimationState(Animation.Walk_Start_Right);
            idleAnimation = Animation.Idle_Right;
        }
    }

    IEnumerator StopW0RM()
    {
        yield return new WaitForSeconds(movementTime);
        agent.SetDestination(transform.position);
        doWalk = true;
    }

    void PlayEndAnimation(string direction)
    {
        animator.speed = 0.8f / movementTime;

        if (direction == "Left")
        {
            ChangeAnimationState(Animation.Walk_End_Left);
            idleAnimation = Animation.Idle_Left;
            return;
        }

        ChangeAnimationState(Animation.Walk_End_Right);
        idleAnimation = Animation.Idle_Right;
    }

    void PlayDigAnimation(string direction)
    {
        if (direction == "Left")
        {
            ChangeAnimationState(Animation.Dig_Left);
            idleAnimation = Animation.Idle_Left;
            return;
        }

        ChangeAnimationState(Animation.Dig_Right);
        idleAnimation = Animation.Idle_Right;
    }

    void PlayDigOutAnimation(string direction)
    {
        if (direction == "Left")
        {
            ChangeAnimationState(Animation.Dig_Up_Left);
            idleAnimation = Animation.Idle_Left;
            return;
        }

        ChangeAnimationState(Animation.Dig_Up_Right);
        idleAnimation = Animation.Idle_Right;
    }

    void ChangeAnimationState(Animation newState)
    {
        if (newState == currentState) return;

        animator.Play(worm + "_" + newState.ToString());
        currentState = newState;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("BonineHitbox") && isActive)
        {
            isHitting = true;
            damageTimer = 0;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("BonineHitbox")) isHitting = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, digGizmos);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, moveGizmos);
    }
}
