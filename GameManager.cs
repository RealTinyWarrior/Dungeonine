using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public GameObject pausePanel;
    public Image pauseContainerImage;
    public RectTransform pauseContainer;
    public Sprite emptyItem;
    public GameObject chatObject;
    public GameObject startObject;
    public GameObject chestObject;
    public GameObject slogan;
    public AudioSource pauseAudio;
    public AudioSource resumeAudio;
    public AudioSource bgAudio;
    public AudioSource bgAudio2;
    public GameObject sceneTransition;
    public GameObject restartPanel;
    public GameObject quitPanel;
    CursorManager cursorManager;
    public GameObject lightObject;
    public Light2D globalLight;
    [HideInInspector] public bool bossFightOngoing = false;
    [HideInInspector] public bool canPause = true;
    Camera mainCamera;
    Movement bonineMovement;
    BonineEnergy bonineEnergy;
    MessageManager messageManager;
    InventoryManager inventory;
    ChestManager chest;

    void Update()
    {
        bool escapeKeyDown = Input.GetKeyDown(KeyCode.Escape);

        if (canPause)
        {
            if (!escapeKeyDown) return;
            if (Time.timeScale == 0) ResumeGame(false);
            else PauseGame();
        }

        else if ((escapeKeyDown || Input.GetKeyDown(KeyCode.E)) && slogan == null)
        {
            chest.ExitChestID();
        }
    }

    void Start()
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume")) * 20);
        mainCamera = Camera.main;

        GameObject bonine = GameObject.FindGameObjectWithTag("Bonine");
        bonineEnergy = bonine.GetComponent<BonineEnergy>();
        bonineMovement = bonine.GetComponent<Movement>();
        messageManager = GetComponent<MessageManager>();
        cursorManager = GetComponent<CursorManager>();
        inventory = GetComponent<InventoryManager>();
        chest = GetComponent<ChestManager>();

        if (PlayerPrefs.GetInt("lighting", 1) == 0 && lightObject != null && globalLight != null)
        {
            lightObject.SetActive(false);
            globalLight.intensity = 1;
        }

        if (PlayerPrefs.GetInt("glow", 1) == 0)
        {
            UniversalAdditionalCameraData cameraData = mainCamera.GetUniversalAdditionalCameraData();
            cameraData.renderPostProcessing = false;
        }
    }

    public void ChangeFloor(int floorNum)
    {
        inventory.SaveUserInventory();
        chest.SaveChestData();

        SceneManager.LoadScene("Floor_" + floorNum);
    }

    public void OnSimpleInteraction(GameObject interactableObject)
    {
        bonineMovement.allowMovement = false;

        string text = interactableObject.GetComponent<OnInteraction>().interactionText;
        messageManager.Edit("Interact", new string[] { text }, new Sprite[] { emptyItem });
    }

    public void LevelComplete(int level) => StartCoroutine(EndLevel(level));

    public void PauseGame()
    {
        if (chatObject.activeSelf || startObject.activeSelf || restartPanel.activeSelf || quitPanel.activeSelf) return;

        Time.timeScale = 0;
        pausePanel.SetActive(true);
        pauseContainerImage.color = new Color(1, 1, 1, 0f);
        pauseContainer.anchoredPosition = new Vector2(0, 400);
        pauseContainerImage.DOFade(1, 0.22f).SetUpdate(true);
        pauseContainer.DOAnchorPosY(0, 0.26f).SetUpdate(true);

        bgAudio.Pause();
        pauseAudio.Play();
        bonineMovement.allowMovement = false;
        if (bgAudio2 != null) bgAudio2.Pause();
    }

    public void ResumeGame(bool isRestart)
    {
        if (chatObject.activeSelf || startObject.activeSelf || restartPanel.activeSelf || quitPanel.activeSelf) return;
        bonineMovement.allowMovement = true;

        if (!isRestart)
        {
            pausePanel.SetActive(false);
            resumeAudio.Play();
            bgAudio.Play();

            if (bgAudio2 != null && bossFightOngoing) bgAudio2.Play();
        }

        Time.timeScale = 1;
    }

    IEnumerator EndLevel(int level)
    {
        sceneTransition.SetActive(true);
        yield return new WaitForSeconds(1f);

        int levelsUnlocked = PlayerPrefs.GetInt("LevelsUnlocked", level);
        PlayerPrefs.SetInt("LevelsUnlocked", levelsUnlocked);
        inventory.SaveUserInventory();
        chest.SaveChestData();

        SceneManager.LoadScene("Floor_" + level);
    }

    public bool UseUtility()
    {
        if (inventory.delay > 0) return false;
        if (bonineMovement.isDead) return false;
        if (bonineEnergy.energy <= 0) return false;
        if (chatObject.activeSelf || chestObject.activeSelf || slogan != null) return false;
        //TODO Apply cursor manager check here.

        return true;
    }

    public void GoToMainMenu() => SceneManager.LoadScene("MainMenu");
    public void QuitGame() => SceneManager.LoadScene("MainMenu");
    public void RestartGame()
    {
        SceneManager.LoadScene("Floor_" + chest.floor);
        Time.timeScale = 1;
    }
}
