using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameJournal : MonoBehaviour
{
    public GameObject gameJournalMenu;

    List<string> journalLogs = new List<string>();

    public void addJournalLog (string log){
        log+= DateTime.Now;

        journalLogs.Add(log);

        Debug.Log(log);
    }

    public void OpenMenu(){
        gameJournalMenu.SetActive(true);
    }
    
    public void CloseMenu(){
        gameJournalMenu.SetActive(false);
    }
}
