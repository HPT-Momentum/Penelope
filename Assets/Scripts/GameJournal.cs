using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameJournal : MonoBehaviour
{
    public GameObject gameJournalMenu;
    public Text gameJournalText;

    List<string> journalLogs = new List<string>();

    public void addJournalLog (string log){
        log+= DateTime.Now;

        journalLogs.Add(log);

        Debug.Log(log);
    }
    
    public void OpenMenu(){
        gameJournalText.text = "";
        gameJournalMenu.SetActive(true);

        var reverseList = new List<string>(journalLogs);
        reverseList.Reverse();
        foreach (string log in reverseList) {
            gameJournalText.text += log + "\n";
        }
    }
    
    public void CloseMenu(){
        gameJournalMenu.SetActive(false);
    }
}
