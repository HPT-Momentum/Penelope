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

        return $"Met {companionName} gepraat om ";
    }
}
