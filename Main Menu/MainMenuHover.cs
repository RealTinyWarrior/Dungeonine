using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Manages the buttons in Main Menu
public class MainMenuHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    bool hovering;
    public bool addPlayEffect = false;
    TextMeshProUGUI text;
    AudioSource hoverAudio;
    float colorCount = 0;
    float countSceneDelay = 0;
    public Texture2D pointerCursor;
    public Texture2D defaultCursor;
    public Image background;
    public GameObject settingsMenu;
    public GameObject resetData;
    public GameObject floorsPanel;
    public GameObject clickSound;
    public GameObject quitPanel;
    public GameObject playPanel;
    public GameObject gameManager;
    public MainMenuManager mainMenuManager;

    public enum Buttons
    {
        quit,
        newGame,
        floors,
        play,
        settings,
        others,
        exitSettings,
        exitFloors
    }

    public Buttons button;

    void Start()
    {
        hoverAudio = gameObject.GetComponent<AudioSource>();
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (addPlayEffect)
        {
            colorCount += Time.deltaTime;
            if (colorCount < 1) playPanel.GetComponent<Image>().color = new Color(0, 0, 0, colorCount);

            else
            {
                playPanel.GetComponent<Image>().color = new Color(0, 0, 0, 1);
                background.color = new Color(0, 0, 0, 1);

                countSceneDelay += Time.deltaTime;

                if (countSceneDelay > 1.5)
                {
                    int levelsUnlocked = PlayerPrefs.GetInt("LevelsUnlocked", 1);
                    SceneManager.LoadScene("Floor_" + levelsUnlocked);
                }
            }
        }

        if (!hovering || button == Buttons.others) return;
        if (button == Buttons.settings) gameObject.GetComponent<RectTransform>().Rotate(Vector3.forward, 20 * Time.deltaTime);

        if (Input.GetMouseButtonDown(0)) ButtonControl();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button == Buttons.exitFloors || button == Buttons.exitSettings) transform.localScale = new Vector3(1.1f, 1.1f, 1);
        Cursor.SetCursor(pointerCursor, Vector2.zero, CursorMode.Auto);
        if (text != null) text.color = Color.cyan;
        if (hoverAudio.gameObject.activeSelf) hoverAudio.Play();
        hovering = true;

        if (mainMenuManager == null) return;
        if (mainMenuManager.selectedButton != (int)button) mainMenuManager.texts[mainMenuManager.selectedButton].color = new Color(0.807f, 0.956f, 1);
        else mainMenuManager.texts[mainMenuManager.selectedButton].color = Color.cyan;

        mainMenuManager.pointerPos.anchoredPosition = new Vector2(mainMenuManager.pointerPos.anchoredPosition.x, 128 + 100 * (int)button);
        mainMenuManager.selectedButton = (int)button;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (text != null) text.color = new Color(0.807f, 0.956f, 1);
        if (button == Buttons.exitFloors || button == Buttons.exitSettings) transform.localScale = new Vector3(1, 1, 1);

        hovering = false;
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    public void ButtonControl()
    {
        if (clickSound != null) clickSound.GetComponent<AudioSource>().Play();

        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        hovering = false;

        switch (button)
        {
            case Buttons.settings:
                settingsMenu.SetActive(true);
                break;

            case Buttons.exitSettings:
                transform.localScale = new Vector3(1, 1, 1);
                settingsMenu.SetActive(false);
                break;

            case Buttons.quit:
                quitPanel.SetActive(true);
                break;

            case Buttons.newGame:
                resetData.SetActive(true);
                break;

            case Buttons.floors:
                floorsPanel.SetActive(true);
                break;

            case Buttons.exitFloors:
                transform.localScale = new Vector3(1, 1, 1);
                floorsPanel.SetActive(false);
                break;

            case Buttons.play:
                gameManager.GetComponent<AudioSource>().Stop();
                playPanel.SetActive(true);
                addPlayEffect = true;
                break;
        }
    }
}