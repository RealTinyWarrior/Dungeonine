using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Bucket : MonoBehaviour
{
    public float speed;
    public float viewDistance;
    public int startDamage;
    public int endDamage;
    public float damageDelay = 0.3f;
    public float knockbackStrength = 2;
    public float knockbackDuration = 0.2f;
    Animator animator;
    GameObject bonine;
    BonineHealth bonineHealth;
    NavMeshAgent agent;
    Knockback knockback;
    Knockback bonineKnockback;
    Movement movement;
    bool isColliding;
    float timer;

    void Update()
    {
        if (isColliding)
        {
            if (timer >= damageDelay)
            {
                Damage();
                timer = 0;
            }

            else timer += Time.deltaTime;
        }
    }

    void Start()
    {
        bonine = GameObject.FindGameObjectWithTag("Bonine");
        bonineHealth = bonine.GetComponent<BonineHealth>();
        bonineKnockback = bonine.GetComponent<Knockback>();
        movement = bonineHealth.GetComponent<Movement>();
        agent = GetComponent<NavMeshAgent>();
        knockback = GetComponent<Knockback>();
        animator = GetComponent<Animator>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;
        timer = damageDelay;

        StartCoroutine(FindPath());
        animator.speed = speed / 2.8f;
    }

    IEnumerator FindPath()
    {
        while (true)
        {
            if (Vector2.Distance(bonine.transform.position, transform.position) <= viewDistance && !movement.isDead)
            {
                if (agent.enabled) agent.SetDestination(bonine.transform.position);

                yield return new WaitForSeconds(0.25f);
            }

            else
            {
                int willMove = Random.Range(0, 2);

                if (willMove == 0)
                {
                    Vector2 randomDirection = Random.insideUnitCircle.normalized;
                    RaycastHit2D ray = Physics2D.Raycast(transform.position, randomDirection, 2.5f);

                    agent.SetDestination(ray.point);
                    yield return new WaitForSeconds(0.5f);
                    agent.SetDestination(transform.position);

                }
            }
        }
    }

    void Damage()
    {
        Vector2 knockbackDirection = ((Vector2)(bonine.transform.position - transform.position)).normalized;
        Vector2 direction = ((Vector2)(transform.position - bonine.transform.position)).normalized;

        if (knockbackDirection.x == 0 && knockbackDirection.y == 0)
        {
            knockbackDirection = Random.insideUnitCircle.normalized;
            direction = new Vector2(-knockbackDirection.x, -knockbackDirection.y);
        }

        bonineHealth.Damage(Random.Range(startDamage, endDamage));
        if (!knockback.isKnocking) knockback.ApplyKnockback(direction, 6.2f, 0.9f);
        bonineKnockback.ApplyKnockback(knockbackDirection, knockbackStrength, knockbackDuration);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bonine"))
        {
            isColliding = true;
            Damage();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Bonine"))
        {
            isColliding = false;
        }
    }
}
