using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpMenu : MonoBehaviour
{
    public GameObject popUpMenu;
    public GameObject playerObject;

    public Button buddyButton;
    public Button gdriveButton;
    public Button gscholarButton;
    
    private GameObject buddy;
    private GameObject gdrive;
    private GameObject gscholar;
    

    void Start()
    {
        buddy = GameObject.Find("Buddy");
        gdrive = GameObject.Find("GDrive");
        gscholar = GameObject.Find("GScholar");
        
        buddyButton.onClick.AddListener(() => TeleportPlayerToObject(buddy));
        gdriveButton.onClick.AddListener(() => TeleportPlayerToObject(gdrive));
        gscholarButton.onClick.AddListener(() => TeleportPlayerToObject(gscholar));
    }
    
    public void OpenMenu(){
        popUpMenu.SetActive(true);
    }
    
    public void CloseMenu(){
        popUpMenu.SetActive(false);
    }

    private void TeleportPlayerToObject(GameObject targetObject)
    {
        var offset = targetObject.transform.position - playerObject.transform.position;
        playerObject.GetComponent<PlayerScript>().UpdatePlayerMovement(offset);
        
        CloseMenu();
    }
}
