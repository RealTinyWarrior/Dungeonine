using UnityEngine;

public class WormDigDamage : MonoBehaviour
{
    public int startDamage;
    public int endDamage;
    public float knockbackStrength = 10;
    public float knockbackDuration = 0.5f;
    BonineHealth bonineHealth;
    GameObject bonine;
    Knockback bonineKnockback;

    void Start()
    {
        bonine = GameObject.FindGameObjectWithTag("Bonine");
        bonineHealth = bonine.GetComponent<BonineHealth>();
        bonineKnockback = bonine.GetComponent<Knockback>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("BonineHitbox"))
        {
            Vector2 knockbackDirection = (Vector2)(bonine.transform.position - transform.position);

            if (knockbackDirection.x == 0 && knockbackDirection.y == 0)
                knockbackDirection = Random.insideUnitCircle.normalized;

            else knockbackDirection = knockbackDirection.normalized;

            bonineHealth.Damage(Random.Range(startDamage, endDamage));
            bonineKnockback.ApplyKnockback(knockbackDirection, knockbackStrength, knockbackDuration);
        }
    }
}