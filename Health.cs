using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public EffectCode deathEffect;
    [HideInInspector] public int health;
    EffectManager effectManager;
    HitEffect hitEffect;

    void Start()
    {
        effectManager = GameObject.FindGameObjectWithTag("EffectManager").GetComponent<EffectManager>();
        hitEffect = GetComponent<HitEffect>();
        health = maxHealth;
    }

    public void Damage(int damage)
    {
        hitEffect.CreateHitffect();
        health -= damage;

        if (health < 0)
        {
            health = 0;
            effectManager.Create(deathEffect, 0.5f, (Vector2)transform.position + hitEffect.effectOffset, 1.2f);
            Destroy(gameObject);
        }
    }

    public void Heal(int healing)
    {
        health += healing;

        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
}