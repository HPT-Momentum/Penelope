using UnityEngine;

public class WebsiteOpenerScript : InteractableObject
{
    public string websiteName;
    public string url;

    public override string GetDescription()
    {
        return $"Druk op [{key}] om {websiteName} te openen";
    }

        //Wanneer er geinteracteerd wordt met het GameObject open de URL en maak een log voor de game journal
        public override string OnInteract(GameObject dialogueBox)
    {
        Application.OpenURL(url);

        return $"Je hebt {websiteName} gebruikt om ";
    }
}