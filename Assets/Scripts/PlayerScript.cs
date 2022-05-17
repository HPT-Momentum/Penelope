using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System;

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

    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    
    // Agora variables
    public string agoraAppID;
    public string channelName;
    public string channelToken;
    public uint playerUid;
    public GameObject playerFace;

    public CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

	private bool isMenuOpen = false;

    public override void OnNetworkSpawn()
    {
	    if (IsOwner)
        {
	        playerUid = Convert.ToUInt32(NetworkManager.Singleton.LocalClientId);
	        
	        // attach a video call script for the local player object
	        var videoCallScript = gameObject.AddComponent<VideoCallScript>();
	        videoCallScript.Setup(agoraAppID, channelName, channelToken, playerUid);
	        
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
        if (IsOwner)
        {
            CalculatePlayerMovement();
            transform.position = Position.Value;

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

	public void CalculatePlayerMovement() 
	{
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = 0f;
        }
        
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 movementDirection = transform.right * x + transform.forward * y;
        Vector3 playerHorizontalMovement = movementDirection * movementSpeed * Time.deltaTime;

		UpdatePlayerMovement(playerHorizontalMovement);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
        }
        
        velocity.y += gravityForce * Time.deltaTime;
        Vector3 playerVerticalMovement = velocity * Time.deltaTime;
		
		UpdatePlayerMovement(playerVerticalMovement);
	}

	public void UpdatePlayerMovement(Vector3 playerMovement) {
        if (NetworkManager.Singleton.IsServer){
            SubmitPositionToClientRpc(playerMovement);
        } else {
            SubmitPositionToServerRpc(playerMovement);
        }
	}

    [ServerRpc]
    void SubmitPositionToServerRpc(Vector3 playerMovement = default, ServerRpcParams rpcParams = default)
    {
        controller.Move(playerMovement);
        Position.Value = controller.transform.position;
    }

    [ClientRpc]
    void SubmitPositionToClientRpc(Vector3 playerMovement = default, ClientRpcParams rpcParams = default)
    {
        controller.Move(playerMovement);
        try {
			Position.Value = controller.transform.position;
        }
        catch {
            // Otherwise keeps whining about not being able to write networkvariable, however it doesn't work without l107
        }
        //NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerScript>().Position.Value = Position.Value; Werkt niet, had verwacht van wel
    }
}
