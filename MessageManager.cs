using System;
using System.Collections;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    [HideInInspector] public string question = "";
    [HideInInspector] public bool answer = false;
    public float typingSpeed = 0.05f;
    public TextMeshProUGUI chatNameField;
    public TextMeshProUGUI textField;
    public AudioSource clickAudio;
    public AudioSource deepClick;
    public Image userIconField;
    public GameObject messageObject;
    public GameObject choiceObject;
    public AudioSource[] typingAudios;
    string tempQuestion = "";
    Movement bonineMovement;
    string resolved = "";
    int position = 0;
    int toBeDecreased = 1;
    int currentTypingAudio = 0;
    string[] texts;
    Sprite[] icons;
    Coroutine typeTextCoroutine;

    void Start() => bonineMovement = GameObject.FindGameObjectWithTag("Bonine").GetComponent<Movement>();

    // Opens the message popup with the specified information
    public void Edit(string name, string[] userTexts, Sprite[] userIcons, int spriteIndex = 0, int audioIndex = 0)
    {
        bonineMovement.ChangeAnimationState(bonineMovement.idleState);
        bonineMovement.allowMovement = false;

        currentTypingAudio = audioIndex;
        typingAudios[currentTypingAudio].Play();

        chatNameField.text = name;
        textField.text = "";
        texts = userTexts;
        icons = userIcons;

        ChangeIcon(icons[spriteIndex]);
        messageObject.SetActive(true);
        StartCoroutine(TypeText(texts[position]));
    }

    // * Element 1: Has the user answered the question | Element 2: What is the answer (yes=true, no=false)
    public bool[] GetAnswer(string questionParam)
    {
        if (questionParam == question)
        {
            question = "";
            bool tempAnswer = answer;
            answer = false;
            return new bool[] { true, tempAnswer };
        }

        return new bool[] { false, false };
    }

    public void StopAllTypingAudios()
    {
        foreach (AudioSource audioClip in typingAudios)
        {
            audioClip.Stop();
        }
    }

    // Goes to the next line of dialogue
    public void NextButton()
    {
        if (choiceObject.activeSelf) return;
        clickAudio.Play();

        // * The end of Dialogue
        if (texts.Length - 1 <= position)
        {
            resolved = texts[position];
            messageObject.SetActive(false);
            bonineMovement.allowMovement = true;
            position = 0;
            StopAllTypingAudios();

            currentTypingAudio = 0;
            toBeDecreased = 1;
        }

        else
        {
            position++;

            switch (texts[position])
            {
                case "<name>":
                    chatNameField.text = texts[position + 1];
                    position += 2;

                    toBeDecreased = 3;
                    break;

                case "<icon>":
                    int textureIndex = Convert.ToInt32(texts[position + 1]);
                    ChangeIcon(icons[textureIndex]);
                    position += 2;

                    toBeDecreased = 3;
                    break;

                case "<choice>":
                    choiceObject.SetActive(true);
                    tempQuestion = texts[position + 1];
                    position += 2;

                    toBeDecreased = 3;
                    break;

                default:
                    choiceObject.SetActive(false);
                    toBeDecreased = 1;
                    break;
            }

            textField.text = "";
            typingAudios[currentTypingAudio].Play();
            StartCoroutine(TypeText(texts[position]));
            resolved = texts[position - toBeDecreased];
        }

        // Makes sure Bonine is standing still
        bonineMovement.ChangeAnimationState(bonineMovement.idleState);
    }

    // * Manages the Typing animation

    IEnumerator TypeText(string text)
    {
        if (typeTextCoroutine != null)
        {
            StopCoroutine(typeTextCoroutine);
        }

        typeTextCoroutine = StartCoroutine(TypeTextCoroutine(text));
        yield return null;
    }

    IEnumerator TypeTextCoroutine(string text)
    {
        foreach (char c in text)
        {
            textField.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        StopAllTypingAudios();
        typeTextCoroutine = null;
    }

    public bool GetResolved(string text)
    {
        if (resolved == text)
        {
            resolved = "";
            return true;
        }

        return false;
    }

    public void NextButtonYes()
    {
        clickAudio.Play();
        question = tempQuestion;
        answer = true;

        choiceObject.SetActive(false);
        NextButton();
    }

    public void NextButtonNo()
    {
        deepClick.Play();
        question = tempQuestion;
        answer = false;

        choiceObject.SetActive(false);
        NextButton();
    }

    void ChangeIcon(Sprite userIcon)
    {
        userIconField.sprite = userIcon;
    }
}
