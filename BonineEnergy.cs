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
    bool blockRegen = false;
    float energyTimer;

    void Update()
    {
        if (energy < 100)
        {
            if (energyTimer < energyCooldown) energyTimer += Time.deltaTime;

            else
            {
                StartCoroutine(RechargeEnergy());
            }
        }
    }

    IEnumerator RechargeEnergy()
    {
        while (energy < 100)
        {
            if (blockRegen)
            {
                blockRegen = false;
                break;
            }

            IncreaseEnergy(1);
            yield return new WaitForSeconds(energyRate);
        }
    }

    public void DecreaseEnergy(int change) => SetEnergy(energy - change >= 0 ? energy - change : 0);
    public void IncreaseEnergy(int change) => SetEnergy(energy + change <= 100 ? energy + change : 100);

    public void SetEnergy(int energyVal)
    {
        if (energyVal < energy)
        {
            blockRegen = true;
            energyTimer = 0;
        }

        energy = energyVal;
        energyBar.value = energyVal;
        energyText.text = energyVal.ToString() + "EG";
        energyIconObject.sprite = energyIcons[Convert.ToInt32(Math.Ceiling((float)energyVal / 10))];
    }
}
