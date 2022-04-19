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
    
    public override void OnInteract(GameObject playerObject)
    {
        if (!playerObject.GetComponent<PlayerInteraction>().dialogueBox.activeSelf) 
            playerObject.GetComponent<PlayerInteraction>().dialogueBox.GetComponent<DialogueScript>().StartDialogue(companionName);
    }
}
