using UnityEngine;
using System;

public class GetGrandfatherTime : MonoBehaviour
{
    public Sprite interactSprite;
    MessageManager messageManager;

    void Start()
    {
        GameObject gameManagerObject = GameObject.FindGameObjectWithTag("GameManager");
        messageManager = gameManagerObject.GetComponent<MessageManager>();
    }

    public void ShowTimeOnChat()
    {
        DateTime now = DateTime.Now;

        int hour = now.Hour;
        string amPm = hour >= 12 ? "PM" : "AM";

        hour %= 12;
        hour = hour != 0 ? hour : 12;

        string text = string.Format("{0}:{1:00} {2}", hour, now.Minute, amPm);

        messageManager.Edit("Clock", new string[] {
            "The clock says it is " + text
        }, new Sprite[] { interactSprite });
    }
}