using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public EffectCode deathEffect;
    [HideInInspector] public int health;
    EffectManager effectManager;
    HitEffect hitEffect;
    public AudioSource hitAudio;

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

        if (health <= 0)
        {
            health = 0;
            GameObject.FindGameObjectWithTag("DeathAudio").GetComponent<AudioSource>().Play();

            effectManager.Create(deathEffect, 0.5f, (Vector2)transform.position + hitEffect.effectOffset, Color.white, 1.2f);
            Destroy(gameObject);
        }

        else if (hitAudio != null) hitAudio.Play();
    }

    public void Heal(int healing)
    {
        health += healing;

        if (health >= maxHealth)
        {
            health = maxHealth;
        }
    }
}