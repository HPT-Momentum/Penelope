using UnityEngine;

public class PortalScript : InteractableObject
{
    public string destinationName;
    public GameObject targetPosition;

    public override string GetDescription()
    {
        return $"Druk op [{key}] om naar {destinationName} te gaan";
    }

    public override string OnInteract(GameObject playerObject)
    {
        // update the player position to the object
        playerObject.GetComponent<PlayerScript>().TeleportToPosition(targetPosition.transform.position);

        return destinationName;
    }
}