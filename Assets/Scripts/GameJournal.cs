using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameJournal : MonoBehaviour
{
    List<string> journalLogs = new List<string>();

    public void addJournalLog (string log){
        journalLogs.Add(log);

        Debug.Log(log);
    }
}
