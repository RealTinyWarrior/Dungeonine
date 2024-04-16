using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Globalization;

public class GameTablet : MonoBehaviour
{
    public Image wallpaper;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI weekTime;
    public string[] applications;
    public GameObject[] applicationObjects;
    RectTransform tabletPosition;
    bool isOpened = false;
    string activeApplication = "home";

    void Start()
    {
        weekTime.text = DateTime.Now.DayOfWeek.ToString();
        tabletPosition = GetComponent<RectTransform>();
        StartCoroutine(UpdateTime());
    }

    IEnumerator UpdateTime()
    {
        while (true)
        {
            DateTime now = DateTime.Now;

            int hour = now.Hour;
            string amPm = hour >= 12 ? "PM" : "AM";

            hour %= 12;
            hour = hour != 0 ? hour : 12;

            timeText.text = string.Format("{0}:{1:00}{2}", hour, now.Minute, amPm);
            yield return new WaitForSeconds(60);
        }
    }

    public void OpenTalbetUI()
    {
        if (isOpened)
        {
            tabletPosition.DOAnchorPos(new Vector2(1500, -413), 0.7f);
            isOpened = false;
            return;
        }

        tabletPosition.DOAnchorPos(new Vector2(831, -413), 0.7f);
        isOpened = true;
    }

    public void OpenApplicatin(string application)
    {
        if (application != "home") wallpaper.color = new Color(1, 1, 1);
        else wallpaper.color = new Color(0.51f, 0.51f, 0.51f);

        applicationObjects[SearchForApp(activeApplication)].SetActive(false);
        applicationObjects[SearchForApp(application)].SetActive(true);
        activeApplication = application;
    }

    int SearchForApp(string app)
    {
        for (int i = 0; i < applications.Length; i++)
        {
            if (applications[i] == app) return i;
        }

        return -1;
    }
}
