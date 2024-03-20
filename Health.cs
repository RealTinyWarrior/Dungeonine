using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    [HideInInspector] public float health;

    void Start()
    {
        health = maxHealth;
    }

    public void Damange(float damage)
    {
        //TODO: set some effects if necessary
        health -= damage;

        if (health < 0)
        {
            health = 0;
        }
    }

    public void Heal(float healing)
    {
        //TODO: set some effects if necessary
        health += healing;

        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
}