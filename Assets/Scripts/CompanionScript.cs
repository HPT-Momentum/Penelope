using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionScript : InteractableObject
{
    public string companionName;

    public override string GetDescription()
    {
        // if (!dialogueBox.gameObject.activeSelf) return $"Press [{key}] to talk to {companionName}";
        return "";
    }
    
    public override void OnInteract(GameObject playerObject)
    {
        if (!playerObject.GetComponent<RectTransform>().gameObject.activeSelf) 
            playerObject.GetComponent<RectTransform>().GetComponent<DialogueScript>().StartDialogue(companionName);
    }
}
