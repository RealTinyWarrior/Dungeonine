using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public MainMenuHover mainMenuHover;
    public AudioSource moveAudio;
    public Texture2D defaultCursor;
    public TextMeshProUGUI text;
    public GameObject floorPanel;
    public GameObject settingsPanel;
    public GameObject resetPanel;
    public GameObject quitPanel;
    public GameObject playPanel;
    public GameObject clickSound;
    public GameObject playButton;
    public RectTransform pointerPos;
    public TextMeshProUGUI[] texts;
    public Toggle toggle;
    public Toggle glowToggle;
    public Toggle particleToggle;
    public AudioSource clickAudioToggle;
    private bool addTimer = true;
    public string[] splashTexts;
    [HideInInspector] public int selectedButton = 3;
    bool hasHeld = false;
    AudioSource localAudio;

    public void NoConfirmationOfGameQuit()
    {
        quitPanel.SetActive(false);
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    public void YesConfirmationOfDataReset()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1);
        int glow = PlayerPrefs.GetInt("glow", 1);
        int lighting = PlayerPrefs.GetInt("lighting", 1);

        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetInt("glow", glow);
        PlayerPrefs.SetInt("lighting", lighting);

        PlayerPrefs.Save();

        resetPanel.SetActive(false);
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);

        playPanel.SetActive(true);
        localAudio.Stop();
        playButton.GetComponent<MainMenuHover>().addPlayEffect = true;
    }

    public void NoConfirmationOfDataReset()
    {
        resetPanel.SetActive(false);
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    void Start()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        text.text = splashTexts[Random.Range(0, splashTexts.Length - 1)];
        localAudio = GetComponent<AudioSource>();

        text.outlineWidth = 0.3f;
        text.outlineColor = Color.black;
        Time.timeScale = 1;

        toggle.isOn = PlayerPrefs.GetInt("lighting", 1) == 1;
        glowToggle.isOn = PlayerPrefs.GetInt("glow", 1) == 1;
        particleToggle.isOn = PlayerPrefs.GetInt("particle", 1) == 1;
    }

    void Update()
    {
        if (addTimer) text.fontSize += 23 * Time.deltaTime;
        else text.fontSize -= 23 * Time.deltaTime;

        if (text.fontSize > 48) addTimer = false;
        if (text.fontSize < 40) addTimer = true;

        if (quitPanel.activeSelf || settingsPanel.activeSelf || resetPanel.activeSelf || playPanel.activeSelf || floorPanel.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            mainMenuHover = texts[selectedButton].GetComponent<MainMenuHover>();
            mainMenuHover.button = (MainMenuHover.Buttons)selectedButton;
            mainMenuHover.ButtonControl();
        }

        float inputDirection = Input.GetAxisRaw("Vertical");
        if (inputDirection == 0) hasHeld = false;

        else if (inputDirection > 0)
        {
            if (hasHeld || selectedButton == 3) return;

            moveAudio.Play();
            hasHeld = true;
            selectedButton++;
            texts[selectedButton].color = Color.cyan;
            pointerPos.anchoredPosition = new Vector2(pointerPos.anchoredPosition.x, 128 + 100 * selectedButton);

            for (int i = 0; i < 4; i++)
                if (i != selectedButton) texts[i].color = new Color(0.807f, 0.956f, 1);
        }

        else
        {
            if (hasHeld || selectedButton == 0) return;

            moveAudio.Play();
            hasHeld = true;
            selectedButton--;
            texts[selectedButton].color = Color.cyan;
            pointerPos.anchoredPosition = new Vector2(pointerPos.anchoredPosition.x, 128 + 100 * selectedButton);

            for (int i = 0; i < 4; i++)
                if (i != selectedButton) texts[i].color = new Color(0.807f, 0.956f, 1);
        }
    }

    public void OnChangeLighting2D()
    {
        if (settingsPanel.activeSelf) clickAudioToggle.Play();

        if (toggle.isOn) PlayerPrefs.SetInt("lighting", 1);
        else PlayerPrefs.SetInt("lighting", 0);
        PlayerPrefs.Save();
    }

    public void OnChangeGlow2D()
    {
        if (settingsPanel.activeSelf) clickAudioToggle.Play();

        if (glowToggle.isOn) PlayerPrefs.SetInt("glow", 1);
        else PlayerPrefs.SetInt("glow", 0);
        PlayerPrefs.Save();
    }

    public void OnChangeParticle2D()
    {
        if (settingsPanel.activeSelf) clickAudioToggle.Play();

        if (particleToggle.isOn) PlayerPrefs.SetInt("particle", 1);
        else PlayerPrefs.SetInt("particle", 0);
        PlayerPrefs.Save();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
