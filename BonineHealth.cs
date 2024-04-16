using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BonineHealth : MonoBehaviour
{
    [HideInInspector] public int health;
    public int maxHealth;


    [Header("Reference")]
    public Slider healthBar;
    public Image heartGameObject;
    public TextMeshProUGUI healthText;
    public Sprite[] healthIcons;
    Animator animator;
    Movement movement;
    HitEffect hitEffect;
    public GameObject deathScreen;

    void Start()
    {
        health = maxHealth;
        movement = GetComponent<Movement>();
        animator = GetComponent<Animator>();
        hitEffect = GetComponent<HitEffect>();
    }

    public void Damage(int damage)
    {
        hitEffect.CreateHitffect();
        health -= damage;

        if (health < 0)
        {
            deathScreen.SetActive(true);
            animator.Play("Bonine_Dead");
            health = 0;

            movement.isDead = true;
            movement.allowMovement = false;
        }

        SetHealthOnUI();
    }

    public void Heal(int healing)
    {
        //* set some effects if necessary
        health += healing;

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        SetHealthOnUI();
    }

    void SetHealthOnUI()
    {
        healthBar.value = health;
        healthText.text = health.ToString() + "HP";
        heartGameObject.sprite = healthIcons[Convert.ToInt32(Math.Ceiling((float)health / 10))];
    }
}
