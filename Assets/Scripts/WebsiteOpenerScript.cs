using UnityEngine;

public class WebsiteOpenerScript : InteractableObject
{
    public string websiteName;
    public string url;

    public override string GetDescription()
    {
        return $"Press [{key}] to open {websiteName}";
    }

    public override string OnInteract(GameObject playerObject)
    {
        Application.OpenURL(url);

        return websiteName;
    }
}