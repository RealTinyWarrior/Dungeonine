using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    [HideInInspector] public string question = "";
    [HideInInspector] public bool answer = false;
    public TextMeshProUGUI chatNameField;
    public TextMeshProUGUI textField;
    public AudioSource clickAudio;
    public Image userIconField;
    public GameObject messageObject;
    public GameObject choiceObject;
    string tempQuestion = "";
    Movement bonineMovement;
    string resolved = "";
    int position = 0;
    int toBeDecreased = 1;
    string[] texts;
    Sprite[] icons;

    public void Start() => bonineMovement = GameObject.FindGameObjectWithTag("Bonine").GetComponent<Movement>();

    public void Edit(string name, string[] userTexts, Sprite[] userIcons)
    {
        chatNameField.text = name;
        texts = userTexts;
        icons = userIcons;

        ChangeIcon(icons[0]);
        textField.text = texts[position];

        messageObject.SetActive(true);
    }

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

    public void NextButton()
    {
        if (texts.Length - 1 <= position)
        {
            resolved = texts[position];
            messageObject.SetActive(false);
            bonineMovement.allowMovement = true;
            position = 0;

            toBeDecreased = 1;
        }

        else
        {
            position++;

            if (texts[position] == "<name>")
            {
                chatNameField.text = texts[position + 1];
                position += 2;

                toBeDecreased = 3;
            }

            else if (texts[position] == "<icon>")
            {
                int textureIndex = Convert.ToInt32(texts[position + 1]);
                ChangeIcon(icons[textureIndex]);
                position += 2;

                toBeDecreased = 3;
            }

            else if (texts[position] == "<choice>")
            {
                choiceObject.SetActive(true);
                tempQuestion = texts[position + 1];
                position += 2;

                toBeDecreased = 3;
            }

            else
            {
                choiceObject.SetActive(false);
                toBeDecreased = 1;
            }

            textField.text = texts[position];
            resolved = texts[position - toBeDecreased];
        }
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
        question = tempQuestion;
        answer = true;

        choiceObject.SetActive(false);
        NextButton();
    }

    public void NextButtonNo()
    {
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
