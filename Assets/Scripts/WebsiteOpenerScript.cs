using UnityEngine;

public class WebsiteOpenerScript : InteractableObject
{
    public string websiteName;
    public string url;

    public override string GetDescription()
    {
        return $"Press [{key}] to open {websiteName}";
    }

        public override void OnInteract(GameObject dialogueBox)
    {
        Application.OpenURL(url);
    }
}