using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BonineEnergy : MonoBehaviour
{
    public Slider energyBar;
    public Image energyIconObject;
    public TextMeshProUGUI energyText;
    public Sprite[] energyIcons;
    public int energy = 100;
    public float energyRate = 0.05f;
    public float energyCooldown = 2f;
    Coroutine runningCoroutine;
    float energyTimer;

    void Update()
    {
        if (energy < 100)
        {
            if (energyTimer < energyCooldown) energyTimer += Time.deltaTime;
            else IncreaseEnergy(100, energyRate);
        }
    }

    public void DecreaseEnergy(int change, float rate = 0.03f) => SetEnergy(energy - change >= 0 ? energy - change : 0, false, rate);
    public void IncreaseEnergy(int change, float rate = 0.03f) => SetEnergy(energy + change <= 100 ? energy + change : 100, true, rate);

    IEnumerator UseEnergyCoroutine(int energyVal, bool increase, float rate)
    {
        bool condition = increase ? energy < energyVal : energy > energyVal;

        while (condition)
        {
            if (!increase)
            {
                energy = energy - 1 <= 0 ? 0 : energy - 1;
                energyTimer = 0;
            }

            else energy = energy + 1 >= 100 ? 100 : energy + 1;

            energyBar.value = energy;
            energyText.text = energy.ToString() + "EG";
            energyIconObject.sprite = energyIcons[Convert.ToInt32(Math.Ceiling((float)energy / 10))];
            condition = increase ? energy < energyVal : energy > energyVal;
            yield return new WaitForSeconds(rate);
        }
    }

    IEnumerator UseEnergy(int energyVal, bool increase, float rate)
    {
        if (runningCoroutine != null) StopCoroutine(runningCoroutine);

        runningCoroutine = StartCoroutine(UseEnergyCoroutine(energyVal, increase, rate));
        yield return null;
    }


    void SetEnergy(int energyVal, bool increase, float rate)
    {
        if (energyVal == energy) return;
        StartCoroutine(UseEnergy(energyVal, increase, rate));
    }
}
