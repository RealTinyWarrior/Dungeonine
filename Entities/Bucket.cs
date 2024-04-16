using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Bucket : MonoBehaviour
{
    public float speed;
    public float viewDistance;
    public int startDamage;
    public int endDamage;
    public float knockbackStrength = 2;
    public float knockbackDuration = 0.2f;
    Animator animator;
    GameObject bonine;
    BonineHealth bonineHealth;
    NavMeshAgent agent;
    Knockback knockback;
    Knockback bonineKnockback;
    Movement movement;

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

        StartCoroutine(FindPath());
        animator.speed = speed / 2.8f;
    }

    IEnumerator FindPath()
    {
        while (true)
        {
            if (Vector2.Distance(bonine.transform.position, transform.position) <= viewDistance && !movement.isDead)
            {
                agent.SetDestination(bonine.transform.position);
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

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("BonineHitbox"))
        {
            bonineHealth.Damage(Random.Range(startDamage, endDamage));
            knockback.ApplyKnockback((Vector2)(transform.position - bonine.transform.position), 5, 0.7f);
            bonineKnockback.ApplyKnockback((Vector2)(bonine.transform.position - transform.position), knockbackStrength, knockbackDuration);
        }
    }
}
