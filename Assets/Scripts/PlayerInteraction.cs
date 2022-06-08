using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction: MonoBehaviour {

    public float interactionDistance = 2f;

    public TMPro.TextMeshProUGUI interactionText;
	public GameObject dialogueBox;

    public Camera playerCamera;
    
    void Update() {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;

        bool successfulHit = false;
		
		// if the camera faces the interactable object and is within distance, handle the interaction
        if (Physics.Raycast(ray, out hit, interactionDistance)) {
            InteractableObject interactableObj = hit.collider.GetComponent<InteractableObject>();

            if (interactableObj != null) {
                HandleInteraction(interactableObj);
				
				// if the dialogueBox is not yet showing, show the description interaction text
				if (!dialogueBox.activeSelf)
                	interactionText.text = interactableObj.GetDescription();
				else
            		interactionText.text = "";

				successfulHit = true;
            }
        }

        if (!successfulHit) {
            interactionText.text = "";
        }
    }

    void HandleInteraction(InteractableObject interactableObj) {
        // depending on what interaction type the object has, perform the interaction
        switch (interactableObj.interactionType) {
            case InteractableObject.InteractionType.Click:
                if (Input.GetKeyDown(interactableObj.key))
                {
                    string interactionLog;
                    if (interactableObj is CompanionScript)
                    {
                        interactionLog = interactableObj.OnInteract(dialogueBox);
                    }
                    else
                    {
                        interactionLog = interactableObj.OnInteract(gameObject);
                    }
                    
                    GetComponent<GameJournal>().addJournalLog(interactionLog);
                }
                break;
            
            default:
                throw new System.Exception("Unsupported type of interactable.");
        }
    }
}
