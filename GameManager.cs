using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Sprite emptyItem;
    public GameObject dragPause;
    Movement bonineMovement;
    MessageManager messageManager;
    InventoryManager inventory;
    ChestManager chest;

    void Start()
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume")) * 20);

        bonineMovement = GameObject.FindGameObjectWithTag("Bonine").GetComponent<Movement>();
        messageManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MessageManager>();
        inventory = GetComponent<InventoryManager>();
        chest = GetComponent<ChestManager>();
    }

    public void ChangeFloor(int floorNum)
    {
        inventory.SaveUserInventory();
        chest.SaveChestData();

        SceneManager.LoadScene("Floor_" + floorNum);
    }

    public void OnSimpleInteraction(GameObject interactableObject)
    {
        string text = interactableObject.GetComponent<OnInteraction>().interactionText;
        messageManager.Edit("Interact", new string[] { text }, new Sprite[] { emptyItem });
    }

    public void LevelComplete(int level)
    {
        int levelsUnlocked = PlayerPrefs.GetInt("LevelsUnlocked", level);
        PlayerPrefs.SetInt("LevelsUnlocked", levelsUnlocked);
        inventory.SaveUserInventory();
        chest.SaveChestData();

        SceneManager.LoadScene("Floor_" + level);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        dragPause.SetActive(true);
        bonineMovement.allowMovement = false;
    }

    public void ResumeGame(bool isRestart)
    {
        if (!isRestart) dragPause.SetActive(false);
        bonineMovement.allowMovement = true;
        Time.timeScale = 1;
    }

    public void QuitGame() => SceneManager.LoadScene("MainMenu");
    public void RestartGame(int floor) => SceneManager.LoadScene("Floor_" + floor);
}
