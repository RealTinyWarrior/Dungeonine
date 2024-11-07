using System.Collections;
using UnityEngine;

public class SteelEnergizersBullet : MonoBehaviour
{
    EffectManager effectManager;
    public float destroyTimer = 1.3f;
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public float speed;
    [HideInInspector] public int damage;
    [HideInInspector] public float degree;
    [HideInInspector] public Vector2 startPosition;
    bool isFirstCollider = true;
    Vector2 gizmosVector;

    void Start()
    {
        effectManager = GameObject.FindGameObjectWithTag("EffectManager").GetComponent<EffectManager>();
        transform.rotation = Quaternion.Euler(0, 0, degree);

        StartCoroutine(DestroyObjectOnTimeout(destroyTimer));
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;

        // Casts a ray to determine when to destroy the bullet

        RaycastHit2D[] rays = Physics2D.RaycastAll(
            new Vector2(transform.position.x, transform.position.y - 0.4f),
            GameManager.DegreeToVector2(degree),
            speed * destroyTimer
        );

        foreach (RaycastHit2D ray in rays)
        {
            if ((ray.collider.CompareTag("Wall") || !ray.collider.isTrigger) && !ray.collider.CompareTag("Bonine"))
            {
                gizmosVector = ray.point;

                float time = (ray.point - new Vector2(transform.position.x, transform.position.y - 0.4f)).magnitude / speed;
                StartCoroutine(DestroyObjectOnTimeout(time));

                break;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bonine")) return;

        if (col.CompareTag("Entity"))
        {
            effectManager.Create(EffectCode.BlastEffect, 0.1f, (Vector2)col.transform.position + col.GetComponent<HitEffect>().effectOffset, Color.yellow, 4.5f, 0);

            Health health = col.GetComponent<Health>();
            health.Damage(damage);

            Destroy(gameObject);
        }

        else if (col.CompareTag("Wall") || col.isTrigger == false)
        {
            if (isFirstCollider) return;
            isFirstCollider = false;

            effectManager.Create(EffectCode.BlastEffect, 0.1f, new Vector2(transform.position.x, transform.position.y), Color.yellow, 4.5f, 10);
            Destroy(gameObject);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, gizmosVector);
    }

    IEnumerator DestroyObjectOnTimeout(float timer)
    {
        yield return new WaitForSeconds(timer);
        DestroyBullet();
    }

    public void DestroyBullet()
    {
        effectManager.Create(EffectCode.BlastEffect, 0.1f, new Vector2(transform.position.x, transform.position.y), Color.yellow, 4.5f, 10);

        Destroy(gameObject);
    }
}