using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

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
    public GameObject deathPanel;
    public AudioSource pauseAudio;
    public AudioSource resumeAudio;
    public AudioSource bgAudio;
    public AudioSource bgAudio2;
    public GameObject sceneTransition;
    public GameObject restartPanel;
    public GameObject quitPanel;
    public GameObject lightObject;
    public Light2D globalLight;
    public Tilemap tilemap;
    [HideInInspector] public bool bossFightOngoing = false;
    [HideInInspector] public bool canPause = true;
    Camera mainCamera;
    Movement bonineMovement;
    BonineEnergy bonineEnergy;
    MessageManager messageManager;
    InventoryManager inventory;
    ChestManager chest;
    float musicVolume;
    float sfxVolume;

    void Update()
    {
        bool escapeKeyDown = Input.GetKeyDown(KeyCode.Escape);

        if (canPause)
        {
            if (!escapeKeyDown) return;
            if (Time.timeScale == 0) ResumeGame(false);
            else if (!deathPanel.activeSelf) PauseGame();
        }

        else if ((escapeKeyDown || Input.GetKeyDown(KeyCode.E)) && slogan == null && (!pausePanel.activeSelf))
        {
            chest.ExitChestID();
        }
    }

    void Start()
    {
        musicVolume = Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20;
        sfxVolume = Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20;

        audioMixer.SetFloat("MusicVolume", musicVolume);
        audioMixer.SetFloat("SFXVolume", sfxVolume);
        mainCamera = Camera.main;

        GameObject bonine = GameObject.FindGameObjectWithTag("Bonine");
        bonineEnergy = bonine.GetComponent<BonineEnergy>();
        bonineMovement = bonine.GetComponent<Movement>();
        messageManager = GetComponent<MessageManager>();
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

        StartCoroutine(ChangeFloorCoroutine(floorNum));
    }

    IEnumerator ChangeFloorCoroutine(int floor)
    {
        sceneTransition.SetActive(true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Floor_" + floor);
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

        //! Audio Control: Setting SFX and Music to 0
        bgAudio.Pause();
        pauseAudio.Play();
        bonineMovement.allowMovement = false;
        if (bgAudio2 != null) bgAudio2.Pause();

        audioMixer.SetFloat("MusicVolume", Mathf.Log(0.0001f) * 20);
        audioMixer.SetFloat("SFXVolume", Mathf.Log(0.0001f) * 20);
    }

    public void ResumeGame(bool isRestart)
    {
        if (chatObject.activeSelf || startObject.activeSelf || restartPanel.activeSelf || quitPanel.activeSelf) return;
        bonineMovement.allowMovement = true;

        if (!isRestart)
        {
            pausePanel.SetActive(false);

            //! Audio Control: Setting SFX and Music to 0
            audioMixer.SetFloat("MusicVolume", musicVolume);
            audioMixer.SetFloat("SFXVolume", sfxVolume);

            resumeAudio.Play();
            if (!bossFightOngoing) bgAudio.Play();
            if (bgAudio2 != null && bossFightOngoing) bgAudio2.Play();
        }

        Time.timeScale = 1;
    }

    IEnumerator EndLevel(int level)
    {
        sceneTransition.SetActive(true);
        yield return new WaitForSeconds(1f);

        int levelsUnlocked = PlayerPrefs.GetInt("LevelsUnlocked", level);
        if (levelsUnlocked < level) PlayerPrefs.SetInt("LevelsUnlocked", level);
        else PlayerPrefs.SetInt("LevelsUnlocked", levelsUnlocked);

        inventory.RemoveDetailItemOnExit();
        inventory.SaveUserInventory();
        chest.SaveChestData();
        PlayerPrefs.Save();

        SceneManager.LoadScene("Floor_" + level);
    }

    public bool UseUtility()
    {
        if (bonineMovement.isDead) return false;
        if (bonineEnergy.energy <= 0) return false;
        if (chatObject.activeSelf || chestObject.activeSelf || slogan != null) return false;
        //TODO Apply cursor manager check here. if necessary, add global delay check here.

        return true;
    }

    public void GoToMainMenu() => SceneManager.LoadScene("MainMenu");
    public void QuitGame() => SceneManager.LoadScene("MainMenu");
    public void RestartGame()
    {
        SceneManager.LoadScene("Floor_" + chest.floor);
        Time.timeScale = 1;
    }

    public bool IsPointNavigable(Vector2 point)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(point);
        TileBase tile = tilemap.GetTile(cellPosition);

        if (tile != null) return false;
        return true;
    }
}
