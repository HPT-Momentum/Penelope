using UnityEngine;

public class PortalScript : InteractableObject
{
    public string destinationName;
    public GameObject targetPosition;

    public override string GetDescription()
    {
        return $"Press [{key}] to go to {destinationName}";
    }

    public override string OnInteract(GameObject playerObject)
    {
        // update the player position to the object
        playerObject.GetComponent<PlayerScript>().TeleportToPosition(targetPosition.transform.position);

        return destinationName;
    }
}