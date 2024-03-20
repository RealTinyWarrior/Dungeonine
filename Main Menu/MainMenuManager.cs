using TMPro;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public Texture2D defaultCursor;
    public TextMeshProUGUI text;
    public GameObject settingsPanel;
    public GameObject ResetPanel;
    public GameObject quitPanel;
    public GameObject playPanel;
    public GameObject clickSound;
    public GameObject playButton;
    private bool addTimer = true;
    public string[] splashTexts;

    public void NoConfirmationOfGameQuit()
    {
        quitPanel.SetActive(false);
    }

    public void YesConfirmationOfDataReset()
    {
        PlayerPrefs.DeleteAll();
        ResetPanel.SetActive(false);
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);

        playPanel.SetActive(true);
        GetComponent<AudioSource>().Stop();
        playButton.GetComponent<MainMenuHover>().addPlayEffect = true;
    }

    public void NoConfirmationOfDataReset()
    {
        ResetPanel.SetActive(false);
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    public void PlayClickSound()
    {
        clickSound.GetComponent<AudioSource>().Play();
    }

    void Start()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        text.text = splashTexts[Random.Range(0, splashTexts.Length - 1)];

        text.outlineWidth = 0.3f;
        text.outlineColor = Color.black;
    }

    void Update()
    {
        if (addTimer) text.fontSize += 23 * Time.deltaTime;
        else text.fontSize -= 23 * Time.deltaTime;

        if (text.fontSize > 48) addTimer = false;
        if (text.fontSize < 40) addTimer = true;
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
