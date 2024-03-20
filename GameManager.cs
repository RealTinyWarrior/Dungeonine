using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    InventoryManager inventory;
    ChestManager chest;

    void Start()
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume")) * 20);

        inventory = GetComponent<InventoryManager>();
        chest = GetComponent<ChestManager>();
    }

    public void ChangeFloor(int floorNum)
    {
        inventory.SaveUserInventory();
        chest.SaveChestData();

        SceneManager.LoadScene("Floor_" + floorNum);
    }
}
