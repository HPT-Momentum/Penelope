using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionScript : InteractableObject
{
    public string companionName;

    public override string GetDescription()
    {
        return $"Press [{key}] to talk to {companionName}";
    }

    public override string OnInteract(GameObject dialogueBox)
    {
        if (!dialogueBox.activeSelf)
            dialogueBox.GetComponent<DialogueScript>().StartDialogue(companionName);

        return companionName;
    }
    public string OnInteract(GameObject dialogueBox, GameJournal gj)
    {
        if (!dialogueBox.activeSelf)
            if (companionName.ToLower().Contains("buddy"))
            {
                // dialogueBox.GetComponent<DialogueScript>().contentTextComponent.text = "tesdsdasdadasd dsadsad";
                // dialogueBox.GetComponent<DialogueScript>().lines = gameJournal.journalLogs.ToArray();
                GameJournalItem lastAction = gj.lastAction;
                dialogueBox.GetComponent<DialogueScript>().lines = new[] { $"{lastAction.actionSentenceStart}, in de engage zone kan je dit bespreken met teamgenoten!" };

                // dialogueBox.GetComponent<DialogueScript>().lines = new[] { "xddd", "tsdasdadasdw dwd asdc asd" };
            }
            else if (companionName.ToLower().Contains("dummy"))
            {
                dialogueBox.GetComponent<DialogueScript>().lines = new[] { $"Begin bij de investigate zone!" };

            }
        dialogueBox.GetComponent<DialogueScript>().StartDialogue(companionName);

        return companionName;
    }
}
