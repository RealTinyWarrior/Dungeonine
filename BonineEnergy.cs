using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BonineEnergy : MonoBehaviour
{
    public Slider energyBar;
    public Image energyIconObject;
    public TextMeshProUGUI energyText;
    public Sprite[] energyIcons;

    public void SetEnergy(float energy)
    {
        energyBar.value = energy;
        energyText.text = energy.ToString() + "HP";
        energyIconObject.sprite = energyIcons[Convert.ToInt32(Math.Ceiling(energy / 10))];
    }
}
