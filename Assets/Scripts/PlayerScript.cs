using Unity.Netcode;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.Serialization;

public class PlayerScript : NetworkBehaviour
{
	public GameObject playerCamera;
	public GameObject playerHUD;

	public NetworkVariable<uint> playerUid = new NetworkVariable<uint>();
	private bool isPlayerIdUpdated = false;
    
    // Agora variables
    public string agoraAppID;
    public string channelName;
    public string channelToken;
    public GameObject playerFace;
    public TextMeshPro playerIdText;
    private VideoCallScript videoCallScript;

    private bool isMenuOpen = false;

    public override void OnNetworkSpawn()
    {
	    if (IsServer)
	    {
		    var newPlayerUid = Convert.ToUInt32(OwnerClientId);
		    playerUid.Value = newPlayerUid;
	    }

	    if (IsClient && IsOwner)
        {
	        var newPlayerUid = Convert.ToUInt32(OwnerClientId);
	        SubmitPlayerUidToServerRpc(newPlayerUid);
	        
	        // attach a video call script for the local player object
	        videoCallScript = gameObject.AddComponent<VideoCallScript>();
	        videoCallScript.Setup(agoraAppID, channelName, channelToken);

	        // these objects need to be enabled for the specific player object
            playerCamera.SetActive(true);
            playerHUD.SetActive(true);

			Waypoint[] waypoints = FindObjectsOfType<Waypoint>();
			foreach(Waypoint waypoint in waypoints)
				GetComponentInChildren<CompassScript>().AddWaypoint(waypoint);
        }
    }

    void Update()
    {
	    if(!isPlayerIdUpdated)
	    {
		    playerIdText.text = playerUid.Value.ToString();

		    if (IsOwner)
		    {
			    videoCallScript.Join(playerUid.Value);
		    }

		    isPlayerIdUpdated = true;
	    }
	    
        if (IsClient && IsOwner)
        {
	        // check whether or not to open the menu
			if (Input.GetKeyDown(KeyCode.Escape))
        	{
				if (!isMenuOpen)
					GetComponent<PopUpMenu>().OpenMenu();
				else
					GetComponent<PopUpMenu>().CloseMenu();
			}
			isMenuOpen = GetComponent<PopUpMenu>().popUpMenu.activeSelf;
			GetComponent<PlayerCameraScript>().PauseMouse(isMenuOpen);
        }
    }
	
	[ServerRpc]
	void SubmitPlayerUidToServerRpc(uint newPlayerUid)
	{
		playerUid.Value = newPlayerUid;
	}
}
