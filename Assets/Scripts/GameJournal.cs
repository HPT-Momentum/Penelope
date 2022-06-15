using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public struct GameJournalItem
{
    public DateTime datetime;
    public string action;
    public string actionSentence;
    public string actionSentenceStart;
    public GameJournalItem(string action, string actionSentenceStart)
    {
        this.datetime = DateTime.Now;
        this.action = action;
        this.actionSentence = $"{actionSentenceStart} om {this.datetime}";
        this.actionSentenceStart = actionSentenceStart;
    }
}
public class GameJournal : MonoBehaviour
{
    public GameObject gameJournalMenu;
    public Text gameJournalText;

    List<GameJournalItem> journalLogs = new List<GameJournalItem>();
    public GameJournalItem lastAction;

    public void addJournalLog(string action, string actionSentenceStart)
    {
        GameJournalItem gji = new GameJournalItem(action, actionSentenceStart);


        journalLogs.Add(gji);
        lastAction = gji;
    }

    public void OpenMenu()
    {
        gameJournalText.text = "";
        gameJournalMenu.SetActive(true);

        var reverseList = new List<GameJournalItem>(journalLogs);
        reverseList.Reverse();
        foreach (GameJournalItem gi in reverseList)
        {
            gameJournalText.text += gi.actionSentence + "\n";
        }
    }

    public void CloseMenu()
    {
        gameJournalMenu.SetActive(false);
    }
}
