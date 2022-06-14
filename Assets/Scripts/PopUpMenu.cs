using UnityEngine;
using UnityEngine.UI;

public class PopUpMenu : MonoBehaviour
{
    public GameObject popUpMenu;
    public GameObject playerObject;

    public Button museumButton;
    public Button forumButton;
    public Button stageButton;
    public Button personalButton;
    
    private GameObject museum;
    private GameObject forum;
    private GameObject stage;
    private GameObject personalSpace;

    void Start()
    {
        museum = GameObject.Find("MuseumPortalAnchor");
        forum = GameObject.Find("ForumPortalAnchor");
        stage = GameObject.Find("StagePortalAnchor");
        personalSpace = GameObject.Find("PersonalPortalAnchor");
        
        museumButton.onClick.AddListener(() => TeleportPlayerToObject(museum));
        forumButton.onClick.AddListener(() => TeleportPlayerToObject(forum));
        stageButton.onClick.AddListener(() => TeleportPlayerToObject(stage));
        personalButton.onClick.AddListener(() => TeleportPlayerToObject(personalSpace));
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