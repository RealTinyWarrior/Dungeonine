using UnityEngine;

public class SteelEnergizersBullet : MonoBehaviour
{
    EffectManager effectManager;
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public float speed;
    [HideInInspector] public int damage;
    [HideInInspector] public float degree;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;

        effectManager = GameObject.FindGameObjectWithTag("EffectManager").GetComponent<EffectManager>();
        transform.rotation = Quaternion.Euler(0, 0, degree);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bonine")) return;

        if (col.CompareTag("Entity"))
        {
            effectManager.Create(EffectCode.BlastEffect, 0.1f, (Vector2)col.transform.position + col.GetComponent<HitEffect>().effectOffset, 4.5f);

            Health health = col.GetComponent<Health>();
            health.Damage(damage);
            Destroy(gameObject);
        }

        else if (col.CompareTag("Wall") || col.isTrigger == false)
        {
            effectManager.Create(EffectCode.BlastEffect, 0.1f, new Vector2(transform.position.x, transform.position.y + 0.2f), 4.5f);
            Destroy(gameObject);
        }
    }
}