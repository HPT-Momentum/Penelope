using Unity.Netcode;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.Serialization;

public class PlayerScript : NetworkBehaviour
{
    public float movementSpeed = 10f;
    public float jumpHeight = 5f;
    public float gravityForce = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

	public GameObject playerCamera;
	public GameObject playerHUD;

	public NetworkVariable<Vector3> playerMovement = new NetworkVariable<Vector3>();
	public NetworkVariable<uint> playerUid = new NetworkVariable<uint>();
	private bool isPlayerIdUpdated = false;
    
    // Agora variables
    public string agoraAppID;
    public string channelName;
    public string channelToken;
    public GameObject playerFace;
    public TextMeshPro playerIdText;
    private VideoCallScript videoCallScript;

    public CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private bool isMenuOpen = false;

    public override void OnNetworkSpawn()
    {
	    if (IsServer)
	    {
		    var newPlayerUid = Convert.ToUInt32(OwnerClientId);
		    playerUid.Value = newPlayerUid;
	    }

	    if (IsOwner)
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
	    
        if (IsOwner)
        {
	        // calculate the current movement of the local player
	        CalculatePlayerMovement();
	        
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
	    
        // update the position of the player object across the network
        controller.Move(playerMovement.Value);
    }

	public void CalculatePlayerMovement() 
	{
		float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // calculate the horizontal movement
        Vector3 movementDirection = transform.right * x + transform.forward * z;
        Vector3 playerHorizontalMovement = movementDirection * movementSpeed * Time.deltaTime;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0f)
        {
	        velocity.y = 0f;
        }
        
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
        }
        
        // calculate the vertical movement
        velocity.y += gravityForce * Time.deltaTime;
        Vector3 playerVerticalMovement = velocity * Time.deltaTime;

        var newPlayerMovement = playerHorizontalMovement + playerVerticalMovement;
        SubmitMovementToServerRpc(newPlayerMovement);
	}
	
	[ServerRpc]
	void SubmitPlayerUidToServerRpc(uint newPlayerUid)
	{
		playerUid.Value = newPlayerUid;
	}

    [ServerRpc]
    public void SubmitMovementToServerRpc(Vector3 newPlayerMovement = default)
    { 
	    playerMovement.Value = newPlayerMovement;
    }
}
