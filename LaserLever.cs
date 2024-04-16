using UnityEngine;

public class LaserLever : MonoBehaviour
{
    public Sprite leverOn;
    public bool isOn;
    public GameObject turnOff;

    public void OnLeverActivation()
    {
        if (isOn) return;
        GetComponent<SpriteRenderer>().sprite = leverOn;

        turnOff.SetActive(false);
        isOn = true;
    }
}
