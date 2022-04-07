using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebsiteOpener : InteractableObject
{
    public string websiteName;
    public string url;

    public override string GetDescription()
    {
        return $"Press [{key}] to go to {websiteName}";
    }
    
    public override void OnInteract()
    {
        Application.OpenURL(url);
        Debug.Log("is this working?");
        // OpenURL();
    }
}

