using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    public int minDamage = 2;
    public int maxDamage = 6;
    EffectManager effectManager;

    void Start()
    {
        effectManager = GameObject.FindGameObjectWithTag("EffectManager").GetComponent<EffectManager>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Entity"))
        {
            effectManager.Create(EffectCode.BlastEffect, 0.1f, (Vector2)col.transform.position + col.GetComponent<HitEffect>().effectOffset, 4.5f);

            Health health = col.GetComponent<Health>();
            health.Damage(RandomDamage());
        }
    }

    int RandomDamage() => Random.Range(minDamage, maxDamage);
}
