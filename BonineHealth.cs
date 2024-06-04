using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using System.Collections;

public class BonineHealth : MonoBehaviour
{
    [HideInInspector] public int health;
    public int maxHealth;

    [Header("Reference")]
    public Slider healthBar;
    public Image heartGameObject;
    public TextMeshProUGUI healthText;
    public AudioMixer audioMixer;
    public GameObject deathScreen;
    public AudioSource deathAudio;
    public AudioSource deathMusic;
    public AudioSource hitAudio;
    public Sprite[] healthIcons;
    public AudioSource[] stopAudio;
    GlowManager glowManager;
    Animator animator;
    Movement movement;
    HitEffect hitEffect;

    void Start()
    {
        health = maxHealth;
        movement = GetComponent<Movement>();
        animator = GetComponent<Animator>();
        hitEffect = GetComponent<HitEffect>();
        glowManager = GetComponent<GlowManager>();
    }

    public void Damage(int damage)
    {
        hitEffect.CreateHitffect();
        health -= damage;
        hitAudio.Play();

        if (health <= 0)
        {
            if (!movement.isDead)
            {
                deathAudio.Play();
                StartCoroutine(PlayDeathMusic());
                audioMixer.SetFloat("SFXVolume", Mathf.Log10(0.2f) * 20);

                for (int i = 0; i < stopAudio.Length; i++)
                {
                    stopAudio[i].Stop();
                }
            }

            deathScreen.SetActive(true);
            animator.Play("Bonine_Dead");
            if (glowManager != null) glowManager.ApplyChangeInGlow(8);

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

        if (health >= maxHealth)
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

    IEnumerator PlayDeathMusic()
    {
        yield return new WaitForSeconds(3.5f);
        deathMusic.Play();
    }
}
