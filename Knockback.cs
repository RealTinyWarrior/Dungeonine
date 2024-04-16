using UnityEngine;
using System.Collections;

public class Knockback : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 originalVelocity;
    public bool isBonine;
    Movement movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (isBonine) movement = GetComponent<Movement>();
    }

    public void ApplyKnockback(Vector2 knockbackDirection, float knockbackStrength, float knockbackDuration)
    {
        originalVelocity = rb.velocity;
        StartCoroutine(KnockbackCoroutine(knockbackDirection * knockbackStrength, knockbackDuration));
    }

    private IEnumerator KnockbackCoroutine(Vector2 knockbackVelocity, float knockbackDuration)
    {
        float timer = 0;
        if (isBonine)
        {
            movement.allowMovement = false;
            movement.isUsedByKnockback = true;
        }

        while (timer < knockbackDuration)
        {
            timer += Time.deltaTime;
            float proportionCompleted = timer / knockbackDuration;
            rb.velocity = Vector2.Lerp(knockbackVelocity, originalVelocity, proportionCompleted);

            yield return null;
        }

        if (isBonine)
        {
            movement.allowMovement = true;
            movement.isUsedByKnockback = false;
        }

        rb.velocity = Vector2.zero;
    }
}
