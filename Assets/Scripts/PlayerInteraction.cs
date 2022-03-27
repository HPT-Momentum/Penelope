using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction: MonoBehaviour {

    public float interactionDistance = 2f;

    public TMPro.TextMeshProUGUI interactionText;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }
    
    void Update() {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;

        bool successfulHit = false;

        if (Physics.Raycast(ray, out hit, interactionDistance)) {
            InteractableObject interactableObj = hit.collider.GetComponent<InteractableObject>();

            if (interactableObj != null) {
                HandleInteraction(interactableObj);
                interactionText.text = interactableObj.GetDescription();
                successfulHit = true;
            }
        }

        // if we miss, hide the UI
        if (!successfulHit) {
            interactionText.text = "";
        }
    }

    void HandleInteraction(InteractableObject interactableObj) {
        
        switch (interactableObj.interactionType) {
            case InteractableObject.InteractionType.Click:
                // interaction type is click and we clicked the button -> interact
                if (Input.GetKeyDown(interactableObj.key)) {
                    interactableObj.OnInteract();
                }
                break;
            
            default:
                throw new System.Exception("Unsupported type of interactable.");
        }
    }
}
