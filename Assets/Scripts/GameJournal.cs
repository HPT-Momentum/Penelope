using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameJournal : MonoBehaviour
{
    public GameObject gameJournalMenu;
    public Text gameJournalText;

    //Nieuwe list aanmaken
    List<string> journalLogs = new List<string>();

    public void addJournalLog (string log){
        //Elke keer als een actiege logd wordt, voeg dan ook de tijd toe
        log+= DateTime.Now;
        //Voeg de log toe aan de string
        journalLogs.Add(log);
    }
    
    public void OpenMenu(){
        //Wanneer het menu wordt geopend, maak een leeg tekstvak aan en open gameJournalMenu
        gameJournalText.text = "";
        gameJournalMenu.SetActive(true);
        //Reverse dan de list journalLogs
        var reverseList = new List<string>(journalLogs);
        reverseList.Reverse();
        foreach (string log in reverseList) {
            //Vul gameJournalText.text met de log en \n zorgt ervoor dat na elke log een new line begint
            gameJournalText.text += log + "\n";
        }
    }
    
    public void CloseMenu(){
        //Wanneer het menu wordt gesloten, sluit gameJournalMenu
        gameJournalMenu.SetActive(false);
    }
}
