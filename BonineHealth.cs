using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BonineHealth : MonoBehaviour
{
    public Slider healthBar;
    public Image heartGameObject;
    public TextMeshProUGUI healthText;
    public Sprite[] healthIcons;

    public void SetHealth(float health)
    {
        healthBar.value = health;
        healthText.text = health.ToString() + "HP";
        heartGameObject.sprite = healthIcons[Convert.ToInt32(Math.Ceiling(health / 10))];
    }
}
