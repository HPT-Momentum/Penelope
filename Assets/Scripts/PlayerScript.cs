using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

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

    public CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

	private bool isMenuOpen = false;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
			// these objects need to be enabled for the specific player object
            playerCamera.SetActive(true);
            playerHUD.SetActive(true);

			Waypoint[] waypoints = GameObject.FindObjectsOfType<Waypoint>();
			foreach(Waypoint waypoint in waypoints)
				GetComponentInChildren<CompassScript>().AddWaypoint(waypoint);
        }
    }

    void Update()
    {
        if (IsOwner)
        {
            UpdatePlayerMovement();
            // if(!NetworkManager.Singleton.IsServer)
            transform.position = Position.Value;

			if (Input.GetKeyDown(KeyCode.Escape))
        	{
				if (!isMenuOpen)
					GetComponent<PopUpMenu>().Open();
				else
					GetComponent<PopUpMenu>().Close();

				isMenuOpen = !isMenuOpen;
				GetComponent<PlayerCameraScript>().PauseMouse(isMenuOpen);
			}
        }
    }

	public void UpdatePlayerMovement() 
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

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
        }
        
        velocity.y += gravityForce * Time.deltaTime;
        Vector3 playerVerticalMovement = velocity * Time.deltaTime;

        if (NetworkManager.Singleton.IsServer){
            SubmitPositionToClientRpc(playerHorizontalMovement, playerVerticalMovement);
        } else {
            SubmitPositionToServerRpc(playerHorizontalMovement, playerVerticalMovement);
        }
	}

    [ServerRpc]
    void SubmitPositionToServerRpc(Vector3 playerHorizontalMovement = default, Vector3 playerVerticalMovement = default, ServerRpcParams rpcParams = default)
    {
        controller.Move(playerHorizontalMovement);
        controller.Move(playerVerticalMovement);
        Position.Value = controller.transform.position;
    }

    [ClientRpc]
    void SubmitPositionToClientRpc(Vector3 playerHorizontalMovement = default, Vector3 playerVerticalMovement = default, ClientRpcParams rpcParams = default)
    {
        controller.Move(playerHorizontalMovement);
        controller.Move(playerVerticalMovement);
        Position.Value = controller.transform.position;
        //NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerScript>().Position.Value = Position.Value; Werkt niet, had verwacht van wel
    }
}
