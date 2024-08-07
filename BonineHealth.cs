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
    Coroutine runningCoroutine;
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

    public void Damage(int damage, float rate = 0.02f)
    {
        hitEffect.CreateHitffect();
        hitAudio.Play();

        StartCoroutine(UseHealthCoroutine(health - damage <= 0 ? 0 : health - damage, false, rate));
    }

    public void Heal(int healing, float rate = 0.02f) => StartCoroutine(UseHealth(health + healing >= 100 ? 100 : health + healing, true, rate));

    IEnumerator UseHealthCoroutine(int healthVal, bool increase, float rate)
    {
        bool condition = increase ? health < healthVal : health > healthVal;

        while (condition)
        {
            if (!increase)
            {
                health = health - 1 <= 0 ? 0 : health - 1;
                if (health <= 0) PlayDeathEffect();
            }

            else health = health + 1 >= 100 ? 100 : health + 1;

            healthBar.value = health;
            healthText.text = health.ToString() + "HP";
            heartGameObject.sprite = healthIcons[Convert.ToInt32(Math.Ceiling((float)health / 10))];
            condition = increase ? health < healthVal : health > healthVal;
            yield return new WaitForSeconds(rate);
        }
    }

    void PlayDeathEffect()
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

    IEnumerator UseHealth(int healthVal, bool increase, float rate)
    {
        if (runningCoroutine != null) StopCoroutine(runningCoroutine);

        runningCoroutine = StartCoroutine(UseHealthCoroutine(healthVal, increase, rate));
        yield return null;
    }

    IEnumerator PlayDeathMusic()
    {
        yield return new WaitForSeconds(3.5f);
        deathMusic.Play();
    }
}
