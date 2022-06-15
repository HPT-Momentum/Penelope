using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    // set the key that is used to interact with the object
    public KeyCode key = KeyCode.E;

    public enum InteractionType
    {
        Click,
        Hold
    }

    float holdTime;

    public InteractionType interactionType;

    public abstract string GetDescription();
    public abstract string OnInteract(GameObject playerObject);

    // methods for interaction type Hold
    public float GetHoldTime() => holdTime;
    public void IncreaseHoldTime() => holdTime += Time.deltaTime;
    public void ResetHoldTime() => holdTime = 0f;
}