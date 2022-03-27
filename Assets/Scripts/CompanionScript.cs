using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionScript : InteractableObject
{
    public RectTransform dialogueBox;
    public string companionName;

    public override string GetDescription()
    {
        if (!dialogueBox.gameObject.activeSelf) return $"Press [{key}] to talk to {companionName}";
        return "";
    }
    
    public override void OnInteract()
    {
        if (!dialogueBox.gameObject.activeSelf) 
            dialogueBox.GetComponent<DialogueScript>().StartDialogue(companionName);
    }
}
